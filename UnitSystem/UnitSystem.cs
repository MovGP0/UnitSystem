using System.Collections;
using System.Collections.ObjectModel;
using UnitSystem.Dimensions;
using UnitSystem.Extensions;
using UnitSystem.Presentation;

namespace UnitSystem;

internal class UnitSystem : IUnitSystem
{
    private readonly Dictionary<Dimension, KnownUnit> _baseUnits;
    private readonly Dictionary<Dimension, Unit> _coherentUnits;
    private readonly IUnitFactory _unitFactory;
    private readonly Dictionary<(double, double, Dimension), KnownUnit> _units;
    private readonly Dictionary<string, KnownUnit> _unitsBySymbol;

    public UnitSystem(string name)
        : this(name, new UnitFactory(), new UnitDialect())
    {
    }

    public UnitSystem(string name, IUnitDialect dialect)
        : this(name, new UnitFactory(), dialect)
    {
    }

    private UnitSystem(string name, IUnitFactory unitFactory, IUnitDialect dialect)
    {
        Check.Argument(name, nameof(name)).IsNotNull();
        Check.Argument(unitFactory, nameof(unitFactory)).IsNotNull();
        Check.Argument(dialect, nameof(dialect)).IsNotNull();

        _unitFactory = unitFactory;
        Name = name;
        _baseUnits = new();
        _coherentUnits = new();
        _units = new();
        _unitsBySymbol = new();
        NoUnit = new DerivedUnit(this, 1.0, 0.0, Dimension.DimensionLess);
        Interpreter = new(this, dialect);
    }

    private UnitInterpreter Interpreter { get; }

    /// <inheritdoc cref="IUnitSystem.Name"/>
    public string Name { get; }

    /// <inheritdoc cref="IUnitSystem.NumberOfDimensions"/>
    public int NumberOfDimensions { get; private set; }

    /// <inheritdoc cref="IUnitSystem.NoUnit"/>
    public Unit NoUnit { get; }

    /// <inheritdoc cref="IUnitSystem.BaseUnits"/>
    public IEnumerable<KnownUnit> BaseUnits => _baseUnits.Values;

    /// <inheritdoc cref="IUnitSystem.this[string]"/>
    public KnownUnit? this[string key]
    {
        get
        {
            _unitsBySymbol.TryGetValue(key, out var unit);
            return unit;
        }
    }

    /// <inheritdoc cref="IUnitSystem.AddBaseUnit(string, string, string, string)"/>
    public Unit AddBaseUnit(string symbol, string name, string inherentPrefix = "", string dimensionSymbol = "")
    {
        var newUnit = _unitFactory.CreateUnit(
            this,
            1.0,
            0.0,
            CreateNewBaseDimension(BaseUnits.Count(), dimensionSymbol),
            symbol,
            name,
            inherentPrefix);

        EnsureUnitIsNotRegistered(newUnit);

        _baseUnits.Add(newUnit.Dimension, newUnit);
        NumberOfDimensions++;

        RegisterKnownUnit(newUnit);

        return newUnit;
    }

    /// <inheritdoc cref="IUnitSystem.AddDerivedUnit(string, string, Unit, double)"/>
    public Unit AddDerivedUnit(string symbol, string name, Unit unit, double offset = 0.0)
    {
        var newUnit = _unitFactory.CreateUnit(
            this,
            unit.Factor,
            offset,
            unit.Dimension,
            symbol,
            name,
            string.Empty);

        EnsureUnitIsNotRegistered(newUnit);
        RegisterKnownUnit(newUnit);
        return newUnit;
    }

    /// <inheritdoc cref="IUnitSystem.CreateUnit(double, double, Dimension)"/>
    public Unit CreateUnit(double factor, double offset, Dimension dimension)
    {
        // Prefer returning a known or coherent unit

        if (_units.TryGetValue((factor, offset, dimension), out var known))
        {
            return known;
        }

        if (factor.Equals(1.0) && offset.Equals(0.0) && _coherentUnits.TryGetValue(dimension, out var coherent))
        {
            return coherent;
        }

        return _unitFactory.CreateUnit(this, factor, offset, dimension);
    }

    /// <inheritdoc cref="IUnitSystem.CreateUnit(double, Dimension)"/>
    public Unit CreateUnit(double factor, Dimension dimension) => CreateUnit(factor, 0.0, dimension);

    /// <inheritdoc cref="IUnitSystem.Parse(string)"/>
    public Unit Parse(string unitExpression) => Interpreter.Parse(unitExpression);

    /// <inheritdoc cref="IUnitSystem.Display(Unit)"/>
    public string Display(Unit unit)
    {
        Check.UnitsAreFromSameSystem(NoUnit, unit);

        return Interpreter.ToString(unit);
    }

    /// <inheritdoc cref="IUnitSystem.MakeCoherent(Unit)"/>
    public Unit MakeCoherent(Unit unit)
    {
        if (unit.IsCoherent) return unit;

        if (_coherentUnits.TryGetValue(unit.Dimension, out var coherentUnit))
        {
            return coherentUnit;
        }

        lock (_coherentUnits)
        {
            if (_coherentUnits.TryGetValue(unit.Dimension, out coherentUnit))
            {
                return coherentUnit;
            }

            coherentUnit = unit / unit.Factor;
            _coherentUnits.Add(coherentUnit.Dimension, coherentUnit);
            return coherentUnit;
        }
    }

    private void RegisterKnownUnit(KnownUnit unit)
    {
        _units.Add((unit.Factor, unit.Offset, unit.Dimension), unit);
        _unitsBySymbol.Add(unit.Symbol, unit);

        if (unit.Factor.Equals(1.0) && unit.Offset.Equals(0.0))
        {
            _coherentUnits.Add(unit.Dimension, unit);
        }
    }

    private void EnsureUnitIsNotRegistered(KnownUnit unit)
    {
        if (_units.Values.Any(u => u.Symbol == unit.Symbol))
        {
            throw new InvalidOperationException(Messages.UnitSymbolAlreadyKnown.FormatWith(unit.Symbol));
        }

        if (_units.TryGetValue((unit.Factor, unit.Offset, unit.Dimension), out var collision))
        {
            throw new InvalidOperationException(Messages.UnitAlreadyKnown.FormatWith(unit, collision));
        }
    }

    private Dimension CreateNewBaseDimension(int index, string symbol)
    {
        var exponents = new float[index + 1];
        exponents[index] = 1;

        _dimensionSymbols.Add(index + 1, symbol);
        return new(exponents);
    }

    private readonly Dictionary<int, string> _dimensionSymbols = new();

    /// <inheritdoc cref="IUnitSystem.DimensionSymbols"/>
    public IReadOnlyDictionary<int, string> DimensionSymbols => new ReadOnlyDictionary<int, string>(_dimensionSymbols);

    #region IEnumerable<KnownUnit>

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public IEnumerator<KnownUnit> GetEnumerator() => _units.Values.GetEnumerator();

    /// <inheritdoc cref="IEnumerable.GetEnumerator()"/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}
