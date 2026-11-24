using ShInUeXx.Numerics;

namespace UnitSystem.Dimensions;

public sealed partial class Dimension
{
    private const string NonBreakingSpace = "\u00A0";
    private const string CrossProductSymbol = "\u00D7";

    /// <summary>
    /// Returns the exact exponent vector as text.
    /// </summary>
    [Pure]
    public override string ToString() => ToString(null, null);

    /// <summary>
    /// Returns the exponent vector. A numeric format renders a floating-point approximation.
    /// </summary>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => string.Join(CrossProductSymbol, this.Select(exponent => FormatExponent(exponent, format, formatProvider)));

    /// <summary>
    /// Returns the exponents with the supplied system's base-dimension symbols.
    /// </summary>
    [Pure]
    public string ToString(IUnitSystem system, string? format, IFormatProvider? formatProvider)
    {
        var symbols = system.DimensionSymbols;
        return string.Join(NonBreakingSpace, this.Select((value, index) =>
        {
            if (value == Rational.Zero)
            {
                return "";
            }

            var symbol = symbols[index + 1];
            return value == Rational.One ? symbol : $"{symbol}^{FormatExponent(value, format, formatProvider)}";
        }).Where(value => value.Length != 0));
    }

    private static string FormatExponent(Rational exponent, string? format, IFormatProvider? formatProvider)
        => string.IsNullOrEmpty(format)
            ? DimensionExponent.Format(exponent)
            : exponent.ToDouble().ToString(format, formatProvider);
}
