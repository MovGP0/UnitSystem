using UnitSystem.Extensions;

namespace UnitSystem.Dimensions;

public sealed partial class Dimension
{
    /// <summary>
    /// Multiplies two dimensions, adding their exponents.
    /// </summary>
    /// <param name="dimension1">The first dimension.</param>
    /// <param name="dimension2">The second dimension.</param>
    /// <returns>A new <see cref="Dimension"/> resulting from the multiplication.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension1"/> or <paramref name="dimension2"/> is null.</exception>
    /// <example>
    /// <code>
    /// var length = new Dimension(1, 0, 0);
    /// var time = new Dimension(0, 1, 0);
    /// var speed = length * time;
    /// // speed is a new Dimension with exponents [1, 1, 0]
    /// </code>
    /// </example>
    public static Dimension operator *(Dimension dimension1, Dimension dimension2)
    {
        Check.Argument(dimension1, nameof(dimension1)).IsNotNull();
        Check.Argument(dimension2, nameof(dimension2)).IsNotNull();

        return new Dimension(dimension1.ZipMerge(dimension2, (x, y) => x + y).ToArray());
    }

    /// <summary>
    /// Divides two dimensions, subtracting their exponents.
    /// </summary>
    /// <param name="dimension1">The first dimension.</param>
    /// <param name="dimension2">The second dimension.</param>
    /// <returns>A new <see cref="Dimension"/> resulting from the division.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension1"/> or <paramref name="dimension2"/> is null.</exception>
    /// <example>
    /// <code>
    /// var length = new Dimension(1, 0, 0);
    /// var time = new Dimension(0, 1, 0);
    /// var speed = length / time;
    /// // speed is a new Dimension with exponents [1, -1, 0]
    /// </code>
    /// </example>
    public static Dimension operator /(Dimension dimension1, Dimension dimension2)
    {
        Check.Argument(dimension1, nameof(dimension1)).IsNotNull();
        Check.Argument(dimension2, nameof(dimension2)).IsNotNull();

        return new Dimension(dimension1.ZipMerge(dimension2, (x, y) => x - y).ToArray());
    }

    /// <summary>
    /// Raises a dimension to a given exponent, multiplying all its exponents by the given exponent.
    /// </summary>
    /// <param name="dimension">The dimension to raise.</param>
    /// <param name="exponent">The exponent to raise the dimension to.</param>
    /// <returns>A new <see cref="Dimension"/> resulting from the exponentiation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dimension"/> is null.</exception>
    /// <example>
    /// <code>
    /// var length = new Dimension(1, 0, 0);
    /// var area = length ^ 2;
    /// // area is a new Dimension with exponents [2, 0, 0]
    /// </code>
    /// </example>
    public static Dimension operator ^(Dimension dimension, float exponent)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();

        return new Dimension(dimension.Select(e => e*exponent).ToArray());
    }
}
