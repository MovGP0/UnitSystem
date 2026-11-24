// ReSharper disable InconsistentNaming
namespace UnitSystem.Systems;

/// <summary>
/// Represents the International System of Units (SI) and defines base and Units.
/// </summary>
/// <remarks>
/// See <a href="https://en.wikipedia.org/wiki/International_System_of_Units">Wikipedia: International System of Units</a> and
/// <a href="https://www.bipm.org/en/publications/si-brochure">SI Brochure</a> for details.
/// </remarks>
public static class SISystem
{
    static SISystem()
    {
        _system = new UnitSystem("SI");

        // Base units
        m = _system.AddBaseUnit("m", "metre", dimensionSymbol: "L");
        kg = _system.AddBaseUnit("kg", "kilogram", "k", dimensionSymbol: "M");
        s = _system.AddBaseUnit("s", "second", dimensionSymbol: "T");
        A = _system.AddBaseUnit("A", "ampere", dimensionSymbol: "I");
        K = _system.AddBaseUnit("K", "kelvin", dimensionSymbol: "Θ");
        mol = _system.AddBaseUnit("mol", "mole", dimensionSymbol: "N");
        cd = _system.AddBaseUnit("cd", "candela", dimensionSymbol: "J");

        // Angles
        rad = _system.AddBaseUnit("rad", "radians", dimensionSymbol: "\u03B1");
        sr = _system.AddDerivedUnit("sr", "steradian", rad * rad);
        deg = _system.AddDerivedUnit("°", "degrees", rad * (Math.PI / 180d));

        // Derived units
        Hz = _system.AddDerivedUnit("Hz", "hertz", s.Pow(-1));
        N = _system.AddDerivedUnit("N", "newton", kg*m*s.Pow(-2));
        Pa = _system.AddDerivedUnit("Pa", "pascal", N*m.Pow(-2));
        J = _system.AddDerivedUnit("J", "joule", N*m);
        W = _system.AddDerivedUnit("W", "watt", J/s);
        C = _system.AddDerivedUnit("C", "coulomb", s*A);
        V = _system.AddDerivedUnit("V", "volt", W/A);
        F = _system.AddDerivedUnit("F", "farad", C/V);
        Ω = _system.AddDerivedUnit("Ω", "ohm", V/A);
        S = _system.AddDerivedUnit("S", "siemens", A/V);
        Wb = _system.AddDerivedUnit("Wb", "weber", V*s);
        T = _system.AddDerivedUnit("T", "tesla", Wb / m.Pow(2));
        H = _system.AddDerivedUnit("H", "inductance", Wb/A);
        lm = _system.AddDerivedUnit("lm", "lumen", cd * sr);
        lx = _system.AddDerivedUnit("lx", "illuminance", lm / m.Pow(2));
        Sv = _system.AddDerivedUnit("Sv", "sievert", J/kg);
        kat = _system.AddDerivedUnit("kat", "katal", s.Pow(-1)*mol);
        degC = _system.AddDerivedUnit("°C", "degrees Celsius", K, 273.15);

        // Incoherent units
        h = _system.AddDerivedUnit("h", "hour", 60*60*s);
        min = _system.AddDerivedUnit("min", "minute", 60*s);
        CGSSystem.Register(_system);
        ImperialSystem.Register(_system);
        _system.Freeze();
    }

    #region IUnitSystem

    /// <inheritdoc cref="IUnitSystem.Name"/>
    public static string Name => _system.Name;

    /// <inheritdoc cref="IUnitSystem.NumberOfDimensions"/>
    public static int NumberOfDimensions => _system.NumberOfDimensions;

    /// <inheritdoc cref="IUnitSystem.BaseUnits"/>
    public static IEnumerable<KnownUnit> BaseUnits => _system.BaseUnits;

    /// <inheritdoc cref="IUnitSystem.NoUnit"/>
    public static Unit NoUnit => _system.NoUnit;

    /// <inheritdoc cref="IUnitSystem.AddBaseUnit(string, string, string, string)"/>
    public static Unit AddBaseUnit(string symbol, string name, string inherentPrefix = "", string dimensionSymbol = "")
        => _system.AddBaseUnit(symbol, name, inherentPrefix, dimensionSymbol);

    /// <inheritdoc cref="IUnitSystem.AddDerivedUnit(string, string, Unit, double)"/>
    public static Unit AddDerivedUnit(string symbol, string name, Unit unit)
        => _system.AddDerivedUnit(symbol, name, unit);

