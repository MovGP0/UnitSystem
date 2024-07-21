using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Power<T> : DerivedQuantity<T>
    {
        public Power()
            : base(1, new Energy<T>(), new Time<T>(-1))
        {
        }

        public Power(float exponent)
            : base(exponent, new Energy<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator Power<T>(T value)
        {
            Power<T> Q = new Power<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
