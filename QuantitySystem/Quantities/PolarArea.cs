using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class PolarArea<T> : DerivedQuantity<T>
{
    public PolarArea()
        : base(1, new Displacement<T>(2))
    {
    }

    public PolarArea(float exponent)
        : base(exponent, new Displacement<T>(2 * exponent))
    {
    }


    public static implicit operator PolarArea<T>(T value)
    {
        PolarArea<T> Q = new()
        {
            Value = value
        };

        return Q;
    }


}