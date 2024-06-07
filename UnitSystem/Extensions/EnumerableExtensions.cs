namespace UnitSystem.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> to support additional operations.
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Merges two collections by applying an aggregation function to each pair of elements. The merging stops based on the specified condition.
    /// </summary>
    /// <typeparam name="TIn1">The type of elements in the first collection.</typeparam>
    /// <typeparam name="TIn2">The type of elements in the second collection.</typeparam>
    /// <typeparam name="TOut">The type of elements in the resulting collection.</typeparam>
    /// <param name="collection1">The first collection to merge.</param>
    /// <param name="collection2">The second collection to merge.</param>
    /// <param name="aggregation">The function to apply to pairs of elements from the collections.</param>
    /// <param name="matchShortest">If true, the merging stops when the shortest collection is exhausted; otherwise, it stops when the longest collection is exhausted.</param>
    /// <returns>A collection of merged elements.</returns>
    /// <example>
    /// <code>
    /// var list1 = new List&lt;int&gt; { 1, 2, 3 };
    /// var list2 = new List&lt;string&gt; { "a", "b", "c", "d" };
    /// var result = list1.ZipMerge(list2, (i, s) => $"{i}{s}", true);
    /// // Result: { "1a", "2b", "3c" }
    /// </code>
    /// </example>
    [Pure]
    public static IEnumerable<TOut> ZipMerge<TIn1, TIn2, TOut>(
        this IEnumerable<TIn1> collection1,
        IEnumerable<TIn2> collection2,
        Func<TIn1?, TIn2?, TOut> aggregation,
        bool matchShortest = false)
    {
        if (matchShortest)
        {
            return collection1.ZipMerge(collection2, aggregation, (more1, more2) => more1 && more2);
        }

        return collection1.ZipMerge(collection2, aggregation, (more1, more2) => more1 || more2);
    }

    /// <summary>
    /// Merges two collections by applying an aggregation function to each pair of elements. The merging continues as long as the provided check function returns true.
    /// </summary>
    /// <typeparam name="TIn1">The type of elements in the first collection.</typeparam>
    /// <typeparam name="TIn2">The type of elements in the second collection.</typeparam>
    /// <typeparam name="TOut">The type of elements in the resulting collection.</typeparam>
    /// <param name="collection1">The first collection to merge.</param>
    /// <param name="collection2">The second collection to merge.</param>
    /// <param name="aggregation">The function to apply to pairs of elements from the collections.</param>
    /// <param name="check">The function that determines when to stop merging.</param>
    /// <returns>A collection of merged elements.</returns>
    [Pure]
    private static IEnumerable<TOut> ZipMerge<TIn1, TIn2, TOut>(
        this IEnumerable<TIn1> collection1,
        IEnumerable<TIn2> collection2,
        Func<TIn1?, TIn2?, TOut> aggregation,
        Func<bool, bool, bool> check)
    {
        using var enumerator1 = collection1.GetEnumerator();
        using var enumerator2 = collection2.GetEnumerator();

        var more1 = enumerator1.MoveNext();
        var more2 = enumerator2.MoveNext();

        while (check(more1, more2))
        {
            yield return aggregation(
                more1 ? enumerator1.Current : default,
                more2 ? enumerator2.Current : default);

            more1 = enumerator1.MoveNext();
            more2 = enumerator2.MoveNext();
        }
    }

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
