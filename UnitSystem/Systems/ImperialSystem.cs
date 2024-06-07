namespace UnitSystem.Systems;

/// <summary>
/// Extends the <see cref="SISystem"/> by the Imperial System of Units.
/// </summary>
/// <remarks>
/// See <a href="https://en.wikipedia.org/wiki/Imperial_units">Wikipedia: Imperial units</a> for details.
/// </remarks>
public static class ImperialSystem
{
    static ImperialSystem()
    {
        System = SISystem.System;

        // Base units
        ft = System.AddDerivedUnit("ft", "foot", 0.3048 * SISystem.m);
        lb = System.AddDerivedUnit("lb", "pound", 0.45359237 * SISystem.kg);

        s =  SISystem.s;
        A =  SISystem.A;
        K =  SISystem.K;
        mol = SISystem.mol;
        cd = SISystem.cd;

        // Angles
        rad = SISystem.rad;
        sr = SISystem.sr;
        deg = SISystem.deg;

        // Incoherent units
        min = SISystem.min;
        h = SISystem.h;

        // Derived units
        @in = System.AddDerivedUnit("in", "inch", ft / 12);
        yd = System.AddDerivedUnit("yd", "yard", 3 * ft);
        mi = System.AddDerivedUnit("mi", "mile", 5280 * ft);
        oz = System.AddDerivedUnit("oz", "ounce", lb / 16);
        ton_long = System.AddDerivedUnit("long ton", "long ton", 2240 * lb);
        ton_short = System.AddDerivedUnit("short ton", "short ton", 2000 * lb);
        gal = System.AddDerivedUnit("gal", "gallon", 277.419432 * @in * @in * @in);
        gal_us = System.AddDerivedUnit("us gal", "US gallon", 231 * @in * @in * @in);
        qt = System.AddDerivedUnit("qt", "quart", gal / 4);
        pt = System.AddDerivedUnit("pt", "pint", gal / 8);
        fl_oz = System.AddDerivedUnit("fl_oz", "fluid ounce", gal / 160);
        hp = System.AddDerivedUnit("hp", "horsepower", 550 * ft * lb / (s * s));
        psi = System.AddDerivedUnit("psi", "pound per square inch", lb / (@in * @in));
        mph = System.AddDerivedUnit("mph", "miles per hour", mi / h);
        lbf = System.AddDerivedUnit("lbf", "pound-force", lb * ft / (s * s));
        btu = System.AddDerivedUnit("btu", "british thermal unit", 778.169 * ft * lb);
        degF = System.AddDerivedUnit("°F", "degrees fahrenheit", 5.0/9.0 * K, offset: 459.67);
    }

    #region IUnitSystem

    /// <inheritdoc cref="IUnitSystem.Name"/>
    public static string Name => System.Name;

    /// <inheritdoc cref="IUnitSystem.NumberOfDimensions"/>
    public static int NumberOfDimensions => System.NumberOfDimensions;

    /// <inheritdoc cref="IUnitSystem.BaseUnits"/>
    public static IEnumerable<KnownUnit> BaseUnits => System.BaseUnits;

    /// <inheritdoc cref="IUnitSystem.NoUnit"/>
    public static Unit NoUnit => System.NoUnit;

    /// <inheritdoc cref="IUnitSystem.AddBaseUnit(string, string, string, string)"/>
    public static Unit AddBaseUnit(string symbol, string name, string inherentPrefix = "", string dimensionSymbol = "")
        => System.AddBaseUnit(symbol, name, inherentPrefix, dimensionSymbol);

    /// <inheritdoc cref="IUnitSystem.AddDerivedUnit(string, string, Unit)"/>
    public static Unit AddDerivedUnit(string symbol, string name, Unit unit)
        => System.AddDerivedUnit(symbol, name, unit);

    /// <inheritdoc cref="IUnitSystem.Parse(string)"/>
    public static Unit Parse(string unitExpression) => System.Parse(unitExpression);

