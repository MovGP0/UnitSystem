using UnitSystem.Extensions;
using UnitSystem.Prefixes;

namespace UnitSystem.Presentation;

internal sealed class UnitInterpreter
{
    private readonly IUnitSystem _system;
    private readonly KnownUnit[] _baseUnits;
    private readonly Dictionary<string, KnownUnit> _prefixedBases;
    private readonly MetricPrefix[] _prefixes;
    private readonly string[] _division;
    private readonly string _multiplication;
    private readonly string[] _explicitMultiplication;
    private readonly int _maxSymbolWords;
    private readonly string[] _exponentiation;

    public UnitInterpreter(IUnitSystem system, IUnitDialect dialect)
    {
        _system = system;
        _baseUnits = system.BaseUnits.ToArray();
        _prefixedBases = _baseUnits.Where(unit => unit.BaseSymbol != unit.Symbol)
            .ToDictionary(unit => unit.BaseSymbol, StringComparer.Ordinal);
        _prefixes = MetricPrefix.GetAll().Where(prefix => prefix.Symbol.Length != 0)
            .OrderByDescending(prefix => prefix.Symbol.Length).ToArray();
        _division = (string[])dialect.Division;
        _multiplication = dialect.Multiplication.First();
        _explicitMultiplication = dialect.Multiplication.Where(token => token != " ").ToArray();
        _maxSymbolWords = system.Select(unit => unit.Symbol.Count(c => c == ' ') + 1).DefaultIfEmpty(1).Max();
        _exponentiation = (string[])dialect.Exponentiation;
    }

    public string Display(Unit unit)
    {
        if (!unit.IsCoherent && unit is KnownUnit known && _system[known.Symbol] == unit)
        {
            return known.Symbol;
        }

        if (unit.IsAbsoluteUnit && !unit.IsCoherent && TryDisplayPrefixedUnit(unit) is { } prefixed)
        {
            return prefixed;
        }

        var numerator = new List<string>();
        var denominator = new List<string>();

        for (var i = 0; i < unit.Dimension.Count; i++)
        {
            var exponent = unit.Dimension[i];
            if (exponent > 0)
            {
                numerator.Add(DisplayFactor(_baseUnits[i].Symbol, exponent));
            }
            else if (exponent < 0)
            {
                denominator.Add(DisplayFactor(_baseUnits[i].Symbol, -exponent));
            }
        }

        if (numerator.Count == 0 && denominator.Count == 0)
        {
            return unit.IsCoherent ? string.Empty : DisplayTransform(unit);
        }

        var expression = numerator.Count == 0 ? "1" : string.Join(_multiplication, numerator);
        if (denominator.Count != 0)
        {
            expression += " " + _division[0] + " " + string.Join(_multiplication, denominator);
        }

        return unit.IsCoherent ? expression : DisplayTransform(unit) + " " + expression;
    }

    private string? TryDisplayPrefixedUnit(Unit unit)
    {
        foreach (var baseUnit in _baseUnits)
        {
            if (!unit.Dimension.Equals(baseUnit.Dimension)) continue;

            foreach (var prefix in _prefixes)
            {
                var symbol = prefix.Symbol + baseUnit.BaseSymbol;
                var exact = _system[symbol];
                if (exact is not null ? exact == unit : (prefix.Factor / baseUnit.InherentFactor).Equals(unit.Factor))
                {
                    return symbol;
                }
            }
        }

        return null;
    }

    private string DisplayFactor(string symbol, float exponent)
        => exponent == 1 ? symbol : symbol + _exponentiation[0] + exponent.ToString("R", CultureInfo.InvariantCulture);

    private static string DisplayTransform(Unit unit)
    {
        var factor = unit.Factor.ToString("R", CultureInfo.InvariantCulture);
        return unit.IsAbsoluteUnit ? factor : factor + "@" + unit.Offset.ToString("R", CultureInfo.InvariantCulture);
    }

