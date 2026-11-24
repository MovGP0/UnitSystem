namespace UnitSystem.Quantities;

public sealed partial class ScalarQuantity
{
    /// <summary>
    /// Adds two differences, or shifts a point by a difference.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator +(ScalarQuantity left, ScalarQuantity right)
    {
        Check.UnitsAreSameDimension(left.CoherentUnit, right.CoherentUnit);
        if (left.IsPoint && right.IsPoint)
        {
            throw new InvalidOperationException("Two points cannot be added.");
        }

        return new ScalarQuantity(left.CoherentAmount + right.CoherentAmount, left.CoherentUnit,
            isPoint: left.IsPoint || right.IsPoint);
    }

    /// <summary>
    /// Subtracts two quantities. Subtracting two points yields a difference.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator -(ScalarQuantity left, ScalarQuantity right)
    {
        Check.UnitsAreSameDimension(left.CoherentUnit, right.CoherentUnit);
        if (!left.IsPoint && right.IsPoint)
        {
            throw new InvalidOperationException("A point cannot be subtracted from a difference.");
        }

        return new ScalarQuantity(left.CoherentAmount - right.CoherentAmount, left.CoherentUnit,
            isPoint: left.IsPoint && !right.IsPoint);
    }

    /// <summary>
    /// Multiplies two linear quantities.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator *(ScalarQuantity left, ScalarQuantity right)
    {
        left.EnsureLinear();
        right.EnsureLinear();
        return new ScalarQuantity(left.CoherentAmount * right.CoherentAmount, left.CoherentUnit * right.CoherentUnit,
            isPoint: false);
    }

    /// <summary>
    /// Multiplies a linear quantity by a factor.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator *(ScalarQuantity quantity, double factor)
    {
        quantity.EnsureLinear();
        return new ScalarQuantity(quantity.CoherentAmount * factor, quantity.CoherentUnit, isPoint: false);
    }

    /// <inheritdoc cref="operator *(ScalarQuantity, double)" />
    [Pure]
    public static ScalarQuantity operator *(double factor, ScalarQuantity quantity) => quantity * factor;

    /// <summary>
    /// Divides two linear quantities.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator /(ScalarQuantity left, ScalarQuantity right)
    {
        left.EnsureLinear();
        right.EnsureLinear();
        return new ScalarQuantity(left.CoherentAmount / right.CoherentAmount, left.CoherentUnit / right.CoherentUnit,
            isPoint: false);
    }

    /// <summary>
    /// Divides a linear quantity by a factor.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator /(ScalarQuantity quantity, double factor)
    {
        quantity.EnsureLinear();
        return new ScalarQuantity(quantity.CoherentAmount / factor, quantity.CoherentUnit, isPoint: false);
    }

    /// <summary>
    /// Divides a factor by a linear quantity.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator /(double factor, ScalarQuantity quantity)
    {
        quantity.EnsureLinear();
        return new ScalarQuantity(factor / quantity.CoherentAmount, quantity.CoherentUnit ^ -1, isPoint: false);
    }

    /// <summary>
    /// Raises a linear quantity to the specified power.
    /// </summary>
    [Pure]
    public static ScalarQuantity operator ^(ScalarQuantity quantity, int exponent)
    {
        quantity.EnsureLinear();
        return new ScalarQuantity(Math.Pow(quantity.CoherentAmount, exponent), quantity.CoherentUnit ^ exponent,
            isPoint: false);
    }

    private void EnsureLinear()
    {
        if (IsPoint)
        {
            throw new InvalidOperationException("Multiplication, division, and powers require a difference or a linear quantity.");
        }
    }
}
