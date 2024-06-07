namespace UnitSystem.Quantities;

public sealed partial class ScalarQuantity
{
    /// <summary>
    /// Adds two scalar quantities.
    /// </summary>
    /// <param name="q1">The first scalar quantity.</param>
    /// <param name="q2">The second scalar quantity.</param>
    /// <returns>The sum of the two scalar quantities.</returns>
    [Pure]
    public static ScalarQuantity operator +(ScalarQuantity q1, ScalarQuantity q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new ScalarQuantity(q1._coherent.Amount + q2._coherent.Amount, q1._coherent.Unit);
    }

    /// <summary>
    /// Subtracts the second scalar quantity from the first scalar quantity.
    /// </summary>
    /// <param name="q1">The first scalar quantity.</param>
    /// <param name="q2">The second scalar quantity.</param>
    /// <returns>The difference between the two scalar quantities.</returns>
    [Pure]
    public static ScalarQuantity operator -(ScalarQuantity q1, ScalarQuantity q2)
    {
        Check.UnitsAreSameDimension(q1.Unit, q2.Unit);
        return new ScalarQuantity(q1._coherent.Amount - q2._coherent.Amount, q1._coherent.Unit);
    }

    /// <summary>
    /// Multiplies two scalar quantities.
    /// </summary>
    /// <param name="q1">The first scalar quantity.</param>
    /// <param name="q2">The second scalar quantity.</param>
    /// <returns>The product of the two scalar quantities.</returns>
    [Pure]
    public static ScalarQuantity operator *(ScalarQuantity q1, ScalarQuantity q2)
        => new(q1._coherent.Amount * q2._coherent.Amount, q1._coherent.Unit * q2._coherent.Unit);

    /// <summary>
    /// Multiplies a scalar quantity by a factor.
    /// </summary>
    /// <param name="q">The scalar quantity.</param>
    /// <param name="factor">The factor to multiply by.</param>
    /// <returns>The product of the scalar quantity and the factor.</returns>
    [Pure]
    public static ScalarQuantity operator *(ScalarQuantity q, double factor)
        => new(q._coherent.Amount * factor, q._coherent.Unit);

    /// <summary>
    /// Multiplies a factor by a scalar quantity.
    /// </summary>
    /// <param name="factor">The factor to multiply by.</param>
    /// <param name="q">The scalar quantity.</param>
    /// <returns>The product of the factor and the scalar quantity.</returns>
    [Pure]
    public static ScalarQuantity operator *(double factor, ScalarQuantity q)
        => new(q._coherent.Amount * factor, q._coherent.Unit);

    /// <summary>
    /// Divides the first scalar quantity by the second scalar quantity.
    /// </summary>
    /// <param name="q1">The first scalar quantity.</param>
    /// <param name="q2">The second scalar quantity.</param>
    /// <returns>The quotient of the two scalar quantities.</returns>
    [Pure]
    public static ScalarQuantity operator /(ScalarQuantity q1, ScalarQuantity q2)
        => new(q1._coherent.Amount / q2._coherent.Amount, q1._coherent.Unit / q2._coherent.Unit);

    /// <summary>
    /// Divides a scalar quantity by a factor.
    /// </summary>
    /// <param name="q">The scalar quantity.</param>
    /// <param name="factor">The factor to divide by.</param>
    /// <returns>The quotient of the scalar quantity and the factor.</returns>
    [Pure]
    public static ScalarQuantity operator /(ScalarQuantity q, double factor)
        => new(q._coherent.Amount / factor, q._coherent.Unit);

    /// <summary>
    /// Divides a factor by a scalar quantity.
    /// </summary>
    /// <param name="factor">The factor to divide by.</param>
    /// <param name="q">The scalar quantity.</param>
    /// <returns>The quotient of the factor and the scalar quantity.</returns>
    [Pure]
    public static ScalarQuantity operator /(double factor, ScalarQuantity q)
        => new(q._coherent.Amount / factor, q._coherent.Unit);

    /// <summary>
    /// Raises a scalar quantity to the power of an exponent.
    /// </summary>
    /// <param name="q">The scalar quantity.</param>
    /// <param name="exponent">The exponent.</param>
    /// <returns>The scalar quantity raised to the power of the exponent.</returns>
    [Pure]
    public static ScalarQuantity operator ^(ScalarQuantity q, int exponent)
        => new(Math.Pow(q._coherent.Amount, exponent), q._coherent.Unit ^ exponent);
}
