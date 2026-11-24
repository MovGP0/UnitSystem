using ShInUeXx.Numerics;

namespace UnitSystem.Dimensions;

public sealed partial class Dimension
{
    /// <summary>
    /// Multiplies two dimensions by adding their corresponding exponents.
    /// </summary>
    public static Dimension operator *(Dimension left, Dimension right)
        => Combine(left, right, (first, second) => first + second);

    /// <summary>
    /// Divides two dimensions by subtracting their corresponding exponents.
    /// </summary>
    public static Dimension operator /(Dimension left, Dimension right)
        => Combine(left, right, (first, second) => first - second);

    /// <summary>
    /// Raises this dimension to an integer power.
    /// </summary>
    public Dimension Pow(int exponent) => Pow((Rational)exponent);

    /// <summary>
    /// Raises this dimension to the round-trip decimal representation of a single-precision exponent.
    /// </summary>
    public Dimension Pow(float exponent) => Pow(DimensionExponent.FromSingle(exponent));

    /// <summary>
    /// Raises this dimension to an exact rational power.
    /// </summary>
    public Dimension Pow(Rational exponent)
    {
        if (!Rational.IsFinite(exponent))
        {
            throw new ArgumentOutOfRangeException(nameof(exponent), "A dimension exponent must be finite.");
        }

        return new Dimension(this.Select(value => value * exponent).ToArray());
    }

    private static Dimension Combine(Dimension left, Dimension right, Func<Rational, Rational, Rational> combine)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var exponents = new Rational[Math.Max(left.Count, right.Count)];
        for (var index = 0; index < exponents.Length; index++)
        {
            var first = index < left.Count ? left[index] : Rational.Zero;
            var second = index < right.Count ? right[index] : Rational.Zero;
            exponents[index] = combine(first, second);
        }

        return new Dimension(exponents);
    }
}
