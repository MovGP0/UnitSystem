namespace UnitSystem.Quantities;

public sealed partial class ScalarQuantity : IEquatable<ScalarQuantity>
{
    /// <summary>
    /// Gets the hash code for the scalar quantity.
    /// </summary>
    /// <returns>The hash code of the scalar quantity.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(CoherentUnit, CoherentAmount, IsPoint);

    /// <summary>
    /// Determines whether the specified <see cref="ScalarQuantity"/> is equal to the current <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="other">The <see cref="ScalarQuantity"/> to compare with the current <see cref="ScalarQuantity"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="ScalarQuantity"/> is equal to the current <see cref="ScalarQuantity"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(ScalarQuantity? other)
    {
        if (other is null)
            return false;

        return IsPoint == other.IsPoint
               && CoherentUnit == other.CoherentUnit
               && CoherentAmount.Equals(other.CoherentAmount);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="ScalarQuantity"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="ScalarQuantity"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj)
        => Equals(obj as ScalarQuantity);

    /// <summary>
    /// Determines whether two <see cref="ScalarQuantity"/> instances are equal.
    /// </summary>
    /// <param name="quantity1">The first <see cref="ScalarQuantity"/> to compare.</param>
    /// <param name="quantity2">The second <see cref="ScalarQuantity"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="ScalarQuantity"/> instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(ScalarQuantity? quantity1, ScalarQuantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2))
            return true;
        if (quantity1 is null)
            return false;

        return quantity1.Equals(quantity2);
    }

    /// <summary>
    /// Determines whether two <see cref="ScalarQuantity"/> instances are not equal.
    /// </summary>
    /// <param name="quantity1">The first <see cref="ScalarQuantity"/> to compare.</param>
    /// <param name="quantity2">The second <see cref="ScalarQuantity"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="ScalarQuantity"/> instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(ScalarQuantity? quantity1, ScalarQuantity? quantity2)
        => !(quantity1 == quantity2);
}
