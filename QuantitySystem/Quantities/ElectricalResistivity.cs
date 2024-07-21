using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class ElectricalResistivity<T> : DerivedQuantity<T>
    {
        public ElectricalResistivity()
            : base(1, new ElectricResistance<T>(), new Length<T>())
        {
        }

        public ElectricalResistivity(float exponent)
            : base(exponent, new ElectricResistance<T>(exponent), new Length<T>(exponent))
        {
        }


        public static implicit operator ElectricalResistivity<T>(T value)
        {
            ElectricalResistivity<T> Q = new ElectricalResistivity<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
