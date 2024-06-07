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
    static CGSSystem()
    {
        System = SISystem.System;

        // Base units
        cm = System.AddDerivedUnit("cm", "centimetre", MetricPrefix.Centi * SISystem.m);
        g = System.AddDerivedUnit("g", "gram", 0.001 * SISystem.kg);
        s = SISystem.s;
        A = SISystem.A;
        K = SISystem.K;
        mol = SISystem.mol;
        cd = SISystem.cd;

        dyn = System.AddDerivedUnit("dyn", "dyne", g * cm * (s^-2));
        erg = System.AddDerivedUnit("erg", "erg", dyn * cm);
        statC = System.AddDerivedUnit("statC", "statcoulomb", dyn * (cm^2) * (s^-2));
        Gs = System.AddDerivedUnit("Gs", "gauss", dyn * (cm^-2));

        // Angles
        rad = SISystem.rad;
        sr = SISystem.sr;
        deg = SISystem.deg;

        // Incoherent units
        min = SISystem.min;
        h = SISystem.h;

        // Derived units
        dyne = System.AddDerivedUnit("dyn", "dyne", g*cm*(s^-2));
        erg = System.AddDerivedUnit("erg", "erg", dyne*cm);
        Ba = System.AddDerivedUnit("Ba", "barye", dyne*(cm^-2));
        stC = System.AddDerivedUnit("stC", "statcoulomb", dyn*(cm^2)*(s^-2));
        stV = System.AddDerivedUnit("stV", "statvolt", erg*(stC^-1));
        stF = System.AddDerivedUnit("stF", "statfarad", stC*(stV^-1));
        stΩ = System.AddDerivedUnit("stΩ", "statohm", stV*(dyn^-1));
        stS = System.AddDerivedUnit("stS", "statmho", stC*(erg^-1));
        stWb = System.AddDerivedUnit("stWb", "statweber", erg*(s^-2));
        stT = System.AddDerivedUnit("stT", "stattesla", stWb*(cm^-2));
        stH = System.AddDerivedUnit("stH", "stathenry", stWb*(stC^-1));
        Gs = System.AddDerivedUnit("Gs", "gauss", dyne*(cm^-2));
        ph = System.AddDerivedUnit("ph", "phot", erg*(cm^-2)*(s^-1));
        r = System.AddDerivedUnit("r", "roentgen", statC*(g^-1));
        kg = System.AddDerivedUnit("kg", "kilogram", 1000 * g);
        kat = System.AddDerivedUnit("kat", "katal", mol*(s^-1));
        Sv = System.AddDerivedUnit("Sv", "sievert", erg*(g^-1));
        kat = System.AddDerivedUnit("kat", "katal", mol*(s^-1));
        k = System.AddDerivedUnit("k", "kayser", cm^-1);
        St = System.AddDerivedUnit("St", "stokes", (cm^2)*(s^-1));
        P = System.AddDerivedUnit("P", "poise", g*(cm^-1)*(s^-1));
        Gal = System.AddDerivedUnit("Gal", "gal", cm*(s^-2));
        sb = System.AddDerivedUnit("sb", "stilb", cd*(cm^-2));
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
    public static IUnitSystem System { get; }

    #region Base units

    /// <summary>
    /// luminous intensity
    /// </summary>
    public static Unit cd { get; }

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
    /// length
    /// </summary>
    public static Unit cm { get; }

    /// <summary>
    /// mass
    /// </summary>
    public static Unit g { get; }

    /// <summary>
    /// time
    /// </summary>
    public static Unit s { get; }

    /// <summary>
    /// force
    /// </summary>
    public static Unit dyn { get; }

    /// <summary>
    /// energy
    /// </summary>
    public static Unit erg { get; }

    /// <summary>
    /// electric charge
    /// </summary>
    public static Unit statC { get; }

    /// <summary>
    /// magnetic flux density
    /// </summary>
    public static Unit Gs { get; }

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

    #endregion

    #region Derived units

    /// <summary>
    /// mass
    /// </summary>
    public static Unit kg { get; }

    /// <summary>
    /// force
    /// </summary>
    public static Unit dyne { get; }

    /// <summary>
    /// pressure
    /// </summary>
    public static Unit Ba { get; }

    /// <summary>
    /// electric charge
    /// </summary>
    public static Unit stC { get; }

    /// <summary>
    /// voltage
    /// </summary>
    public static Unit stV { get; }

    /// <summary>
    /// electric capacitance
    /// </summary>
    public static Unit stF { get; }

    /// <summary>
    /// electric resistance
    /// </summary>
    public static Unit stΩ { get; }

    /// <summary>
    /// electrical conductance
    /// </summary>
    public static Unit stS { get; }

    /// <summary>
    /// magnetic flux
    /// </summary>
    public static Unit stWb { get; }

    /// <summary>
    /// magnetic field strength
    /// </summary>
    public static Unit stT { get; }

    /// <summary>
    /// inductance
    /// </summary>
    public static Unit stH { get; }

    /// <summary>
    /// illuminance
    /// </summary>
    public static Unit ph { get; }

    /// <summary>
    /// exposure
    /// </summary>
    public static Unit r { get; }

    /// <summary>
    /// catalytic activity
    /// </summary>
    public static Unit kat { get; }

    /// <summary>
    /// wave number
    /// </summary>
    public static Unit k { get; }

    /// <summary>
    /// kinematic viscosity
    /// </summary>
    public static Unit St { get; }

    /// <summary>
    /// dynamic viscosity
    /// </summary>
    public static Unit P { get; }

    /// <summary>
    /// dose equivalent
    /// </summary>
    public static Unit Sv { get; }

    /// <summary>
    /// acceleration
    /// </summary>
    public static Unit Gal { get; }

    /// <summary>
    /// luminance
    /// </summary>
    public static Unit sb { get; }

    #endregion

    #region Incoherent units

    /// <summary>
    /// Time expressed in minutes
    /// </summary>
    public static Unit min { get; }

    /// <summary>
    /// Time expressed in hours
    /// </summary>
    public static Unit h { get; }

    #endregion
}
