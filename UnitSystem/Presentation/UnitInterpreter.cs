using System.Text;
using System.Text.RegularExpressions;
using UnitSystem.Extensions;
using UnitSystem.Prefixes;

namespace UnitSystem.Presentation;

/// <summary>
/// Represents a unit interpreter that parses and formats units according to a specified unit system and dialect.
/// </summary>
internal class UnitInterpreter
{
    /// <summary>
    /// The list of base units in the unit system.
    /// </summary>
    private readonly List<KnownUnit> _baseUnits;

    /// <summary>
    /// The dialect used for parsing and formatting units.
    /// </summary>
    private readonly IUnitDialect _dialect;

    /// <summary>
    /// A cache for parsed units to optimize repeated parsing operations.
    /// </summary>
    private readonly Dictionary<string, Unit> _parseCache = new();

    /// <summary>
    /// A dictionary of known units with prefixes mapped to their base symbols.
    /// </summary>
    private readonly Dictionary<string, KnownUnit> _prefixedUnits = new();

    /// <summary>
    /// The unit system that defines the units and their relationships.
    /// </summary>
    private readonly IUnitSystem _system;

    /// <summary>
    /// The regular expression pattern used to parse unit expressions.
    /// </summary>
    private readonly string _unitRegex;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitInterpreter"/> class.
    /// </summary>
    /// <param name="system">The unit system to be used by the interpreter.</param>
    /// <param name="dialect">The dialect defining how units are parsed and formatted.</param>
    public UnitInterpreter(IUnitSystem system, IUnitDialect dialect)
    {
        _system = system;
        _dialect = dialect;
        _unitRegex = BuildUnitRegex();
        _baseUnits = new List<KnownUnit>(_system.BaseUnits);

        foreach (var unit in system)
        {
            if (unit.Symbol != unit.BaseSymbol)
            {
                _prefixedUnits.Add(unit.BaseSymbol, unit);
            }
        }
    }

    /// <summary>
    /// Converts a <see cref="Unit"/> to its string representation.
    /// </summary>
    /// <param name="unit">The unit to be converted to a string.</param>
    /// <returns>A string representation of the unit.</returns>
    public string ToString(Unit unit)
    {
        var nominator = new List<string>();
        var denominator = new List<string>();

        for (var i = 0; i < unit.Dimension.Count; i++)
        {
            var exponent = unit.Dimension[i];

            if (exponent > 0)
            {
                nominator.Add(BuildFactor(_baseUnits[i].Symbol, exponent));
            }

            if (exponent < 0)
            {
                denominator.Add(BuildFactor(_baseUnits[i].Symbol, exponent));
            }
        }

        if (!nominator.Any() && !denominator.Any())
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        if (!unit.IsCoherent)
        {
            builder.Append(unit.Factor).Append(' ');
        }

        builder.Append(nominator.Count == 0 ? "1" : string.Join(_dialect.Multiplication.First(), nominator));

        if (denominator.Count != 0)
        {
            builder.Append(" {0} ".FormatWith(_dialect.Division.First()));
            builder.Append(string.Join(_dialect.Multiplication.First(), denominator));
        }

        return builder.ToString();
    }

    private const float FloatComparisonTolerance = 1e-6f;

    /// <summary>
    /// Builds a factor string representation based on the symbol and exponent.
    /// </summary>
    /// <param name="symbol">The unit symbol.</param>
    /// <param name="exponent">The exponent associated with the unit.</param>
    /// <returns>A string representation of the factor.</returns>
    private string BuildFactor(string symbol, float exponent)
    {
        var absolute = Math.Abs(exponent);

        if (Math.Abs(absolute - 1f) < FloatComparisonTolerance)
        {
            return symbol;
        }

        return "{0}{1}{2}".FormatWith(symbol, _dialect.Exponentiation.First(), absolute);
    }

    /// <summary>
    /// Parses a unit expression string and returns the corresponding <see cref="Unit"/>.
    /// </summary>
    /// <param name="unitExpression">The unit expression to be parsed.</param>
    /// <returns>The parsed unit.</returns>
    public Unit Parse(string unitExpression)
    {
        if (_parseCache.TryGetValue(unitExpression, out var unit))
        {
            return unit;
        }

        lock (_parseCache)
        {
            if (_parseCache.TryGetValue(unitExpression, out unit))
            {
                return unit;
            }

            unit = ParseCore(unitExpression);
            _parseCache.Add(unitExpression, unit);
        }

        return unit;
    }

