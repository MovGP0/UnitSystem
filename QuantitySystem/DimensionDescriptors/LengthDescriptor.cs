namespace QuantitySystem.DimensionDescriptors
{
    public struct LengthDescriptor : IDimensionDescriptor<LengthDescriptor>
    {

        public LengthDescriptor(float normalExponent, float polarExponent):this()
        {
            ScalarExponent = normalExponent;
            VectorExponent = polarExponent;
        }

        #region Length Properties Types

        public float ScalarExponent
        {
            get;
            set;
        }

        public float VectorExponent
        {
            get;
            set;
        }

        public float MatrixExponent
        {
            get;
            set;
        }

        #endregion


        public override bool Equals(object obj)
        {
            try
            {
                LengthDescriptor ld = (LengthDescriptor)obj;
                {
                    if (ScalarExponent != ld.ScalarExponent) return false;

                    if (VectorExponent != ld.VectorExponent) return false;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ScalarExponent.GetHashCode() ^ VectorExponent.GetHashCode();
        }

        #region IDimensionDescriptor<LengthDescriptor> Members


        public float Exponent
        {
            get { return ScalarExponent + VectorExponent; }
            set { }
        }



        public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = ScalarExponent + dimensionDescriptor.ScalarExponent;
            l.VectorExponent = VectorExponent + dimensionDescriptor.VectorExponent;

            return l;
        }

        public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = ScalarExponent - dimensionDescriptor.ScalarExponent;
            l.VectorExponent = VectorExponent - dimensionDescriptor.VectorExponent;

            return l;
        }

        public LengthDescriptor Multiply(float exponent)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = ScalarExponent * exponent;
            l.VectorExponent = VectorExponent * exponent;

            return l;
        }

        public LengthDescriptor Invert()
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = 0 - ScalarExponent;
            l.VectorExponent = 0 - VectorExponent;
            return l;
        }


        #endregion


        #region Helper Instantiators
        public static LengthDescriptor NormalLength(int exponent)
        {

            return new LengthDescriptor(exponent, 0);

        }
        public static LengthDescriptor RadiusLength(int exponent)
        {

            return new LengthDescriptor(0, exponent);

        }
        #endregion
    }
}
