﻿namespace QuantitySystem.Quantities.BaseQuantities
{
    public class LuminousIntensity<T> : AnyQuantity<T>
    {
        public LuminousIntensity() : base(1) { }

        public LuminousIntensity(float exponent) : base(exponent) { }

        private static QuantityDimension _Dimension = new(0, 0, 0, 0, 0, 0, 1);
        public override QuantityDimension Dimension
        {
            get
            {
                return  _Dimension * Exponent;
            }
        }


        public static implicit operator LuminousIntensity<T>(T value)
        {
            LuminousIntensity<T> Q = new()
            {
                Value = value
            };

            return Q;
        }

    }
}
