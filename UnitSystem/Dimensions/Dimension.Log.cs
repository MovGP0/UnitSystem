namespace UnitSystem.Dimensions;

public sealed partial class Dimension
{
    /// <summary>
    /// Computes the natural logarithm (ln) of a dimension, applying the logarithm to each exponent.
    /// </summary>
    /// <param name="dimension">The dimension to apply the natural logarithm to.</param>
    /// <returns>A new <see cref="Dimension"/> resulting from the natural logarithm of the original dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is null.</exception>
    public static Dimension Ln(Dimension dimension)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();
        return new Dimension(dimension.Select(e => e == 0 ? 0 : (float)Math.Log(e)).ToArray());
    }

    /// <summary>
    /// Computes the base-10 logarithm (lg) of a dimension, applying the logarithm to each exponent.
    /// </summary>
    /// <param name="dimension">The dimension to apply the base-10 logarithm to.</param>
    /// <returns>A new <see cref="Dimension"/> resulting from the base-10 logarithm of the original dimension.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is null.</exception>
    public static Dimension Lg(Dimension dimension)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();
        return new Dimension(dimension.Select(e => e == 0 ? 0 : (float)Math.Log10(e)).ToArray());
    }

    /// <summary>
    /// Computes the logarithm of a dimension to a specified base, applying the logarithm to each exponent.
    /// </summary>
    /// <param name="dimension">The dimension to apply the logarithm to.</param>
    /// <param name="base">The base of the logarithm.</param>
    /// <returns>A new <see cref="Dimension"/> resulting from the logarithm of the original dimension to the specified base.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is null.</exception>
    public static Dimension Log(Dimension dimension, double @base)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();
        return new Dimension(dimension.Select(e => e == 0 ? 0 : (float)(Math.Log(e) / Math.Log(@base))).ToArray());
    }
}
