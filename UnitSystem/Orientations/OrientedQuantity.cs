using UnitSystem.Quantities;

namespace UnitSystem.Orientations;

/// <summary>
/// A scalar quantity or one signed Cartesian component with a Siano label.
/// </summary>
/// <remarks>
/// Operands use the same Cartesian frame. The sign of a component is part of the amount.
/// Convert spatial components with <see cref="ToVector"/> to add or rotate them as vectors.
/// </remarks>
public sealed record OrientedQuantity
{
    /// <summary>
    /// Assigns one of the four Siano basis labels to a quantity or component.
    /// </summary>
    public OrientedQuantity(ScalarQuantity quantity, SianoOrientation orientation)
    {
        ArgumentNullException.ThrowIfNull(quantity);
        if (!orientation.IsBasisElement)
        {
            throw new ArgumentOutOfRangeException(nameof(orientation), "A quantity component requires a Siano basis element.");
        }

        Quantity = quantity;
        Orientation = orientation;
    }

    /// <summary>Gets the measurement.</summary>
    public ScalarQuantity Quantity { get; }

    /// <summary>Gets the orientation in the shared frame.</summary>
    public SianoOrientation Orientation { get; }

    /// <summary>
    /// Converts the measurement to a compatible unit and retains its orientation.
    /// </summary>
    public OrientedQuantity Convert(Unit unit) => new(Quantity.Convert(unit), Orientation);

    /// <summary>Creates a vector with this signed quantity on the labeled axis.</summary>
    public VectorQuantity ToVector()
    {
        if (Quantity.IsPoint || Orientation == SianoOrientation.Orientationless)
        {
            throw new InvalidOperationException("A vector component requires a spatial axis and a linear quantity or difference.");
        }

        var amount = Quantity.Amount;
        var unit = Quantity.Unit;
        return new VectorQuantity(
            Orientation == SianoOrientation.X ? amount : 0,
            Orientation == SianoOrientation.Y ? amount : 0,
            Orientation == SianoOrientation.Z ? amount : 0,
            unit);
    }

    /// <summary>
    /// Raises the measurement to an integer power. Even powers are orientationless.
    /// </summary>
    public OrientedQuantity Pow(int exponent)
        => new(Quantity.Pow(exponent), (exponent & 1) == 0 ? SianoOrientation.Orientationless : Orientation);

    /// <summary>Adds measurements with matching orientations.</summary>
    public static OrientedQuantity operator +(OrientedQuantity left, OrientedQuantity right)
    {
        RequireMatchingOrientations(left, right);
        return new OrientedQuantity(left.Quantity + right.Quantity, left.Orientation);
    }

    /// <summary>Subtracts measurements with matching orientations.</summary>
    public static OrientedQuantity operator -(OrientedQuantity left, OrientedQuantity right)
    {
        RequireMatchingOrientations(left, right);
        return new OrientedQuantity(left.Quantity - right.Quantity, left.Orientation);
    }

    /// <summary>Multiplies measurements and combines their orientations.</summary>
    public static OrientedQuantity operator *(OrientedQuantity left, OrientedQuantity right)
        => new(left.Quantity * right.Quantity, left.Orientation * right.Orientation);

    /// <summary>Divides measurements. Each basis orientation is its own inverse.</summary>
    public static OrientedQuantity operator /(OrientedQuantity left, OrientedQuantity right)
        => new(left.Quantity / right.Quantity, left.Orientation * right.Orientation);

    private static void RequireMatchingOrientations(OrientedQuantity left, OrientedQuantity right)
    {
        if (left.Orientation != right.Orientation)
        {
            throw new InvalidOperationException("Addition and subtraction require matching Siano orientations.");
        }
    }
}
