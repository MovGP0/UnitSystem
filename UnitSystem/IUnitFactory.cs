using UnitSystem.Dimensions;

namespace UnitSystem;

/// <summary>
/// Defines methods for creating units within a specific unit system.
/// </summary>
internal interface IUnitFactory
{
    /// <summary>
    /// Creates a known unit with the specified properties.
    /// </summary>
    /// <param name="system">The unit system to which the unit belongs.</param>
    /// <param name="factor">The factor by which the unit is multiplied.</param>
    /// <param name="offset">The offset of the unit from zero.</param>
    /// <param name="dimension">The dimension of the unit.</param>
    /// <param name="symbol">The symbol representing the unit.</param>
    /// <param name="name">The name of the unit.</param>
    /// <param name="inherentPrefix">Indicates whether the unit has an inherent prefix.</param>
    /// <returns>A <see cref="KnownUnit"/> with the specified properties.</returns>
    KnownUnit CreateUnit(
        IUnitSystem system,
        double factor,
        double offset,
        Dimension dimension,
        string symbol,
        string name,
        string inherentPrefix);

    /// <summary>
    /// Creates a unit with the specified properties.
    /// </summary>
    /// <param name="system">The unit system to which the unit belongs.</param>
    /// <param name="factor">The factor by which the unit is multiplied.</param>
    /// <param name="offset">The offset of the unit from zero.</param>
    /// <param name="dimension">The dimension of the unit.</param>
    /// <returns>A <see cref="Unit"/> with the specified properties.</returns>
    Unit CreateUnit(
        IUnitSystem system,
        double factor,
        double offset,
        Dimension dimension);
}