    /// <inheritdoc cref="IUnitSystem.Parse(string)"/>
    public static Unit Parse(string unitExpression) => _system.Parse(unitExpression);

    /// <inheritdoc cref="IUnitSystem.Display(Unit)"/>
    public static string Display(Unit unit) => _system.Display(unit);

    /// <inheritdoc cref="IUnitSystem.MakeCoherent(Unit)"/>
    public static Unit MakeCoherent(Unit unit) => _system.MakeCoherent(unit);

    /// <inheritdoc cref="IUnitSystem.DimensionSymbols"/>
    public static IReadOnlyDictionary<int, string> DimensionSymbols => _system.DimensionSymbols;

    #endregion

    private static readonly UnitSystem _system;

    /// <summary>
    /// International System of Units
    /// </summary>
    public static IUnitSystem System => _system;

    #region Base units

    /// <summary>
    /// length
    /// </summary>
    public static Unit m { get; } = null!;

    /// <summary>
    /// mass
    /// </summary>
    public static Unit kg { get; } = null!;

    /// <summary>
    /// time
    /// </summary>
    public static Unit s { get; } = null!;

    /// <summary>
    /// electric current
    /// </summary>
    public static Unit A { get; } = null!;

    /// <summary>
    /// thermodynamic temperature
    /// </summary>
    public static Unit K { get; } = null!;

    /// <summary>
    /// amount of substance
    /// </summary>
    public static Unit mol { get; } = null!;

    /// <summary>
    /// luminous intensity
    /// </summary>
    public static Unit cd { get; } = null!;

    /// <summary>
    /// angle
    /// </summary>
    public static Unit rad { get; } = null!;

    /// <summary>
    /// spherical angle
    /// </summary>
    public static Unit sr { get; } = null!;

    /// <summary>
    /// angle
    /// </summary>
    public static Unit deg { get; } = null!;

    #endregion

    #region Derived units

    /// <summary>
    /// luminous flux
    /// </summary>
    public static Unit lm { get; } = null!;

    /// <summary>
    /// frequency
    /// </summary>
    public static Unit Hz { get; } = null!;

    /// <summary>
    /// force, weight
    /// </summary>
    public static Unit N { get; } = null!;

    /// <summary>
    /// pressure, stress
    /// </summary>
    public static Unit Pa { get; } = null!;

    /// <summary>
    /// energy, work, heat
    /// </summary>
    public static Unit J { get; } = null!;

    /// <summary>
    /// power, radiant flux
    /// </summary>
    public static Unit W { get; } = null!;

    /// <summary>
    /// electric charge, quantity of electricity
    /// </summary>
    public static Unit C { get; } = null!;

    /// <summary>
    /// voltage (electrical potential difference), electromotive force
    /// </summary>
    public static Unit V { get; } = null!;

    /// <summary>
    /// electric capacitance
    /// </summary>
    public static Unit F { get; } = null!;

    /// <summary>
    /// electric resistance, impedance, reactance
    /// </summary>
    public static Unit Ω { get; } = null!;

    /// <summary>
    /// electrical conductance
    /// </summary>
    public static Unit S { get; } = null!;

    /// <summary>
    /// magnetic flux
    /// </summary>
    public static Unit Wb { get; } = null!;

    /// <summary>
    /// magnetic field strength
    /// </summary>
    public static Unit T { get; } = null!;

    /// <summary>
    /// inductance
    /// </summary>
    public static Unit H { get; } = null!;

    /// <summary>
    /// illuminance
    /// </summary>
    public static Unit lx { get; } = null!;

    /// <summary>
    /// equivalent dose of ionizing radiation
    /// </summary>
    public static Unit Sv { get; } = null!;

    /// <summary>
    /// catalytic activity
    /// </summary>
    public static Unit kat { get; } = null!;

    /// <summary>
    /// thermodynamic temperature
    /// </summary>
    /// <remarks>
    /// This is a relative unit and needs to be converted to <see cref="K">Kelvin</see> to be used in calculations.
    /// </remarks>
    public static Unit degC { get; } = null!;

    #endregion

    #region Incoherent units

    /// <summary>
    /// Time expressed in hours
    /// </summary>
    public static Unit h { get; } = null!;

    /// <summary>
    /// Time expressed in minutes
    /// </summary>
    public static Unit min { get; } = null!;

    #endregion
}
