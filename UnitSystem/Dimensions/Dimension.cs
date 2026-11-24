using ShInUeXx.Numerics;

namespace UnitSystem.Dimensions;

/// <summary>
/// Represents a physical dimension as exact rational exponents of its base dimensions.
/// </summary>
public sealed partial class Dimension : ImmutableCollection<Rational>
{
    /// <summary>
    /// Initializes a dimensionless value.
    /// </summary>
    public Dimension()
        : this(Array.Empty<Rational>())
    {
    }

    /// <summary>
    /// Initializes a dimension with integer exponents.
    /// </summary>
    public Dimension(params int[] exponents)
        : this(exponents.Select(exponent => (Rational)exponent).ToArray())
    {
    }

    /// <summary>
    /// Initializes a dimension from single-precision exponents using their round-trip decimal representations.
    /// </summary>
    public Dimension(params float[] exponents)
        : this(exponents.Select(DimensionExponent.FromSingle).ToArray())
    {
    }

    /// <summary>
    /// Initializes a dimension with exact rational exponents.
    /// </summary>
    public Dimension(params Rational[] exponents)
        : base(Trim(exponents))
    {
    }

    /// <summary>
    /// Gets a dimensionless value.
    /// </summary>
    public static Dimension DimensionLess { get; } = new();

    private static Rational[] Trim(Rational[] exponents)
    {
        ArgumentNullException.ThrowIfNull(exponents);
        if (exponents.Any(exponent => !Rational.IsFinite(exponent)))
        {
            throw new ArgumentOutOfRangeException(nameof(exponents), "Dimension exponents must be finite.");
        }

        var count = exponents.Length;
        while (count > 0 && exponents[count - 1] == Rational.Zero)
        {
            count--;
        }

        Array.Resize(ref exponents, count);
        return exponents;
    }
}
