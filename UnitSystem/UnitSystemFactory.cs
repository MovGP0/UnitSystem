namespace UnitSystem;

/// <summary>
/// Factory class for creating unit systems.
/// </summary>
public static class UnitSystemFactory
{
    /// <summary>
    /// Creates a unit system with the specified name.
    /// </summary>
    /// <param name="name">The name of the unit system to create.</param>
    /// <returns>A new instance of <see cref="IUnitSystem"/> with the specified name.</returns>
    [Pure]
    public static IUnitSystem CreateSystem(string name)
        => new UnitSystem(name);
}
