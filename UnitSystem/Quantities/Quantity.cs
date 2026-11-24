namespace UnitSystem.Quantities;

/// <summary>
/// Represents a quantity with an amount and a unit.
/// </summary>
/// <typeparam name="T">The type of the amount.</typeparam>
public abstract partial class Quantity<T>
    where T : struct
{
    /// <summary>
    /// Initializes a quantity using the unit's default point or difference semantics.
    /// </summary>
    protected Quantity(T amount, Unit unit)
        : this(amount, unit, unit.SupportsPoints)
    {
    }

    /// <summary>
    /// Initializes a quantity with explicit point or difference semantics.
    /// </summary>
    protected Quantity(T amount, Unit unit, bool isPoint)
    {
        Amount = amount;
        Unit = unit;
        IsPoint = isPoint;
        CoherentUnit = unit.System.MakeCoherent(unit);
        CoherentAmount = unit.ToCoherent(amount, isPoint);
    }

    /// <summary>
    /// Initializes a converted quantity from its canonical value.
    /// </summary>
    protected Quantity(T amount, Unit unit, Quantity<T> coherent)
    {
        Amount = amount;
        Unit = unit;
        IsPoint = coherent.IsPoint;
        CoherentUnit = coherent.CoherentUnit;
        CoherentAmount = coherent.CoherentAmount;
    }

    /// <summary>
    /// Gets the amount in <see cref="Unit"/>.
    /// </summary>
    public T Amount { get; }

    /// <summary>
    /// Gets the unit of the amount.
    /// </summary>
    public Unit Unit { get; }

    /// <summary>
    /// Gets whether the quantity represents a point on an affine scale, such as a temperature reading.
    /// </summary>
    public bool IsPoint { get; }

    internal T CoherentAmount { get; }
    internal Unit CoherentUnit { get; }

    /// <summary>
    /// Converts the quantity to a compatible unit, preserving its point or difference semantics.
    /// </summary>
    [Pure]
    public Quantity<T> Convert(Unit unit)
    {
        Check.UnitsAreSameDimension(Unit, unit);
        if (Unit == unit) return this;

        return CreateQuantity(unit.FromCoherent(CoherentAmount, IsPoint), unit, ToCoherent());
    }

    internal Quantity<T> ToCoherent()
        => Unit.IsCoherent ? this : CreateQuantity(CoherentAmount, CoherentUnit, this);

    /// <summary>
    /// Creates a quantity with the supplied amount, unit, and canonical value.
    /// </summary>
    [Pure]
    protected abstract Quantity<T> CreateQuantity(T amount, Unit unit, Quantity<T> coherent);
}
