namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Time<T> : AnyQuantity<T>
    {

        public Time() : base(1) { }

        public Time(float dimension) : base(dimension) { }

        private static QuantityDimension _Dimension = new QuantityDimension(0, 0, 1);
        public override QuantityDimension Dimension
        {
            get
            {
                return  _Dimension * Exponent;
            }
        }


        public static implicit operator Time<T>(T value)
        {
            Time<T> Q = new Time<T>
            {
                Value = value
            };

            return Q;
        }
    }
}
