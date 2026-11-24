namespace UnitSystem.Quantities;

/// <summary>
/// Represents a double-precision measurement or a point on an affine scale.
/// </summary>
public sealed partial class ScalarQuantity
{
    /// <summary>
    /// Creates a quantity. Units in an affine dimension, including kelvin, construct points by default.
    /// </summary>
    public ScalarQuantity(double amount, Unit unit)
        : this(amount, unit, unit.SupportsPoints)
    {
    }

    /// <summary>
    /// Creates a quantity, explicitly specifying whether the amount is a point or a difference.
    /// Use <c>false</c> for intervals such as a change of ten degrees Celsius.
    /// </summary>
    public ScalarQuantity(double amount, Unit unit, bool isPoint)
        : this(amount, unit, isPoint, unit.System.MakeCoherent(unit), unit.ToCoherent(amount, isPoint))
    {
    }

    private ScalarQuantity(double amount, Unit unit, bool isPoint, Unit coherentUnit, double coherentAmount)
    {
        Amount = amount;
        Unit = unit;
        IsPoint = isPoint;
        CoherentUnit = coherentUnit;
        CoherentAmount = coherentAmount;
    }

    /// <summary>
    /// Gets the amount in <see cref="Unit"/>.
    /// </summary>
    public double Amount { get; }

    /// <summary>
    /// Gets the unit of the amount.
    /// </summary>
    public Unit Unit { get; }

    /// <summary>
    /// Gets whether the quantity represents a point on an affine scale, such as a temperature reading.
    /// </summary>
    public bool IsPoint { get; }

    internal double CoherentAmount { get; }

    internal Unit CoherentUnit { get; }

    /// <summary>
    /// Converts the quantity to a compatible unit, preserving its point or difference semantics.
    /// </summary>
    [Pure]
    public ScalarQuantity Convert(Unit unit)
    {
        Check.UnitsAreSameDimension(Unit, unit);
        if (Unit == unit)
            return this;

        return new ScalarQuantity(unit.FromCoherent(CoherentAmount, IsPoint), unit, IsPoint, CoherentUnit, CoherentAmount);
    }
}