    /// <inheritdoc cref="IUnitSystem.Display(Unit)"/>
    public static string Display(Unit unit) => System.Display(unit);

    /// <inheritdoc cref="IUnitSystem.MakeCoherent(Unit)"/>
    public static Unit MakeCoherent(Unit unit) => System.MakeCoherent(unit);

    /// <inheritdoc cref="IUnitSystem.DimensionSymbols"/>
    public static IReadOnlyDictionary<int, string> DimensionSymbols => System.DimensionSymbols;

    #endregion

    /// <summary>
    /// Imperial System of Units
    /// </summary>
    public static IUnitSystem System { get; }

    #region Incoherent units

    /// <summary>
    /// angle
    /// </summary>
    public static Unit rad { get; }

    /// <summary>
    /// spherical angle
    /// </summary>
    public static Unit sr { get; }

    /// <summary>
    /// angle
    /// </summary>
    public static Unit deg { get; }

    /// <summary>
    /// Time expressed in minutes
    /// </summary>
    public static Unit min { get; }

    /// <summary>
    /// Time expressed in hours
    /// </summary>
    public static Unit h { get; }

    #endregion

    #region Base units

    /// <summary>
    /// length
    /// </summary>
    public static Unit ft { get; }

    /// <summary>
    /// mass
    /// </summary>
    public static Unit lb { get; }

    /// <summary>
    /// time
    /// </summary>
    public static Unit s { get; }

    /// <summary>
    /// electric current
    /// </summary>
    public static Unit A { get; }

    /// <summary>
    /// thermodynamic temperature
    /// </summary>
    public static Unit K { get; }

    /// <summary>
    /// amount of substance
    /// </summary>
    public static Unit mol { get; }

    /// <summary>
    /// luminous intensity
    /// </summary>
    public static Unit cd { get; }

    #endregion

    #region Derived units

    /// <summary>
    /// length
    /// </summary>
    public static Unit @in { get; }

    /// <summary>
    /// length
    /// </summary>
    public static Unit yd { get; }

    /// <summary>
    /// length
    /// </summary>
    public static Unit mi { get; }

    /// <summary>
    /// mass
    /// </summary>
    public static Unit oz { get; }

    /// <summary>
    /// mass
    /// </summary>
    public static Unit ton_long { get; }

    /// <summary>
    /// mass
    /// </summary>
    public static Unit ton_short { get; }

    /// <summary>
    /// volume
    /// </summary>
    /// <remarks>
    /// This is the UK gallon.
    /// Equals to 4.54609 litres or 277.419432 cubic inches
    /// </remarks>
    public static Unit gal { get; }

    /// <summary>
    /// volume
    /// </summary>
    /// <remarks>
    /// This is the US gallon.
    /// Equals to 3.785411784 litres or 231 cubic inches
    /// </remarks>
    public static Unit gal_us { get; }

    /// <summary>
    /// volume
    /// </summary>
    public static Unit qt { get; }

    /// <summary>
    /// volume
    /// </summary>
    public static Unit pt { get; }

    /// <summary>
    /// volume
    /// </summary>
    public static Unit fl_oz { get; }

    /// <summary>
    /// power
    /// </summary>
    public static Unit hp { get; }

    /// <summary>
    /// pressure
    /// </summary>
    public static Unit psi { get; }

    /// <summary>
    /// speed
    /// </summary>
    public static Unit mph { get; }

    /// <summary>
    /// force
    /// </summary>
    public static Unit lbf { get; }

    /// <summary>
    /// energy
    /// </summary>
    public static Unit btu { get; }

    /// <summary>
    /// thermodynamic temperature
    /// </summary>
    /// <remarks>
    /// This is a relative unit and needs to be converted to <see cref="K">Kelvin</see> to be used in calculations.
    /// </remarks>
    public static Unit degF { get; }

    #endregion
}
