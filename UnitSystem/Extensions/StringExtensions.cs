namespace UnitSystem.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/> to support additional operations.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Formats a string using the specified arguments.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="args">The arguments to format the string with.</param>
    /// <returns>A copy of <paramref name="format"/> with the format items replaced by the string representation of corresponding objects in <paramref name="args"/>.</returns>
    /// <example>
    /// <code>
    /// string result = "Hello, {0}!".FormatWith("World");
    /// // Result: "Hello, World!"
    /// </code>
    /// </example>
    [Pure]
    public static string FormatWith(this string format, params object[] args)
        => string.Format(format, args);
}
