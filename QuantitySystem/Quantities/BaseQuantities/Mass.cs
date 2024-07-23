namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Mass<T> : AnyQuantity<T>
    {
        /// <summary>
        /// Create Mass object with dimension equals 1 M^1.
        /// </summary>
        public Mass() : base(1)
        {

        }

        /// <summary>
        /// Create Mass object with the desired dimension.
        /// </summary>
        /// <param name="exponent">exponent of created Quantity</param>
        public Mass(float exponent) : base(exponent) { }


        private static QuantityDimension _Dimension = new(1, 0, 0);
        public override QuantityDimension Dimension
        {
            get
            {
                return _Dimension * Exponent;
            }
        }


        public static implicit operator Mass<T>(T value)
        {
            Mass<T> Q = new()
            {
                Value = value
            };

            return Q;
        }

    }
}
