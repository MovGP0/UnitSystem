namespace UnitSystem.Quantities;

/// <summary>
/// Represents a scalar measurement or a point on an affine scale.
/// </summary>
public sealed partial class ScalarQuantity : Quantity<double>
{
    /// <summary>
    /// Creates a quantity. Units in an affine dimension, including kelvin, construct points by default.
    /// </summary>
    public ScalarQuantity(double amount, Unit unit)
        : base(amount, unit)
    {
    }

    /// <summary>
    /// Creates a quantity, explicitly specifying whether the amount is a point or a difference.
    /// Use <c>false</c> for intervals such as a change of ten degrees Celsius.
    /// </summary>
    public ScalarQuantity(double amount, Unit unit, bool isPoint)
        : base(amount, unit, isPoint)
    {
    }

    private ScalarQuantity(double amount, Unit unit, Quantity<double> coherent)
        : base(amount, unit, coherent)
    {
    }

    /// <inheritdoc />
    [Pure]
    protected override Quantity<double> CreateQuantity(double amount, Unit unit, Quantity<double> coherent)
        => new ScalarQuantity(amount, unit, coherent);

    /// <inheritdoc cref="Quantity{T}.Convert(Unit)"/>
    [Pure]
    new public ScalarQuantity Convert(Unit unit) => (ScalarQuantity)base.Convert(unit);
}
