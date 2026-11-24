namespace UnitSystem;

public abstract partial class Unit
{
    /// <summary>
    /// Computes the natural logarithm of a dimensionless unit's scale factor.
    /// </summary>
    [Pure]
    public static double Ln(Unit unit) => Math.Log(GetLogarithmScale(unit));

    /// <summary>
    /// Computes the base-10 logarithm of a dimensionless unit's scale factor.
    /// </summary>
    [Pure]
    public static double Lg(Unit unit) => Math.Log10(GetLogarithmScale(unit));

    /// <summary>
    /// Computes the logarithm of a dimensionless unit's scale factor to the specified base.
    /// </summary>
    [Pure]
    public static double Log(Unit unit, double @base) => Math.Log(GetLogarithmScale(unit), @base);

    private static double GetLogarithmScale(Unit unit)
    {
        Check.Argument(unit, nameof(unit)).IsNotNull();
        Check.EnsureAbsoluteUnit(unit);
        if (unit.Dimension.Count != 0)
        {
            throw new InvalidOperationException("Logarithms require a dimensionless unit.");
        }

        return unit.Factor;
    }
}
