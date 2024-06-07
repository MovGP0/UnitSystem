namespace UnitSystem.Dimensions;

/// <summary>
/// Represents a physical dimension, defined by a collection of exponents corresponding to the fundamental dimensions.
/// </summary>
public sealed partial class Dimension : ImmutableCollection<float>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Dimension"/> class with the specified exponents.
    /// </summary>
    /// <param name="exponents">The exponents representing the dimension.</param>
    public Dimension(params float[] exponents)
        : base(Trim(exponents))
    {
        Check.Argument(exponents, nameof(exponents)).IsNotNull();
    }

    /// <summary>
    /// Gets a dimensionless instance of <see cref="Dimension"/>.
    /// </summary>
    public static Dimension DimensionLess { get; } = new();

    /// <summary>
    /// Trims trailing zeros from the array of exponents.
    /// </summary>
    /// <param name="exponents">The array of exponents to trim.</param>
    /// <returns>The trimmed array of exponents.</returns>
    private static float[] Trim(float[] exponents)
    {
        if (exponents.Length == 0) return exponents;

        var index = exponents.Length - 1;

        while (index >= 0 && exponents[index] == 0)
        {
            index--;
        }

        Array.Resize(ref exponents, index + 1);

        return exponents;
    }
}
