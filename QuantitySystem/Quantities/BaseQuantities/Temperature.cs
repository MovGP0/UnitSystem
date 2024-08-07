﻿namespace QuantitySystem.Quantities.BaseQuantities
{
    public class Temperature<T> : AnyQuantity<T>
    {
        public Temperature() : base(1) { }

        public Temperature(float exponent) : base(exponent) { }

        private static QuantityDimension _Dimension = new(0, 0, 0, 1, 0, 0, 0);
        public override QuantityDimension Dimension
        {
            get
            {
                return _Dimension * Exponent;
            }
        }


        public static implicit operator Temperature<T>(T value)
        {
            Temperature<T> Q = new()
            {
                Value = value
            };

            return Q;
        }
    }
}
