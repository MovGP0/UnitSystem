namespace UnitSystem.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> to support additional operations.
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Computes a hash code for a collection by aggregating the hash codes of its elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to compute the hash code for.</param>
    /// <returns>The computed hash code.</returns>
    /// <example>
    /// <code>
    /// var list = new List&lt;int&gt; { 1, 2, 3 };
    /// int hash = list.Hash();
    /// </code>
    /// </example>
    [Pure]
    public static int Hash<T>(this IEnumerable<T> collection)
    {
        var hash = new HashCode();

        foreach (var item in collection)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }
}
