using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Energy<T> : DerivedQuantity<T>
    {
        public Energy()
            : base(1, new Force<T>(), new Length<T>())
        {
        }

        public Energy(float exponent)
            : base(exponent, new Force<T>(exponent), new Length<T>(exponent))
        {
        }


        public static implicit operator Energy<T>(T value)
        {
            Energy<T> Q = new Energy<T>();

            Q.Value = value;

            return Q;
        }


    }
}
