namespace UnitSystem.Quantities;

/// <summary>
/// Represents a scalar quantity with a specified amount and unit, inheriting from <see cref="Quantity{T}"/>.
/// </summary>
public sealed partial class ScalarQuantity : Quantity<double>
{
    /// <summary>
    /// Creates a new instance of the <see cref="Quantity{T}"/> class.
    /// </summary>
    /// <param name="amount">The amount of the quantity.</param>
    /// <param name="unit">The unit of the quantity.</param>
    /// <param name="coherent">The coherent quantity.</param>
    /// <returns>A new instance of the <see cref="ScalarQuantity"/> class.</returns>
    [Pure]
    protected override Quantity<double> CreateQuantity(double amount, Unit unit, Quantity<double> coherent) => new ScalarQuantity(amount, unit, coherent);

    /// <summary>
    /// Initializes a new instance of the <see cref="ScalarQuantity"/> class.
    /// </summary>
    /// <param name="amount">The amount of the scalar quantity.</param>
    /// <param name="unit">The unit of the scalar quantity.</param>
    public ScalarQuantity(double amount, Unit unit)
        : base(amount, unit)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScalarQuantity"/> class with a specified coherent quantity.
    /// </summary>
    /// <param name="amount">The amount of the scalar quantity.</param>
    /// <param name="unit">The unit of the scalar quantity.</param>
    /// <param name="coherent">The coherent scalar quantity.</param>
    private ScalarQuantity(double amount, Unit unit, Quantity<double> coherent)
        : base(amount, unit, coherent)
    {
    }

    /// <summary>
    /// Converts the scalar quantity to a specified unit.
    /// </summary>
    /// <param name="unit">The target unit to convert to.</param>
    /// <returns>The converted scalar quantity.</returns>
    [Pure]
    new public ScalarQuantity Convert(Unit unit)
    {
        if (Unit == unit) return this;

        Check.UnitsAreSameDimension(Unit, unit);
        return new ScalarQuantity(_coherent.Amount / unit.Factor, unit, _coherent);
    }

    /// <summary>
    /// Converts the scalar quantity to its coherent representation.
    /// </summary>
    /// <returns>The coherent scalar quantity.</returns>
    [Pure]
    new internal ScalarQuantity ToCoherent()
    {
        var unit = Unit;

        if (unit.IsCoherent)
        {
            return this;
        }

        var coherentUnit = unit.System.MakeCoherent(unit);

        return new ScalarQuantity(unit.Factor * Amount - unit.Offset, coherentUnit);
    }
}
