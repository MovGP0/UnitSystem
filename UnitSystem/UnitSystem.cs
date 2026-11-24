using System.Collections;
using System.Collections.ObjectModel;
using ShInUeXx.Numerics;
using UnitSystem.Dimensions;
using UnitSystem.Extensions;
using UnitSystem.Presentation;

namespace UnitSystem;

internal sealed class UnitSystem : IUnitSystem
{
    private readonly object _gate = new();

    private readonly List<KnownUnit> _baseUnits = new();

    private readonly Dictionary<(double Factor, double Offset, Dimension Dimension), KnownUnit> _units = new();

    private readonly Dictionary<string, KnownUnit> _unitsBySymbol = new(StringComparer.Ordinal);

    private readonly Dictionary<int, string> _dimensionSymbols = new();

    private readonly HashSet<Dimension> _affineDimensions = new();

    private readonly IUnitFactory _unitFactory = new UnitFactory();

    private UnitInterpreter? _interpreter;

    private bool _frozen;

    public UnitSystem(string name)
    {
        Check.Argument(name, nameof(name)).IsNotNull();
        Name = name;
        NoUnit = new DerivedUnit(this, 1.0, 0.0, Dimension.DimensionLess);
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public int NumberOfDimensions
    {
        get
        {
            lock (_gate)
                return _baseUnits.Count;
        }
    }

    /// <inheritdoc />
    public Unit NoUnit { get; }

    /// <inheritdoc />
    public IEnumerable<KnownUnit> BaseUnits
    {
        get
        {
            lock (_gate)
                return _baseUnits.ToArray();
        }
    }

    /// <inheritdoc />
    public KnownUnit? this[string key]
    {
        get
        {
            lock (_gate)
            {
                _unitsBySymbol.TryGetValue(key, out var unit);
                return unit;
            }
        }
    }

    /// <inheritdoc />
    public Unit AddBaseUnit(string symbol, string name, string inherentPrefix = "", string dimensionSymbol = "")
    {
        lock (_gate)
        {
            EnsureMutable();
            var exponents = new Rational[_baseUnits.Count + 1];
            Array.Fill(exponents, Rational.Zero);
            exponents[^1] = Rational.One;
            var unit = new KnownUnit(
                this,
                1.0,
                0.0,
                new Dimension(exponents),
                symbol,
                name,
                inherentPrefix);
            if (unit.BaseSymbol != unit.Symbol && _baseUnits.Any(existing =>
                    existing.BaseSymbol != existing.Symbol && existing.BaseSymbol == unit.BaseSymbol))
            {
                throw new InvalidOperationException(Messages.UnitSymbolAlreadyKnown.FormatWith(unit.BaseSymbol));
            }

            RegisterKnownUnit(unit);
            _baseUnits.Add(unit);
            _dimensionSymbols.Add(exponents.Length, dimensionSymbol);
            return unit;
        }
    }

    /// <inheritdoc />
    public Unit AddDerivedUnit(string symbol, string name, Unit unit, double offset = 0.0)
    {
        lock (_gate)
        {
            EnsureMutable();
            Check.UnitsAreFromSameSystem(NoUnit, unit);
            var derived = _unitFactory.CreateUnit(
                this,
                unit.Factor,
                offset,
                unit.Dimension,
                symbol,
                name,
                string.Empty);
            RegisterKnownUnit(derived);
            return derived;
        }
    }

    internal void AddAlias(string symbol, KnownUnit unit)
    {
        lock (_gate)
        {
            EnsureMutable();
            Check.UnitsAreFromSameSystem(NoUnit, unit);
            EnsureSymbolAvailable(symbol);
            _unitsBySymbol.Add(symbol, unit);
            _interpreter = null;
        }
    }

    internal bool IsAffineDimension(Dimension dimension)
    {
        lock (_gate)
            return _affineDimensions.Contains(dimension);
    }

    internal void Freeze()
    {
        lock (_gate)
        {
            _interpreter ??= new UnitInterpreter(this, new UnitDialect());
            _frozen = true;
        }
    }

    /// <inheritdoc />
    public Unit CreateUnit(double factor, double offset, Dimension dimension)
    {
        lock (_gate)
        {
            if (_units.TryGetValue((factor, offset, dimension), out var known))
            {
                return known;
            }

            if (factor.Equals(1.0) && offset.Equals(0.0) && dimension.Count == 0)
            {
                return NoUnit;
            }

            return _unitFactory.CreateUnit(this, factor, offset, dimension);
        }
    }

    /// <inheritdoc />
    public Unit CreateUnit(double factor, Dimension dimension) => CreateUnit(factor, 0.0, dimension);

    /// <inheritdoc />
    public Unit Parse(string unitExpression)
    {
        lock (_gate)
        {
            _interpreter ??= new UnitInterpreter(this, new UnitDialect());
            return _interpreter.Parse(unitExpression);
        }
    }

    /// <inheritdoc />
    public string Display(Unit unit)
    {
        Check.UnitsAreFromSameSystem(NoUnit, unit);
        lock (_gate)
        {
            _interpreter ??= new UnitInterpreter(this, new UnitDialect());
            return _interpreter.Display(unit);
        }
    }

    /// <inheritdoc />
    public Unit MakeCoherent(Unit unit)
    {
        Check.UnitsAreFromSameSystem(NoUnit, unit);
        return unit.IsCoherent ? unit : CreateUnit(1.0, unit.Dimension);
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<int, string> DimensionSymbols
    {
        get
        {
            lock (_gate)
                return new ReadOnlyDictionary<int, string>(new Dictionary<int, string>(_dimensionSymbols));
        }
    }

    /// <inheritdoc />
    public IEnumerator<KnownUnit> GetEnumerator()
    {
        lock (_gate)
            return ((IEnumerable<KnownUnit>)_units.Values.ToArray()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void RegisterKnownUnit(KnownUnit unit)
    {
        EnsureSymbolAvailable(unit.Symbol);
        var key = (unit.Factor, unit.Offset, unit.Dimension);
        if (_units.TryGetValue(key, out var collision))
        {
            throw new InvalidOperationException(Messages.UnitAlreadyKnown.FormatWith(unit, collision));
        }

        _units.Add(key, unit);
        _unitsBySymbol.Add(unit.Symbol, unit);
        if (!unit.IsAbsoluteUnit)
        {
            _affineDimensions.Add(unit.Dimension);
        }

        _interpreter = null;
    }

    private void EnsureSymbolAvailable(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol) || symbol == "1"
            || symbol.Trim().StartsWith(UnitInterpreter.CanonicalPrefix, StringComparison.Ordinal))
        {
            throw new ArgumentException("A unit requires a nonempty, nonreserved symbol.", nameof(symbol));
        }

        if (_unitsBySymbol.ContainsKey(symbol))
        {
            throw new InvalidOperationException(Messages.UnitSymbolAlreadyKnown.FormatWith(symbol));
        }
    }

    private void EnsureMutable()
    {
        if (_frozen)
        {
            throw new InvalidOperationException("The standard unit catalog is read-only.");
        }
    }
}
