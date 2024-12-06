using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Quantities;

public class RotationalStiffness<T> : DerivedQuantity<T>
{
    public RotationalStiffness()
        : base(1, new Torque<T>(), new Angle<T>(-1))
    {
    }

    public RotationalStiffness(float exponent)
        : base(exponent, new Torque<T>(exponent), new Angle<T>(-1 * exponent))
    {
    }


    public static implicit operator RotationalStiffness<T>(T value)
    {
        RotationalStiffness<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}