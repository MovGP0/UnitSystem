namespace UnitSystem.Prefixes;

public static class BinaryEngineeringNotation
{
    [Pure]
    public static string FromValue(double value, IFormatProvider? formatProvider = null)
    {
        const string NonBreakingSpace = "\u00A0";
        var prefix = GetPrefix(value);
        var scaledValue = value / prefix.Factor;
        return (scaledValue.ToString("0.###,###", formatProvider ?? CultureInfo.InvariantCulture) + NonBreakingSpace + prefix.Symbol).TrimEnd();
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static string FromValue(decimal value, IFormatProvider? formatProvider = null) => FromValue((double)value, formatProvider);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static string FromValue(float value, IFormatProvider? formatProvider = null) => FromValue((double)value, formatProvider);

    private static readonly BinaryPrefix[] PrefixesByDescendingFactor =
        BinaryPrefix.GetAll().OrderByDescending(prefix => prefix.Factor).ToArray();

    [Pure]
    private static BinaryPrefix GetPrefix(double value)
    {
        foreach (var prefix in PrefixesByDescendingFactor)
        {
            if (value >= prefix.Factor)
                return prefix;
        }

        return BinaryPrefix.One;
    }
}
