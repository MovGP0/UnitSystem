using ShInUeXx.Numerics;

namespace UnitSystem.Dimensions;

internal static class DimensionExponent
{
    private const int MaxScientificExponent = 10_000;

    internal static Rational FromSingle(float value)
    {
        if (!float.IsFinite(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "A dimension exponent must be finite.");
        }

        return Parse(value.ToString("R", CultureInfo.InvariantCulture));
    }

    internal static bool TryParse(ReadOnlySpan<char> text, out Rational value)
    {
        text = text.Trim();
        var exponentMarker = text.IndexOfAny('e', 'E');
        // A short scientific literal can otherwise expand into a very large integer.
        if (exponentMarker >= 0
            && (!int.TryParse(
                text[(exponentMarker + 1)..],
                NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out var exponent)
                || exponent is < -MaxScientificExponent or > MaxScientificExponent))
        {
            value = default;
            return false;
        }

        // Keep accepting leading decimal points as the previous floating-point parser did.
        var signLength = !text.IsEmpty && text[0] is '+' or '-' ? 1 : 0;
        if (text[signLength..].StartsWith(".", StringComparison.Ordinal))
        {
            text = string.Concat(text[..signLength], "0", text[signLength..]);
        }

        return Rational.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
            && Rational.IsFinite(value);
    }

    internal static Rational Parse(ReadOnlySpan<char> text)
        => TryParse(text, out var value)
            ? value
            : throw new FormatException($"'{text}' is not a finite dimension exponent.");

    internal static string Format(Rational value)
        => value.Denominator.IsOne
            ? value.Numerator.ToString(CultureInfo.InvariantCulture)
            : value.ToString("F", CultureInfo.InvariantCulture);
}
