using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

class ElectricalConductivity<T> : DerivedQuantity<T>
{
    public ElectricalConductivity()
        : base(1, new ElectricConductance<T>(), new Length<T>(-1))
    {
    }

    public ElectricalConductivity(float exponent)
        : base(exponent, new ElectricConductance<T>(exponent), new Length<T>(-1 * exponent))
    {
    }


    public static implicit operator ElectricalConductivity<T>(T value)
    {
        ElectricalConductivity<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}