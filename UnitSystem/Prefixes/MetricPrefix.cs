namespace UnitSystem.Prefixes;

/// <summary>
/// Prefixes as defined in the <b>International System of Units (SI)</b>.
/// </summary>
/// <remarks>
/// See <a href="https://en.wikipedia.org/wiki/Metric_prefix">Wikipedia: Metric Prefix</a> and
/// <a href="https://www.bipm.org/en/publications/si-brochure">SI Brochure</a> for details.
/// </remarks>
public sealed class MetricPrefix : ExtendableEnums.ExtendableEnumBase<MetricPrefix, int>, IUnitPrefix
{
    /// <summary>
    /// Represents the unit prefix for quetta (10^30).
    /// </summary>
    public static readonly MetricPrefix Quetta = new(30, "Q", "quetta");

    /// <summary>
    /// Represents the unit prefix for ronna (10^27).
    /// </summary>
    public static readonly MetricPrefix Ronna = new(27, "R", "ronna");

    /// <summary>
    /// Represents the unit prefix for yotta (10^24).
    /// </summary>
    public static readonly MetricPrefix Yotta = new(24, "Y", "yotta");

    /// <summary>
    /// Represents the unit prefix for zetta (10^21).
    /// </summary>
    public static readonly MetricPrefix Zetta = new(21, "Z", "zetta");

    /// <summary>
    /// Represents the unit prefix for exa (10^18).
    /// </summary>
    public static readonly MetricPrefix Exa = new(18, "E", "exa");

    /// <summary>
    /// Represents the unit prefix for peta (10^15).
    /// </summary>
    public static readonly MetricPrefix Peta = new(15, "P", "peta");

    /// <summary>
    /// Represents the unit prefix for tera (10^12).
    /// </summary>
    public static readonly MetricPrefix Tera = new(12, "T", "tera");

    /// <summary>
    /// Represents the unit prefix for giga (10^9).
    /// </summary>
    public static readonly MetricPrefix Giga = new(9, "G", "giga");

    /// <summary>
    /// Represents the unit prefix for mega (10^6).
    /// </summary>
    public static readonly MetricPrefix Mega = new(6, "M", "mega");

    /// <summary>
    /// Represents the unit prefix for kilo (10^3).
    /// </summary>
    public static readonly MetricPrefix Kilo = new(3, "k", "kilo");

    /// <summary>
    /// Represents the unit prefix for hecto (10^2).
    /// </summary>
    public static readonly MetricPrefix Hecto = new(2, "h", "hecto");

    /// <summary>
    /// Represents the unit prefix for deca (10^1).
    /// </summary>
    public static readonly MetricPrefix Da = new(1, "da", "deca");

    /// <summary>
    /// Represents the unit prefix for one (10^0).
    /// </summary>
    public static readonly MetricPrefix One = new(0, "", "");

    /// <summary>
    /// Represents the unit prefix for deci (10^-1).
    /// </summary>
    public static readonly MetricPrefix Deci = new(-1, "d", "deci");

    /// <summary>
    /// Represents the unit prefix for centi (10^-2).
    /// </summary>
    public static readonly MetricPrefix Centi = new(-2, "c", "centi");

    /// <summary>
    /// Represents the unit prefix for milli (10^-3).
    /// </summary>
    public static readonly MetricPrefix Milli = new(-3, "m", "milli");

    /// <summary>
    /// Represents the unit prefix for micro (10^-6).
    /// </summary>
    public static readonly MetricPrefix Micro = new(-6, "µ", "micro");

    /// <summary>
    /// Represents the unit prefix for nano (10^-9).
    /// </summary>
    public static readonly MetricPrefix Nano = new(-9, "n", "nano");

    /// <summary>
    /// Represents the unit prefix for pico (10^-12).
    /// </summary>
    public static readonly MetricPrefix Pico = new(-12, "p", "pico");

    /// <summary>
    /// Represents the unit prefix for femto (10^-15).
    /// </summary>
    public static readonly MetricPrefix Femto = new(-15, "f", "femto");

    /// <summary>
    /// Represents the unit prefix for atto (10^-18).
    /// </summary>
    public static readonly MetricPrefix Atto = new(-18, "a", "atto");

    /// <summary>
    /// Represents the unit prefix for zepto (10^-21).
    /// </summary>
    public static readonly MetricPrefix Zepto = new(-21, "z", "zepto");

    /// <summary>
    /// Represents the unit prefix for yocto (10^-24).
    /// </summary>
    public static readonly MetricPrefix Yocto = new(-24, "y", "yocto");

    /// <summary>
    /// Represents the unit prefix for ronto (10^-27).
    /// </summary>
    public static readonly MetricPrefix Ronto = new(-27, "r", "ronto");

    /// <summary>
    /// Represents the unit prefix for quecto (10^-30).
    /// </summary>
    public static readonly MetricPrefix Quecto = new(-30, "q", "quecto");

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricPrefix"/> class.
    /// </summary>
    /// <param name="exponent">The exponent for the metric prefix.</param>
    /// <param name="symbol">The symbol for the metric prefix.</param>
    /// <param name="name">The name of the metric prefix.</param>
    private MetricPrefix(int exponent, string symbol, string name)
        : base(exponent, name)
    {
        Exponent = exponent;
        Factor = Math.Pow(10, exponent);
        Symbol = symbol;
        Name = name;

        symbolToPrefix ??= new();
        symbolToPrefix.Add(symbol, this);
    }

    /// <summary>
    /// Gets the exponent of the metric prefix.
    /// </summary>
    public int Exponent { get; }

    /// <summary>
    /// Gets the factor of the metric prefix.
    /// </summary>
    public double Factor { get; }

    /// <summary>
    /// Gets the symbol of the metric prefix.
    /// </summary>
    public string Symbol { get; }

    /// <summary>
    /// Gets the name of the metric prefix.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Performs an explicit conversion from <see cref="MetricPrefix"/> to <see cref="double"/>.
    /// </summary>
    /// <param name="prefix">The metric prefix to convert.</param>
    /// <returns>The factor of the metric prefix.</returns>
    public static explicit operator double(MetricPrefix prefix) => prefix.Factor;

    /// <summary>
    /// Maps the prefix symbols to a <see cref="MetricPrefix"/>.
    /// </summary>
    private static Dictionary<string, MetricPrefix>? symbolToPrefix;

    /// <summary>
    /// Tries to get the <see cref="MetricPrefix"/> associated with the specified symbol.
    /// </summary>
    /// <param name="symbol">The symbol of the metric prefix to get.</param>
    /// <param name="prefix">
    /// When this method returns, contains the <see cref="MetricPrefix"/> associated with the specified symbol,
    /// if the symbol is found; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="MetricPrefix"/> associated with the specified symbol is found;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetValue(string symbol, out MetricPrefix prefix)
        => symbolToPrefix!.TryGetValue(symbol, out prefix!) || TryParse(symbol, out prefix);

    /// <inheritdoc cref="symbolToPrefix"/>
    public static IReadOnlyDictionary<string, MetricPrefix> Prefixes => symbolToPrefix!;
}
