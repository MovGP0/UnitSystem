using ShInUeXx.Numerics;

namespace UnitSystem.Dimensions;

public sealed partial class Dimension
{
    /// <summary>
    /// Approximates the natural logarithm of each nonzero exponent.
    /// </summary>
    [Obsolete("For physical quantities, take the logarithm of a dimensionless ratio.")]
    public static Dimension Ln(Dimension dimension) => MapLogarithm(dimension, Math.Log);

    /// <summary>
    /// Approximates the base-10 logarithm of each nonzero exponent.
    /// </summary>
    [Obsolete("For physical quantities, take the logarithm of a dimensionless ratio.")]
    public static Dimension Lg(Dimension dimension) => MapLogarithm(dimension, Math.Log10);

    /// <summary>
    /// Approximates the logarithm of each nonzero exponent to the specified base.
    /// </summary>
    [Obsolete("For physical quantities, take the logarithm of a dimensionless ratio.")]
    public static Dimension Log(Dimension dimension, double @base)
        => MapLogarithm(dimension, exponent => Math.Log(exponent, @base));

    private static Dimension MapLogarithm(Dimension dimension, Func<double, double> logarithm)
    {
        ArgumentNullException.ThrowIfNull(dimension);
        return new Dimension(dimension.Select(exponent =>
            exponent == Rational.Zero ? Rational.Zero : Rational.From(logarithm(exponent.ToDouble())))
            .ToArray());
    }
}
