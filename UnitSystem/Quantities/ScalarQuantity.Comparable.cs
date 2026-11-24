namespace UnitSystem.Quantities;

public sealed partial class ScalarQuantity : IComparable<ScalarQuantity>
{
    /// <summary>
    /// Compares canonical amounts of quantities with the same dimension and point or difference semantics.
    /// </summary>
    /// <exception cref="InvalidOperationException">The quantities have incompatible dimensions or semantics.</exception>
    [Pure]
    public int CompareTo(ScalarQuantity? other)
    {
        if (other is null)
            return 1;

        Check.UnitsAreSameDimension(CoherentUnit, other.CoherentUnit);
        if (IsPoint != other.IsPoint)
        {
            throw new InvalidOperationException("A point cannot be ordered against a difference.");
        }

        return CoherentAmount.CompareTo(other.CoherentAmount);
    }

    /// <inheritdoc cref="CompareTo" />
    [Pure]
    public static bool operator >(ScalarQuantity? left, ScalarQuantity? right) => Compare(left, right) > 0;

    /// <inheritdoc cref="CompareTo" />
    [Pure]
    public static bool operator <(ScalarQuantity? left, ScalarQuantity? right) => Compare(left, right) < 0;

    /// <inheritdoc cref="CompareTo" />
    [Pure]
    public static bool operator >=(ScalarQuantity? left, ScalarQuantity? right) => Compare(left, right) >= 0;

    /// <inheritdoc cref="CompareTo" />
    [Pure]
    public static bool operator <=(ScalarQuantity? left, ScalarQuantity? right) => Compare(left, right) <= 0;

    private static int Compare(ScalarQuantity? left, ScalarQuantity? right)
        => left is null ? (right is null ? 0 : -1) : left.CompareTo(right);
}
