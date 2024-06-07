namespace UnitSystem.Presentation;

/// <summary>
/// Provides a concrete implementation of the <see cref="IUnitDialect"/> interface.
/// </summary>
internal class UnitDialect : IUnitDialect
{
    /// <inheritdoc />
    public Token Division => new("/", "\u00f7");

    /// <inheritdoc />
    public Token Multiplication => new(" ", "\u00B7", "\u00D7", "*");

    /// <inheritdoc />
    public Token Exponentiation => new("^");
}
