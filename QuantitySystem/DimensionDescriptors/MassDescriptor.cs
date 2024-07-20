namespace QuantitySystem.DimensionDescriptors
{
    public struct MassDescriptor : IDimensionDescriptor<MassDescriptor>
    {

        public MassDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }


        #region IDimensionDescriptor<MassDescriptor> Members
        public float Exponent
        {
            get;
            set;
        }




        public MassDescriptor Add(MassDescriptor dimensionDescriptor)
        {
            MassDescriptor desc = new MassDescriptor();
            desc.Exponent = Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public MassDescriptor Subtract(MassDescriptor dimensionDescriptor)
        {
            MassDescriptor desc = new MassDescriptor();
            desc.Exponent = Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public MassDescriptor Multiply(float exponent)
        {
            MassDescriptor desc = new MassDescriptor();
            desc.Exponent = Exponent * exponent;
            return desc;
        }

        public MassDescriptor Invert()
        {
            MassDescriptor l = new MassDescriptor();
            l.Exponent = 0 - Exponent;

            return l;
        }

        #endregion
    }
}
