using UnitSystem.Prefixes;

namespace UnitSystem.Systems;

/// <summary>
/// Extends the <see cref="SISystem"/> by Centimetre-Gram-Second (CGS) units.
/// </summary>
/// <remarks>
/// See <a href="https://en.wikipedia.org/wiki/Centimetre%E2%80%93gram%E2%80%93second_system_of_units">
/// Wikipedia: Centimetre–gram–second system of units
/// </a> for details.
/// </remarks>
public static class CGSSystem
{
    internal static void Register(UnitSystem system)
    {
        var cm = system.AddDerivedUnit("cm", "centimetre", MetricPrefix.Centi * SISystem.m);
        var g = system.AddDerivedUnit("g", "gram", SISystem.kg / 1000);
        var s = SISystem.s;
        var cmSquared = cm ^ 2;
        var dyne = system.AddDerivedUnit("dyn", "dyne", g * cm / (s ^ 2));
        var erg = system.AddDerivedUnit("erg", "erg", dyne * cm);
        system.AddDerivedUnit("Ba", "barye", dyne / cmSquared);

        // The CGS electrostatic units are expressed on the catalog's SI basis.
        const double statcoulombInCoulombs = 1 / (10.0 * 299_792_458);
        var statC = (KnownUnit)system.AddDerivedUnit("statC", "statcoulomb", statcoulombInCoulombs * SISystem.C);
        system.AddAlias("stC", statC);
        var statA = statC / s;
        var statV = system.AddDerivedUnit("stV", "statvolt", erg / statC);
        system.AddDerivedUnit("stF", "statfarad", statC / statV);
        system.AddDerivedUnit("stΩ", "statohm", statV / statA);
        system.AddDerivedUnit("stS", "statmho", statA / statV);
        var statWb = system.AddDerivedUnit("stWb", "statweber", statV * s);
        system.AddDerivedUnit("stT", "stattesla", statWb / cmSquared);
        system.AddDerivedUnit("stH", "stathenry", statWb / statA);
        system.AddDerivedUnit("Gs", "gauss", SISystem.T / 10_000);
        system.AddDerivedUnit("ph", "phot", 10_000 * SISystem.lx);
        system.AddDerivedUnit("r", "roentgen", 2.58e-4 * SISystem.C / SISystem.kg);
        system.AddDerivedUnit("k", "kayser", cm ^ -1);
        system.AddDerivedUnit("St", "stokes", cmSquared / s);
        system.AddDerivedUnit("P", "poise", g / (cm * s));
        system.AddDerivedUnit("Gal", "gal", cm / (s ^ 2));
        system.AddDerivedUnit("sb", "stilb", SISystem.cd / cmSquared);
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
    /// Centimetre-Gram-Second System of Units
    /// </summary>
    public static IUnitSystem System => SISystem.System;

    #region Base units

    /// <summary>
    /// luminous intensity
    /// </summary>
    public static Unit cd => System["cd"]!;

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
    /// length
    /// </summary>
    public static Unit cm => System["cm"]!;

    /// <summary>
    /// mass
    /// </summary>
    public static Unit g => System["g"]!;

    /// <summary>
    /// time
    /// </summary>
    public static Unit s => System["s"]!;

    /// <summary>
    /// force
    /// </summary>
    public static Unit dyn => System["dyn"]!;

    /// <summary>
    /// energy
    /// </summary>
    public static Unit erg => System["erg"]!;

    /// <summary>
    /// electric charge
    /// </summary>
    public static Unit statC => System["statC"]!;

    /// <summary>
    /// magnetic flux density
    /// </summary>
    public static Unit Gs => System["Gs"]!;

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

    #endregion

    #region Derived units

    /// <summary>
    /// mass
    /// </summary>
    public static Unit kg => System["kg"]!;

    /// <summary>
    /// force
    /// </summary>
    public static Unit dyne => System["dyn"]!;

    /// <summary>
    /// pressure
    /// </summary>
    public static Unit Ba => System["Ba"]!;

    /// <summary>
    /// electric charge
    /// </summary>
    public static Unit stC => System["stC"]!;

    /// <summary>
    /// voltage
    /// </summary>
    public static Unit stV => System["stV"]!;

    /// <summary>
    /// electric capacitance
    /// </summary>
    public static Unit stF => System["stF"]!;

    /// <summary>
    /// electric resistance
    /// </summary>
    public static Unit stΩ => System["stΩ"]!;

    /// <summary>
    /// electrical conductance
    /// </summary>
    public static Unit stS => System["stS"]!;

    /// <summary>
    /// magnetic flux
    /// </summary>
    public static Unit stWb => System["stWb"]!;

    /// <summary>
    /// magnetic field strength
    /// </summary>
    public static Unit stT => System["stT"]!;

    /// <summary>
    /// inductance
    /// </summary>
    public static Unit stH => System["stH"]!;

    /// <summary>
    /// illuminance
    /// </summary>
    public static Unit ph => System["ph"]!;

    /// <summary>
    /// exposure
    /// </summary>
    public static Unit r => System["r"]!;

    /// <summary>
    /// catalytic activity
    /// </summary>
    public static Unit kat => System["kat"]!;

    /// <summary>
    /// wave number
    /// </summary>
    public static Unit k => System["k"]!;

    /// <summary>
    /// kinematic viscosity
    /// </summary>
    public static Unit St => System["St"]!;

    /// <summary>
    /// dynamic viscosity
    /// </summary>
    public static Unit P => System["P"]!;

    /// <summary>
    /// dose equivalent
    /// </summary>
    public static Unit Sv => System["Sv"]!;

    /// <summary>
    /// acceleration
    /// </summary>
    public static Unit Gal => System["Gal"]!;

    /// <summary>
    /// luminance
    /// </summary>
    public static Unit sb => System["sb"]!;

    #endregion

    #region Incoherent units

    /// <summary>
    /// Time expressed in minutes
    /// </summary>
    public static Unit min => System["min"]!;

    /// <summary>
    /// Time expressed in hours
    /// </summary>
    public static Unit h => System["h"]!;

    #endregion
}
