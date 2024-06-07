namespace UnitSystem.Prefixes;

public static class EngineeringNotation
{
    /// <summary>
    /// Formats a value with the proper engineering notation and metric prefix.
    /// </summary>
    /// <example>
    /// The following result will be "123,456.789 k":
    /// <c>var result = EngineeringNotation.FromValue(123456.789);</c>
    /// </example>
    [Pure]
    public static string FromValue(double value, IFormatProvider? formatProvider = null)
    {
        var decimalExponent = GetDecimalExponent(value);
        int metricExponent = GetClosestMetricExponent(decimalExponent);

        // scale the value by the exponent
        double scaledValue = value * Math.Pow(10, -metricExponent);
        var prefix = prefixes[metricExponent];
        const char NonBreakingSpace = (char)0xA0;
        return (scaledValue.ToString("0.###,###", formatProvider ?? CultureInfo.InvariantCulture) + NonBreakingSpace + prefix.Symbol).TrimEnd();
    }

    /// <summary>
    /// Calculates the Base10 exponent of the given value.
    /// </summary>
    private static double GetDecimalExponent(double value)
        => Math.Floor(Math.Log10(Math.Abs(value)));

    /// <summary>
    /// Returns the closest metric exponent for the given decimal exponent.
    /// </summary>
    private static int GetClosestMetricExponent(double decimalExponent)
    {
        return decimalExponent switch
        {
            // bigger than Quetta
            >= 30.0 => 30,
            // smaller than quecto
            <= -30.0 => -30,
            // special handling for centi, deci, deca, hecto
            >= -3.0 and <= 3.0 => (int)decimalExponent,
            // special handling for negative exponents
            < -3.0 => ((int)decimalExponent % 3) switch
            {
                -2 => (int)((decimalExponent - 2) / 3 ) * 3,
                -1 => (int)(decimalExponent / 3 - 1) * 3,
                _ => (int)(decimalExponent / 3) * 3
            },
            // handling for positive exponents
            _ => (int)(decimalExponent / 3) * 3
        };
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static string FromValue(decimal value, IFormatProvider? formatProvider = null) => FromValue((double)value, formatProvider);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static string FromValue(float value, IFormatProvider? formatProvider = null) => FromValue((double)value, formatProvider);

    /// <summary>
    /// Cache of all metric prefixes
    /// </summary>
    private static Dictionary<int, MetricPrefix> prefixes => MetricPrefix.GetAll().ToDictionary(i => i.Exponent);
}
