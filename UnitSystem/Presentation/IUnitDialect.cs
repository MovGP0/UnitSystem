namespace UnitSystem.Presentation;

/// <summary>
/// Represents a convention for unit representation.
/// </summary>
/// <remarks>
/// The first representation of a token will take precedence for unit display.
/// </remarks>
public interface IUnitDialect
{
    /// <summary>
    /// Gets the token used for division.
    /// </summary>
    Token Division { get; }

    /// <summary>
    /// Gets the token used for multiplication.
    /// </summary>
    Token Multiplication { get; }

    /// <summary>
    /// Gets the token used for exponentiation.
    /// </summary>
    Token Exponentiation { get; }
}
