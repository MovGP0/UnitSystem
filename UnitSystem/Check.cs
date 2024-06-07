using UnitSystem.Dimensions;
using UnitSystem.Extensions;

namespace UnitSystem;

/// <summary>
/// Provides a set of static methods to perform various validation checks on units and arguments.
/// </summary>
internal static class Check
{
    /// <summary>
    /// Checks if two units belong to the same unit system.
    /// </summary>
    /// <param name="unit1">The first unit to compare.</param>
    /// <param name="unit2">The second unit to compare.</param>
    /// <exception cref="InvalidOperationException">Thrown when the units do not belong to the same unit system.</exception>
    /// <example>
    /// <code>
    /// Unit unit1 = Unit.Meter;
    /// Unit unit2 = Unit.Kilogram;
    /// Check.UnitsAreFromSameSystem(unit1, unit2);
    /// // Throws InvalidOperationException if units are not from the same system.
    /// </code>
    /// </example>
    public static void UnitsAreFromSameSystem(Unit unit1, Unit unit2)
    {
        if (!unit1.HasSameSystem(unit2))
        {
            throw new InvalidOperationException(Messages.UnitsNotSameSystem);
        }
    }

    /// <summary>
    /// Checks if two units have the same dimension.
    /// </summary>
    /// <param name="unit1">The first unit to compare.</param>
    /// <param name="unit2">The second unit to compare.</param>
    /// <exception cref="InvalidOperationException">Thrown when the units do not have the same dimension.</exception>
    /// <example>
    /// <code>
    /// Unit unit1 = Unit.Meter;
    /// Unit unit2 = Unit.Second;
    /// Check.UnitsAreSameDimension(unit1, unit2);
    /// // Throws InvalidOperationException if units do not have the same dimension.
    /// </code>
    /// </example>
    public static void UnitsAreSameDimension(Unit unit1, Unit unit2)
    {
        if (!unit1.HasSameDimension(unit2))
        {
            throw new InvalidOperationException(Messages.UnitsNotSameDimension);
        }
    }

    /// <summary>
    /// Checks if a unit system is aware of a given dimension.
    /// </summary>
    /// <param name="system">The unit system to check.</param>
    /// <param name="dimension">The dimension to check for.</param>
    /// <exception cref="InvalidOperationException">Thrown when the unit system does not know the specified dimension.</exception>
    /// <example>
    /// <code>
    /// IUnitSystem system = UnitSystem.SI;
    /// Dimension dimension = Dimension.Length;
    /// Check.SystemKnowsDimension(system, dimension);
    /// // Throws InvalidOperationException if the system does not know the dimension.
    /// </code>
    /// </example>
    public static void SystemKnowsDimension(IUnitSystem system, Dimension dimension)
    {
        if (system.NumberOfDimensions < dimension.Count)
        {
            throw new InvalidOperationException(Messages.DimensionUnknown.FormatWith(system.Name,
                system.NumberOfDimensions));
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="ArgumentCheck{T}"/> to validate an argument.
    /// </summary>
    /// <typeparam name="T">The type of the argument to validate.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="paramName">The name of the parameter representing the argument.</param>
    /// <returns>An instance of <see cref="ArgumentCheck{T}"/>.</returns>
    /// <example>
    /// <code>
    /// var check = Check.Argument("test", "paramName");
    /// check.IsNotNull();
    /// // Validates that the argument is not null or empty.
    /// </code>
    /// </example>
    public static ArgumentCheck<T> Argument<T>(T argument, string paramName) => new(argument, paramName);

    public static void EnsureAbsoluteUnit(Unit unit)
    {
        if (!unit.IsAbsoluteUnit)
        {
            throw new InvalidOperationException("Operation is not permitted for relative units.");
        }
    }
}
