namespace UnitSystem.Systems;

/// <summary>
/// Extends the <see cref="SISystem"/> by the Imperial System of Units.
/// </summary>
/// <remarks>
/// See <a href="https://en.wikipedia.org/wiki/Imperial_units">Wikipedia: Imperial units</a> for details.
/// </remarks>
public static class ImperialSystem
{
    internal static void Register(UnitSystem system)
    {
        var ft = system.AddDerivedUnit("ft", "foot", 0.3048 * SISystem.m);
        var lb = system.AddDerivedUnit("lb", "pound", 0.45359237 * SISystem.kg);
        var s = SISystem.s;
        var inch = system.AddDerivedUnit("in", "inch", ft / 12);
        var mile = system.AddDerivedUnit("mi", "mile", 5280 * ft);
        system.AddDerivedUnit("yd", "yard", 3 * ft);
        system.AddDerivedUnit("oz", "ounce", lb / 16);
        system.AddDerivedUnit("long ton", "long ton", 2240 * lb);
        system.AddDerivedUnit("short ton", "short ton", 2000 * lb);

        var cubicInch = inch.Pow(3);
        var gallon = system.AddDerivedUnit("gal", "gallon", 0.00454609 * SISystem.m.Pow(3));
        system.AddDerivedUnit("us gal", "US gallon", 231 * cubicInch);
        system.AddDerivedUnit("qt", "quart", gallon / 4);
        system.AddDerivedUnit("pt", "pint", gallon / 8);
        system.AddDerivedUnit("fl_oz", "fluid ounce", gallon / 160);

        var lbf = system.AddDerivedUnit("lbf", "pound-force", lb * (9.80665 * SISystem.m / s.Pow(2)));
        system.AddDerivedUnit("hp", "horsepower", 550 * ft * lbf / s);
        system.AddDerivedUnit("psi", "pound per square inch", lbf / inch.Pow(2));
        system.AddDerivedUnit("mph", "miles per hour", mile / SISystem.h);
        system.AddDerivedUnit("btu", "British thermal unit", 778.169 * ft * lbf);

        const double fahrenheitScale = 5.0 / 9.0;
        system.AddDerivedUnit("°F", "degrees Fahrenheit", fahrenheitScale * SISystem.K,
            offset: 273.15 - 32 * fahrenheitScale);
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
    public static IUnitSystem System => SISystem.System;

    #region Incoherent units

    /// <summary>
    /// angle
    /// </summary>
    public static Unit rad => System["rad"]!;

    /// <summary>
    /// spherical angle
    /// </summary>
    public static Unit sr => System["sr"]!;

    /// <summary>
    /// angle
    /// </summary>
    public static Unit deg => System["°"]!;

    /// <summary>
    /// Time expressed in minutes
    /// </summary>
    public static Unit min => System["min"]!;

    /// <summary>
    /// Time expressed in hours
    /// </summary>
    public static Unit h => System["h"]!;

    #endregion

    #region Base units

    /// <summary>
    /// length
    /// </summary>
    public static Unit ft => System["ft"]!;

    /// <summary>
    /// mass
    /// </summary>
    public static Unit lb => System["lb"]!;

    /// <summary>
    /// time
    /// </summary>
    public static Unit s => System["s"]!;

    /// <summary>
    /// electric current
    /// </summary>
    public static Unit A => System["A"]!;

    /// <summary>
    /// thermodynamic temperature
    /// </summary>
    public static Unit K => System["K"]!;

    /// <summary>
    /// amount of substance
    /// </summary>
    public static Unit mol => System["mol"]!;

    /// <summary>
    /// luminous intensity
    /// </summary>
    public static Unit cd => System["cd"]!;

    #endregion

    #region Derived units

    /// <summary>
    /// length
    /// </summary>
    public static Unit @in => System["in"]!;

    /// <summary>
    /// length
    /// </summary>
    public static Unit yd => System["yd"]!;

    /// <summary>
    /// length
    /// </summary>
    public static Unit mi => System["mi"]!;

    /// <summary>
    /// mass
    /// </summary>
    public static Unit oz => System["oz"]!;

    /// <summary>
    /// mass
    /// </summary>
    public static Unit ton_long => System["long ton"]!;

    /// <summary>
    /// mass
    /// </summary>
    public static Unit ton_short => System["short ton"]!;

    /// <summary>
    /// volume
    /// </summary>
    /// <remarks>
    /// This is the UK gallon.
    /// Equals to 4.54609 litres or 277.419432 cubic inches
    /// </remarks>
    public static Unit gal => System["gal"]!;

    /// <summary>
    /// volume
    /// </summary>
    /// <remarks>
    /// This is the US gallon.
    /// Equals to 3.785411784 litres or 231 cubic inches
    /// </remarks>
    public static Unit gal_us => System["us gal"]!;

    /// <summary>
    /// volume
    /// </summary>
    public static Unit qt => System["qt"]!;

    /// <summary>
    /// volume
    /// </summary>
    public static Unit pt => System["pt"]!;

    /// <summary>
    /// volume
    /// </summary>
    public static Unit fl_oz => System["fl_oz"]!;

    /// <summary>
    /// power
    /// </summary>
    public static Unit hp => System["hp"]!;

    /// <summary>
    /// pressure
    /// </summary>
    public static Unit psi => System["psi"]!;

    /// <summary>
    /// speed
    /// </summary>
    public static Unit mph => System["mph"]!;

    /// <summary>
    /// force
    /// </summary>
    public static Unit lbf => System["lbf"]!;

    /// <summary>
    /// energy
    /// </summary>
    public static Unit btu => System["btu"]!;

    /// <summary>
    /// thermodynamic temperature
    /// </summary>
    /// <remarks>
    /// This is a relative unit and needs to be converted to <see cref="K">Kelvin</see> to be used in calculations.
    /// </remarks>
    public static Unit degF => System["°F"]!;

    #endregion
}
