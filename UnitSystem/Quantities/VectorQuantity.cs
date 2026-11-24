using UnitSystem.Orientations;

namespace UnitSystem.Quantities;

/// <summary>A three-dimensional displacement or vector with a shared unit.</summary>
/// <remarks>All components are linear quantities or differences expressed in the same Cartesian frame.</remarks>
public sealed class VectorQuantity
{
    /// <summary>Creates a vector from three components expressed in the specified unit.</summary>
    public VectorQuantity(double x, double y, double z, Unit unit)
    {
        ArgumentNullException.ThrowIfNull(unit);
        X = x;
        Y = y;
        Z = z;
        Unit = unit;
    }

    /// <summary>Gets the X component in <see cref="Unit"/>.</summary>
    public double X { get; }

    /// <summary>Gets the Y component in <see cref="Unit"/>.</summary>
    public double Y { get; }

    /// <summary>Gets the Z component in <see cref="Unit"/>.</summary>
    public double Z { get; }

    /// <summary>Gets the shared unit of the components.</summary>
    public Unit Unit { get; }

    /// <summary>Converts the components to a compatible unit using difference semantics.</summary>
    public VectorQuantity Convert(Unit unit)
    {
        ArgumentNullException.ThrowIfNull(unit);
        Check.UnitsAreSameDimension(Unit, unit);
        if (Unit == unit)
        {
            return this;
        }

        var (x, y, z) = CoherentComponents();
        return new VectorQuantity(
            unit.FromCoherent(x, isPoint: false),
            unit.FromCoherent(y, isPoint: false),
            unit.FromCoherent(z, isPoint: false),
            unit);
    }

    /// <summary>Rotates the components and preserves their unit.</summary>
    public VectorQuantity Rotate(Rotation3D rotation)
    {
        ArgumentNullException.ThrowIfNull(rotation);
        var (x, y, z) = rotation.Apply(X, Y, Z);
        return new VectorQuantity(x, y, z, Unit);
    }

    /// <summary>Calculates the dot product as a scalar quantity in the coherent product unit.</summary>
    public ScalarQuantity Dot(VectorQuantity other)
    {
        ArgumentNullException.ThrowIfNull(other);
        var unit = ProductUnit(other);
        var (x, y, z) = CoherentComponents();
        var (otherX, otherY, otherZ) = other.CoherentComponents();
        return new ScalarQuantity((x * otherX) + (y * otherY) + (z * otherZ), unit, isPoint: false);
    }

    /// <summary>Calculates this vector crossed with the other vector in a right-handed frame.</summary>
    public VectorQuantity Cross(VectorQuantity other)
    {
        ArgumentNullException.ThrowIfNull(other);
        var unit = ProductUnit(other);
        var (x, y, z) = CoherentComponents();
        var (otherX, otherY, otherZ) = other.CoherentComponents();

        return new VectorQuantity(
            (y * otherZ) - (z * otherY),
            (z * otherX) - (x * otherZ),
            (x * otherY) - (y * otherX),
            unit);
    }

    /// <summary>Adds two vectors componentwise in the unit of the left operand.</summary>
    public static VectorQuantity operator +(VectorQuantity left, VectorQuantity right)
    {
        var converted = right.Convert(left.Unit);
        return new VectorQuantity(left.X + converted.X, left.Y + converted.Y, left.Z + converted.Z, left.Unit);
    }

    /// <summary>Subtracts two vectors componentwise in the unit of the left operand.</summary>
    public static VectorQuantity operator -(VectorQuantity left, VectorQuantity right)
    {
        var converted = right.Convert(left.Unit);
        return new VectorQuantity(left.X - converted.X, left.Y - converted.Y, left.Z - converted.Z, left.Unit);
    }

    private (double X, double Y, double Z) CoherentComponents()
        => (Unit.ToCoherent(X, isPoint: false), Unit.ToCoherent(Y, isPoint: false), Unit.ToCoherent(Z, isPoint: false));

    private Unit ProductUnit(VectorQuantity other)
        => Unit.System.MakeCoherent(Unit) * other.Unit.System.MakeCoherent(other.Unit);
}
