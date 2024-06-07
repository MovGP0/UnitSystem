namespace UnitSystem.Prefixes;

public static class BinaryEngineeringNotation
{
    [Pure]
    public static string FromValue(double value, IFormatProvider? formatProvider = null)
    {
        // Get the closest binary exponent
        const string NonBreakingSpace = "\u00A0";
        int binaryExponent = GetClosestBinaryExponent(value);
        var prefix = BinaryPrefix.GetAll().Single(p => p.Base1024Exponent == binaryExponent);
        double scaledValue = value * Math.Pow(2, -prefix.Base2Exponent);
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

    private static readonly Dictionary<int, double> base1024Exponents = new();

    [Pure]
    private static int GetClosestBinaryExponent(double value)
    {
        if (value < 0) return 0;

        for (var i = 10; i >= 0; i--)
        {
            if (base1024Exponents.TryGetValue(i, out var pow))
            {
                if (value >= pow)
                {
                    return i;
                }
            }
            else
            {
                pow = Math.Pow(1024, i);
                base1024Exponents.Add(i, pow);
                if (value >= pow)
                {
                    return i;
                }
            }
        }

        return 0;
    }
}
