namespace UnitSystem.Quantities;

/// <summary>
/// Represents a quantity with a specified amount and unit, allowing for different amount data types such as <see cref="double"/>, <see cref="Vector{T}"/>, or <see cref="Matrix{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the amount, which must be a struct.</typeparam>
public abstract partial class Quantity<T>
    where T : struct
{
    /// <summary>
    /// The coherent quantity, which is a standardized representation of the quantity.
    /// </summary>
    internal readonly Quantity<T> _coherent;

    /// <summary>
    /// The hash code for the quantity.
    /// </summary>
    internal readonly int _hashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Quantity{T}"/> class.
    /// </summary>
    /// <param name="amount">The amount of the quantity.</param>
    /// <param name="unit">The unit of the quantity.</param>
    protected Quantity(T amount, Unit unit)
    {
        Amount = amount;
        Unit = unit;
        // ReSharper disable once VirtualMemberCallInConstructor
        _coherent = ToCoherent();
        _hashCode = GenerateHashCode();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quantity{T}"/> class with a specified coherent quantity.
    /// </summary>
    /// <param name="amount">The amount of the quantity.</param>
    /// <param name="unit">The unit of the quantity.</param>
    /// <param name="coherent">The coherent quantity.</param>
    protected Quantity(T amount, Unit unit, Quantity<T> coherent)
    {
        Amount = amount;
        Unit = unit;
        _coherent = coherent;
        _hashCode = GenerateHashCode();
    }

    /// <summary>
    /// Gets the amount of the quantity.
    /// </summary>
    public T Amount { get; }

    /// <summary>
    /// Gets the unit of the quantity.
    /// </summary>
    public Unit Unit { get; }

    /// <summary>
    /// Converts the quantity to a specified unit.
    /// </summary>
    /// <param name="unit">The target unit to convert to.</param>
    /// <returns>The converted quantity.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the units are not of the same dimension.</exception>
    [Pure]
    public Quantity<T> Convert(Unit unit)
    {
        if (Unit == unit) return this;

        Check.UnitsAreSameDimension(Unit, unit);
        return CreateQuantity(_coherent.Amount.Divide(unit.Factor), unit, _coherent);
    }

    /// <summary>
    /// Converts the quantity to its coherent representation.
    /// </summary>
    /// <returns>The coherent quantity.</returns>
    [Pure]
    internal virtual Quantity<T> ToCoherent()
    {
        var unit = Unit;

        if (unit.IsCoherent)
        {
            return this;
        }

        var coherentUnit = unit.System.MakeCoherent(unit);

        return CreateQuantity(Amount.Multiply(unit.Factor), coherentUnit, _coherent);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Quantity{T}"/> class.
    /// </summary>
    /// <param name="amount">The amount of the quantity.</param>
    /// <param name="unit">The unit of the quantity.</param>
    /// <param name="coherent">The coherent quantity.</param>
    /// <returns>A new instance of the <see cref="Quantity{T}"/> class.</returns>
    [Pure]
    protected abstract Quantity<T> CreateQuantity(T amount, Unit unit, Quantity<T> coherent);

    /// <summary>
    /// Generates a hash code for the quantity.
    /// </summary>
    /// <returns>The hash code for the quantity.</returns>
    [Pure]
    private int GenerateHashCode() => HashCode.Combine(Amount, Unit, _coherent);
}
