using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class Speed<T> : DerivedQuantity<T>
{
    public Speed()
        : base(1, new Length<T>(), new Time<T>(-1))
    {
    }

    public Speed(float exponent)
        : base(exponent, new Length<T>(exponent), new Time<T>(-1 * exponent))
    {
    }


    public static implicit operator Speed<T>(T value)
    {
        Speed<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}