namespace UnitSystem.Prefixes;

/// <summary>
/// Formats binary prefix.
/// <a href="https://en.wikipedia.org/wiki/Binary_prefix">Wikipedia: Binary prefix</a> for details.
/// </summary>
/// <remarks>
/// These prefixes are normed by <b>IEC 60027-2 A.2</b> and <b>ISO/IEC 80000:13-2008</b>.
/// </remarks>
public sealed class BinaryPrefix : ExtendableEnums.ExtendableEnum<BinaryPrefix>, IUnitPrefix
{
    /// <summary>
    /// Represents the unit prefix for 1 (no prefix).
    /// </summary>
    public static readonly BinaryPrefix One = new(0, "", "");

    /// <summary>
    /// Represents the unit prefix for kibi (2^10).
    /// </summary>
    public static readonly BinaryPrefix Kibi = new(1, "Ki", "kibi");

    /// <summary>
    /// Represents the unit prefix for mebi (2^20).
    /// </summary>
    public static readonly BinaryPrefix Mebi = new(2, "Mi", "mebi");

    /// <summary>
    /// Represents the unit prefix for gibi (2^30).
    /// </summary>
    public static readonly BinaryPrefix Gibi = new(3, "Gi", "gibi");

    /// <summary>
    /// Represents the unit prefix for tebi (2^40).
    /// </summary>
    public static readonly BinaryPrefix Tebi = new(4, "Ti", "tebi");

    /// <summary>
    /// Represents the unit prefix for pebi (2^50).
    /// </summary>
    public static readonly BinaryPrefix Pebi = new(5, "Pi", "pebi");

    /// <summary>
    /// Represents the unit prefix for exbi (2^60).
    /// </summary>
    public static readonly BinaryPrefix Exbi = new(6, "Ei", "exbi");

    /// <summary>
    /// Represents the unit prefix for zebi (2^70).
    /// </summary>
    public static readonly BinaryPrefix Zebi = new(7, "Zi", "zebi");

    /// <summary>
    /// Represents the unit prefix for yobi (2^80).
    /// </summary>
    public static readonly BinaryPrefix Yobi = new(8, "Yi", "yobi");

    /// <summary>
    /// Represents the unit prefix for robi (2^90).
    /// </summary>
    public static readonly BinaryPrefix Robi = new(9, "Ri", "robi");

    /// <summary>
    /// Represents the unit prefix for qubi (2^100).
    /// </summary>
    public static readonly BinaryPrefix Qubi = new(10, "Qi", "qubi");

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryPrefix"/> class.
    /// </summary>
    /// <param name="base1024Exponent">The exponent for base 1024.</param>
    /// <param name="symbol">The symbol for the binary prefix.</param>
    /// <param name="name">The name of the binary prefix.</param>
    private BinaryPrefix(
        int base1024Exponent,
        string symbol,
        string name)
        : base(base1024Exponent, name)
    {
        Base1024Exponent = base1024Exponent;
        Base2Exponent = base1024Exponent * 10;
        Factor = Math.Pow(2, Base2Exponent);
        Symbol = symbol;
        Name = name;

        symbolToPrefix ??= new();
        symbolToPrefix.Add(symbol, this);
    }

    /// <summary>
    /// Gets the exponent for base 1024.
    /// </summary>
    public int Base1024Exponent { get; }

    /// <summary>
    /// Gets the exponent for base 2.
    /// </summary>
    public int Base2Exponent { get; }

    /// <summary>
    /// Gets the factor of the binary prefix.
    /// </summary>
    public double Factor { get; }

    /// <summary>
    /// Gets the symbol of the binary prefix.
    /// </summary>
    public string Symbol { get; }

    /// <summary>
    /// Gets the name of the binary prefix.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Performs an explicit conversion from <see cref="BinaryPrefix"/> to <see cref="double"/>.
    /// </summary>
    /// <param name="prefix">The binary prefix to convert.</param>
    /// <returns>The factor of the binary prefix.</returns>
    public static explicit operator double(BinaryPrefix prefix) => prefix.Factor;

    /// <summary>
    /// Maps the prefix symbols to a <see cref="MetricPrefix"/>.
    /// </summary>
    private static Dictionary<string, BinaryPrefix>? symbolToPrefix;

    /// <summary>
    /// Tries to get the <see cref="BinaryPrefix"/> associated with the specified symbol.
    /// </summary>
    /// <param name="symbol">The symbol of the binary prefix to get.</param>
    /// <param name="prefix">
    /// When this method returns, contains the <see cref="BinaryPrefix"/> associated with the specified symbol,
    /// if the symbol is found; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="BinaryPrefix"/> associated with the specified symbol is found;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetValue(string symbol, out BinaryPrefix? prefix)
        => symbolToPrefix!.TryGetValue(symbol, out prefix) || TryParse(symbol, out prefix);

    /// <inheritdoc cref="symbolToPrefix"/>
    public static IReadOnlyDictionary<string, BinaryPrefix> Prefixes => symbolToPrefix!;
}