    public Unit Parse(string unitExpression)
    {
        Check.Argument(unitExpression, nameof(unitExpression)).IsNotNull();
        var expression = unitExpression.Trim();
        if (expression.Length == 0) return _system.NoUnit;
        if (_system[expression] is { } known) return known;

        double factor = 1, offset = 0;
        var separator = expression.IndexOf(' ');
        var firstToken = separator < 0 ? expression : expression.Substring(0, separator);
        if (TryParseTransform(firstToken, out var parsedFactor, out var parsedOffset))
        {
            factor = parsedFactor;
            offset = parsedOffset;
            expression = separator < 0 ? "1" : expression.Substring(separator + 1).TrimStart();
            if (_division.Any(token => expression.StartsWith(token, StringComparison.Ordinal)))
            {
                expression = "1 " + expression;
            }
        }

        var fraction = expression.Split(_division, StringSplitOptions.None);
        if (fraction.Length > 2 || fraction.Any(string.IsNullOrWhiteSpace))
        {
            throw InvalidExpression(unitExpression);
        }

        var unit = ParseProduct(fraction[0].Trim());
        if (fraction.Length == 2)
        {
            unit /= ParseProduct(fraction[1].Trim());
        }

        if (factor.Equals(1.0) && offset.Equals(0.0)) return unit;
        Check.EnsureAbsoluteUnit(unit);
        try
        {
            return _system.CreateUnit(factor * unit.Factor, offset, unit.Dimension);
        }
        catch (ArgumentOutOfRangeException exception)
        {
            throw new FormatException(Messages.UnitExpressionInvalid.FormatWith(unitExpression), exception);
        }
    }

    private static bool TryParseTransform(string token, out double factor, out double offset)
    {
        offset = 0;
        var separator = token.IndexOf('@');
        if (separator < 0)
        {
            return double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out factor);
        }

        return double.TryParse(token.Substring(0, separator), NumberStyles.Float, CultureInfo.InvariantCulture, out factor)
            && double.TryParse(token.Substring(separator + 1), NumberStyles.Float, CultureInfo.InvariantCulture, out offset);
    }

    private Unit ParseProduct(string expression)
    {
        if (TryParseTerm(expression) is { } single) return single;

        Unit? product = null;
        foreach (var segment in expression.Split(_explicitMultiplication, StringSplitOptions.None))
        {
            var words = segment.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) throw InvalidExpression(expression);

            for (var index = 0; index < words.Length;)
            {
                var term = ParseNextTerm(words, ref index);
                product = product is null ? term : product * term;
            }
        }

        return product ?? throw InvalidExpression(expression);
    }

    private Unit ParseNextTerm(string[] words, ref int index)
    {
        // Try the longest symbol first so symbols such as "us gal" work inside products.
        for (var count = Math.Min(_maxSymbolWords, words.Length - index); count > 0; count--)
        {
            if (TryParseTerm(string.Join(" ", words, index, count)) is { } term)
            {
                index += count;
                return term;
            }
        }

        throw InvalidExpression(words[index]);
    }

    private Unit? TryParseTerm(string expression)
    {
        if (TryParseSymbol(expression) is { } unit) return unit;

        var power = expression.Split(_exponentiation, StringSplitOptions.None);
        if (power.Length == 2 && TryParseSymbol(power[0]) is { } basis
            && float.TryParse(power[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var exponent)
            && !float.IsNaN(exponent) && !float.IsInfinity(exponent))
        {
            return basis ^ exponent;
        }

        return null;
    }

    private Unit? TryParseSymbol(string symbol)
    {
        if (symbol == "1") return _system.NoUnit;
        if (_system[symbol] is { } known) return known;
        if (_prefixedBases.TryGetValue(symbol, out var prefixedBase))
        {
            return prefixedBase / prefixedBase.InherentFactor;
        }

        foreach (var prefix in _prefixes)
        {
            if (!symbol.StartsWith(prefix.Symbol, StringComparison.Ordinal)) continue;

            var suffix = symbol.Substring(prefix.Symbol.Length);
            if (_prefixedBases.TryGetValue(suffix, out prefixedBase))
            {
                return _system.CreateUnit(prefix.Factor / prefixedBase.InherentFactor, prefixedBase.Dimension);
            }

            if (_system[suffix] is { IsAbsoluteUnit: true } basis && basis.BaseSymbol == basis.Symbol)
            {
                return prefix * basis;
            }
        }

        return null;
    }

    private static FormatException InvalidExpression(string expression)
        => new(Messages.UnitExpressionInvalid.FormatWith(expression));
}
