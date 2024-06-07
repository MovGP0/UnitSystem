using UnitSystem.Dimensions;

namespace UnitSystem;

public abstract partial class Unit
{
    /// <summary>
    /// Computes the natural logarithm (ln) of a unit, applying the logarithm to its factor and dimension.
    /// </summary>
    /// <param name="unit">The unit to apply the natural logarithm to.</param>
    /// <returns>A new <see cref="Unit"/> resulting from the natural logarithm of the original unit.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unit"/> is null.</exception>
    [Pure]
    public static Unit Ln(Unit unit)
    {
        Check.Argument(unit, nameof(unit)).IsNotNull();
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(Math.Log(unit.Factor), Dimension.Ln(unit.Dimension));
    }

    /// <summary>
    /// Computes the base-10 logarithm (lg) of a unit, applying the logarithm to its factor and dimension.
    /// </summary>
    /// <param name="unit">The unit to apply the base-10 logarithm to.</param>
    /// <returns>A new <see cref="Unit"/> resulting from the base-10 logarithm of the original unit.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unit"/> is null.</exception>
    [Pure]
    public static Unit Lg(Unit unit)
    {
        Check.Argument(unit, nameof(unit)).IsNotNull();
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(Math.Log10(unit.Factor), Dimension.Lg(unit.Dimension));
    }

    /// <summary>
    /// Computes the logarithm of a unit to a specified base, applying the logarithm to its factor and dimension.
    /// </summary>
    /// <param name="unit">The unit to apply the logarithm to.</param>
    /// <param name="base">The base of the logarithm.</param>
    /// <returns>A new <see cref="Unit"/> resulting from the logarithm of the original unit to the specified base.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unit"/> is null.</exception>
    [Pure]
    public static Unit Log(Unit unit, double @base)
    {
        Check.Argument(unit, nameof(unit)).IsNotNull();
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(Math.Log(unit.Factor) / Math.Log(@base), Dimension.Log(unit.Dimension, @base));
    }
}
