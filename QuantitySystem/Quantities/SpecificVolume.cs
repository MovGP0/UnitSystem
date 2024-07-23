using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class SpecificVolume<T> : DerivedQuantity<T>
    {

        public SpecificVolume()
            : base(1, new Volume<T>(), new Mass<T>(-1))
        {
        }

        public SpecificVolume(float exponent)
            : base(exponent, new Volume<T>(exponent), new Mass<T>(-1 * exponent))
        {
        }


        public static implicit operator SpecificVolume<T>(T value)
        {
            SpecificVolume<T> Q = new()
            {
                Value = value
            };

            return Q;
        }


    }
}
