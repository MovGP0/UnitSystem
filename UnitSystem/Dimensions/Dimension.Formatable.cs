namespace UnitSystem.Dimensions;

public sealed partial class Dimension
{
    private const string NonBreakingSpace = "\u00A0";
    private const string CrossProductSymbol = "\u00D7";

    /// <summary>
    /// Returns a string representation of the dimension.
    /// </summary>
    /// <returns>A string representation of the dimension.</returns>
    [Pure]
    public override string ToString() => string.Join(CrossProductSymbol, this.Select(i => i.ToString("F2", CultureInfo.InvariantCulture.NumberFormat)));

    /// <summary>
    /// Returns a formatted string representation of the dimension.
    /// </summary>
    /// <param name="format">The format string for the <see cref="float"/> values of the dimension exponents.</param>
    /// <param name="formatProvider">The format provider to use for formatting the exponents.</param>
    /// <returns>A formatted string representation of the dimension.</returns>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => string.Join(CrossProductSymbol, this.Select(i => i.ToString(format, formatProvider)));

    [Pure]
    public string ToString(IUnitSystem system, string? format, IFormatProvider? formatProvider)
    {
        return string.Join(NonBreakingSpace, this.Select((value, index) =>
        {
            if (value == 0)
            {
                return "";
            }

            string symbol = system.DimensionSymbols[index];
            return value == 1f
                ? symbol
                : $"{symbol}^{value.ToString(format, formatProvider)}";
        })
        .Where(v => !string.IsNullOrEmpty(v)));
    }
}
