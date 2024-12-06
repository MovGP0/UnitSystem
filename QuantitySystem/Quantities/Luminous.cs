using QuantitySystem.Quantities.BaseQuantities;


namespace QuantitySystem.Quantities;

public class Luminous<T> : DerivedQuantity<T>
{

    public Luminous()
        : base(1, new LuminousIntensity<T>(), new Area<T>(-1))
    {
    }

    public Luminous(float exponent)
        : base(exponent, new LuminousIntensity<T>(exponent), new Area<T>(-exponent))
    {
    }


    public static implicit operator Luminous<T>(T value)
    {
        Luminous<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}