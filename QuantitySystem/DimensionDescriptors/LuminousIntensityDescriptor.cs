namespace QuantitySystem.DimensionDescriptors
{
    public struct LuminousIntensityDescriptor : IDimensionDescriptor<LuminousIntensityDescriptor>
    {

        public LuminousIntensityDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }


        #region IDimensionDescriptor<LuminousIntensityDescriptor> Members


        public float Exponent
        {
            get;
            set;
        }




        public LuminousIntensityDescriptor Add(LuminousIntensityDescriptor dimensionDescriptor)
        {
            LuminousIntensityDescriptor desc = new LuminousIntensityDescriptor();
            desc.Exponent  = Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public LuminousIntensityDescriptor Subtract(LuminousIntensityDescriptor dimensionDescriptor)
        {
            LuminousIntensityDescriptor desc = new LuminousIntensityDescriptor();
            desc.Exponent = Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public LuminousIntensityDescriptor Multiply(float exponent)
        {
            LuminousIntensityDescriptor desc = new LuminousIntensityDescriptor();
            desc.Exponent = Exponent * exponent;
            return desc;
        }

        public LuminousIntensityDescriptor Invert()
        {
            LuminousIntensityDescriptor l = new LuminousIntensityDescriptor();
            l.Exponent = 0 - Exponent;

            return l;
        }


        #endregion
    }
}
