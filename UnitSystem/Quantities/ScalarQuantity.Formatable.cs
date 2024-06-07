using UnitSystem.Extensions;

namespace UnitSystem.Quantities;

public sealed partial class ScalarQuantity : IFormattable
{
    /// <inheritdoc />
    [Pure]
    public override string ToString() => ToString(null, NumberFormatInfo.CurrentInfo);

    /// <summary>
    /// Converts the scalar quantity to its string representation using the specified format string.
    /// </summary>
    /// <param name="format">The format string to use, or null to use the default format.</param>
    /// <returns>A string representation of the scalar quantity.</returns>
    [Pure]
    public string ToString(string format)
        => ToString(format, NumberFormatInfo.CurrentInfo);

    /// <summary>
    /// Converts the scalar quantity to its string representation using the specified format provider.
    /// </summary>
    /// <param name="formatProvider">The format provider to use, or null to use the current culture format provider.</param>
    /// <returns>A string representation of the scalar quantity.</returns>
    [Pure]
    public string ToString(IFormatProvider formatProvider)
        => ToString(null, formatProvider);

    /// <summary>
    /// Converts the scalar quantity to its string representation in the specified unit.
    /// </summary>
    /// <param name="unit">The unit to convert the scalar quantity to.</param>
    /// <returns>A string representation of the scalar quantity in the specified unit.</returns>
    [Pure]
    public string ToString(Unit unit)
        => Convert(unit).ToString(null, NumberFormatInfo.CurrentInfo);

    /// <summary>
    /// Converts the scalar quantity to its string representation using the specified format string and unit.
    /// </summary>
    /// <param name="format">The format string to use, or null to use the default format.</param>
    /// <param name="unit">The unit to convert the scalar quantity to.</param>
    /// <returns>A string representation of the scalar quantity in the specified unit.</returns>
    [Pure]
    public string ToString(string format, Unit unit)
        => Convert(unit).ToString(format, NumberFormatInfo.CurrentInfo);

    /// <summary>
    /// Converts the scalar quantity to its string representation using the specified format provider and unit.
    /// </summary>
    /// <param name="formatProvider">The format provider to use, or null to use the current culture format provider.</param>
    /// <param name="unit">The unit to convert the scalar quantity to.</param>
    /// <returns>A string representation of the scalar quantity in the specified unit.</returns>
    [Pure]
    public string ToString(IFormatProvider formatProvider, Unit unit)
        => Convert(unit).ToString(null, formatProvider);

    /// <summary>
    /// Converts the scalar quantity to its string representation using the specified format string, format provider, and unit.
    /// </summary>
    /// <param name="format">The format string to use, or null to use the default format.</param>
    /// <param name="formatProvider">The format provider to use, or null to use the current culture format provider.</param>
    /// <param name="unit">The unit to convert the scalar quantity to.</param>
    /// <returns>A string representation of the scalar quantity in the specified unit.</returns>
    [Pure]
    public string ToString(string format, IFormatProvider formatProvider, Unit unit)
        => Convert(unit).ToString(format, formatProvider);

    /// <inheritdoc />
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => "{0} {1}".FormatWith(Amount.ToString(format, formatProvider), Unit);
}
