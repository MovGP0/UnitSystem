namespace UnitSystem.Quantities;

public sealed partial class ScalarQuantity : IComparable<ScalarQuantity>
{
    /// <summary>
    /// Compares the current instance with another <see cref="ScalarQuantity"/> and returns an integer that indicates whether the current instance
    /// precedes, follows, or occurs in the same position in the sort order as the other <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="other">The <see cref="ScalarQuantity"/> to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.
    /// The return value has these meanings:
    /// Less than zero: This instance precedes <paramref name="other"/> in the sort order.
    /// Zero: This instance occurs in the same position in the sort order as <paramref name="other"/>.
    /// Greater than zero: This instance follows <paramref name="other"/> in the sort order.</returns>
    [Pure]
    public int CompareTo(ScalarQuantity? other)
    {
        if (this < other) return -1;
        if (this > other) return 1;

        return 0;
    }

    /// <summary>
    /// Determines whether one <see cref="ScalarQuantity"/> is greater than another <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="quantity1">The first <see cref="ScalarQuantity"/> to compare.</param>
    /// <param name="quantity2">The second <see cref="ScalarQuantity"/> to compare.</param>
    /// <returns><c>true</c> if <paramref name="quantity1"/> is greater than <paramref name="quantity2"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator >(ScalarQuantity? quantity1, ScalarQuantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return false;
        if (quantity1 is null) return false;
        if (quantity2 is null) return true;

        return quantity1._coherent.Amount > quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    /// <summary>
    /// Determines whether one <see cref="ScalarQuantity"/> is less than another <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="quantity1">The first <see cref="ScalarQuantity"/> to compare.</param>
    /// <param name="quantity2">The second <see cref="ScalarQuantity"/> to compare.</param>
    /// <returns><c>true</c> if <paramref name="quantity1"/> is less than <paramref name="quantity2"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator <(ScalarQuantity? quantity1, ScalarQuantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return false;
        if (quantity1 is null) return true;
        if (quantity2 is null) return false;

        return quantity1._coherent.Amount < quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    /// <summary>
    /// Determines whether one <see cref="ScalarQuantity"/> is greater than or equal to another <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="quantity1">The first <see cref="ScalarQuantity"/> to compare.</param>
    /// <param name="quantity2">The second <see cref="ScalarQuantity"/> to compare.</param>
    /// <returns><c>true</c> if <paramref name="quantity1"/> is greater than or equal to <paramref name="quantity2"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator >=(ScalarQuantity? quantity1, ScalarQuantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return true;
        if (quantity1 is null) return false;
        if (quantity2 is null) return true;

        return quantity1._coherent.Amount >= quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }

    /// <summary>
    /// Determines whether one <see cref="ScalarQuantity"/> is less than or equal to another <see cref="ScalarQuantity"/>.
    /// </summary>
    /// <param name="quantity1">The first <see cref="ScalarQuantity"/> to compare.</param>
    /// <param name="quantity2">The second <see cref="ScalarQuantity"/> to compare.</param>
    /// <returns><c>true</c> if <paramref name="quantity1"/> is less than or equal to <paramref name="quantity2"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator <=(ScalarQuantity? quantity1, ScalarQuantity? quantity2)
    {
        if (ReferenceEquals(quantity1, quantity2)) return true;
        if (quantity1 is null) return true;
        if (quantity2 is null) return false;

        return quantity1._coherent.Amount <= quantity2._coherent.Amount
               && quantity1._coherent.Unit == quantity2._coherent.Unit;
    }
}
