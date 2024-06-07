namespace UnitSystem.Presentation;

/// <summary>
/// Represents a token that holds multiple string representations.
/// </summary>
/// <remarks>
/// This class extends <see cref="ImmutableCollection{T}"/> to provide a read-only collection of string representations.
/// </remarks>
public class Token : ImmutableCollection<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class with the specified string representations.
    /// </summary>
    /// <param name="representations">The string representations of the token.</param>
    public Token(params string[] representations)
        : base(representations)
    {
    }

    /// <summary>
    /// Converts a <see cref="Token"/> to a string array explicitly.
    /// </summary>
    /// <param name="token">The token to convert.</param>
    /// <returns>An array of strings representing the token.</returns>
    public static explicit operator string[](Token token) => token.ToArray();
}
