using UnitSystem.Dimensions;

namespace UnitSystem;

/// <summary>
/// Provides methods for creating units within a specific unit system.
/// </summary>
public sealed class UnitFactory : IUnitFactory
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
    /// <param name="inherentPrefix">Indicates the inherent prefix.</param>
    /// <returns>A <see cref="KnownUnit"/> with the specified properties.</returns>
    [Pure]
    public KnownUnit CreateUnit(
        IUnitSystem system,
        double factor,
        double offset,
        Dimension dimension,
        string symbol,
        string name,
        string inherentPrefix)
    {
        return new(system, factor, offset, dimension, symbol, name, inherentPrefix);
    }

    /// <summary>
    /// Creates a unit with the specified properties.
    /// </summary>
    /// <param name="system">The unit system to which the unit belongs.</param>
    /// <param name="factor">The factor by which the unit is multiplied.</param>
    /// <param name="offset">The offset of the unit from zero.</param>
    /// <param name="dimension">The dimension of the unit.</param>
    /// <returns>A <see cref="Unit"/> with the specified properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the unit system does not know the specified dimension.</exception>
    [Pure]
    public Unit CreateUnit(IUnitSystem system, double factor, double offset, Dimension dimension)
    {
        Check.SystemKnowsDimension(system, dimension);

        return new DerivedUnit(system, factor, offset, dimension);
    }
}