    /// <summary>
    /// Core parsing logic for a unit expression string.
    /// </summary>
    /// <param name="unitExpression">The unit expression to be parsed.</param>
    /// <returns>The parsed unit.</returns>
    private Unit ParseCore(string unitExpression)
    {
        var fraction = unitExpression.Split(
            (string[])_dialect.Division,
            StringSplitOptions.RemoveEmptyEntries);

        if (fraction.Length == 0)
        {
            return _system.NoUnit;
        }

        if (fraction.Length == 1)
        {
            return ParseUnitProduct(fraction[0]);
        }

        if (fraction.Length > 2)
        {
            throw new FormatException(Messages.UnitExpressionInvalid.FormatWith(unitExpression));
        }

        var nominator = ParseUnitProduct(fraction[0]);
        var denominator = ParseUnitProduct(fraction[1]);

        return nominator/denominator;
    }

    /// <summary>
    /// Parses a unit product expression.
    /// </summary>
    /// <param name="unitExpression">The unit product expression to be parsed.</param>
    /// <returns>The parsed unit product.</returns>
    private Unit ParseUnitProduct(string unitExpression)
    {
        var factors = unitExpression.Split(
            (string[])_dialect.Multiplication,
            StringSplitOptions.RemoveEmptyEntries);
        var product = _system.NoUnit;

        return factors.Aggregate(product, (current, factor) => current*ParseUnit(factor));
    }

    /// <summary>
    /// Parses an individual unit expression.
    /// </summary>
    /// <param name="unitExpression">The unit expression to be parsed.</param>
    /// <returns>The parsed unit.</returns>
    /// <exception cref="FormatException">Thrown when the unit expression is invalid or ambiguous.</exception>
    private Unit ParseUnit(string unitExpression)
    {
        var matches = Regex.Matches(unitExpression, _unitRegex);

        if (matches.Count == 0)
        {
            throw new FormatException(Messages.UnitExpressionInvalid.FormatWith(unitExpression));
        }

        if (matches.Count > 1)
        {
            throw new FormatException(Messages.UnitExpressionAmbiguous.FormatWith(unitExpression));
        }

        var prefixSymbol = matches[0].Groups["prefix"].Value;
        var unitSymbol = matches[0].Groups["unit"].Value;
        var exponent = matches[0].Groups["exponent"].Value;

        var knownUnit = _system[unitSymbol];
        Unit parsedUnit;

        if (knownUnit == null)
        {
            knownUnit = _prefixedUnits[unitSymbol];
            parsedUnit = knownUnit/knownUnit.InherentFactor;
        }
        else
        {
            parsedUnit = knownUnit;
        }

        if (!string.IsNullOrEmpty(prefixSymbol))
        {
            if (MetricPrefix.TryGetValue(prefixSymbol, out var prefix))
            {
                parsedUnit *= prefix.Factor;
            }
            else
            {
                var message = string.Format(Messages.ThePrefixIsNotRecognized, prefixSymbol);
                throw new ArgumentException(message, nameof(unitExpression));
            }
        }

        if (!string.IsNullOrEmpty(exponent))
        {
            parsedUnit ^= int.Parse(exponent);
        }

        return parsedUnit;
    }

    /// <summary>
    /// Builds the unit regular expression based on the system's units and the dialect.
    /// </summary>
    /// <returns>The unit regular expression string.</returns>
    private string BuildUnitRegex()
    {
        var prefixes = string.Join("|", MetricPrefix.Prefixes.Keys);
        var units = string.Join("|", _system.Select(u => Regex.Escape(u.BaseSymbol)));
        var exponentiationOperators = string.Join("|", _dialect.Exponentiation.Select(Regex.Escape));

        var regex = @"^(?<prefix>({0}))?(?<unit>{1})({2}(?<exponent>-?\d+))?$".FormatWith(prefixes, units,
            exponentiationOperators);

        return regex;
    }
}
