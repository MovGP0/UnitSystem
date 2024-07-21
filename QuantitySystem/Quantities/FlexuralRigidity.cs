namespace QuantitySystem.Quantities
{
    public class FlexuralRigidity<T> : DerivedQuantity<T>
    {
        public FlexuralRigidity()
            : base(1, new Force<T>(), new PolarArea<T>())
        {
        }

        public FlexuralRigidity(float exponent)
            : base(exponent, new Force<T>(exponent), new PolarArea<T>(exponent))
        {
        }


        public static implicit operator FlexuralRigidity<T>(T value)
        {
            FlexuralRigidity<T> Q = new FlexuralRigidity<T>
            {
                Value = value
            };

            return Q;
        }

    }
}
