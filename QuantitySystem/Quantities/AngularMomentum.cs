namespace QuantitySystem.Quantities
{
    public class AngularMomentum<T> : DerivedQuantity<T>
    {
        public AngularMomentum()
            : base(1, new MassMomentOfInertia<T>(), new AngularVelocity<T>())
        {
        }

        public AngularMomentum(float exponent)
            : base(exponent, new MassMomentOfInertia<T>(exponent), new AngularVelocity<T>(exponent))
        {
        }


        public static implicit operator AngularMomentum<T>(T value)
        {
            AngularMomentum<T> Q = new()
            {
                Value = value
            };

            return Q;
        }

    }
}
