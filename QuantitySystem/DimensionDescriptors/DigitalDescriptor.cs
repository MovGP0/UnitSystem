namespace QuantitySystem.DimensionDescriptors
{
    public struct DigitalDescriptor : IDimensionDescriptor<DigitalDescriptor>
    {

        public DigitalDescriptor(float exponent)
            : this()
        {
            Exponent = exponent;
        }

        #region IDimensionDescriptor<InformationDescriptor> Members

        public float Exponent
        {
            get;
            set;
        }

        public DigitalDescriptor Add(DigitalDescriptor dimensionDescriptor)
        {
            DigitalDescriptor desc = new DigitalDescriptor();
            desc.Exponent = Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public DigitalDescriptor Subtract(DigitalDescriptor dimensionDescriptor)
        {
            DigitalDescriptor desc = new DigitalDescriptor();
            desc.Exponent = Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public DigitalDescriptor Multiply(float exponent)
        {
            DigitalDescriptor desc = new DigitalDescriptor();
            desc.Exponent = Exponent * exponent;
            return desc;
        }

        public DigitalDescriptor Invert()
        {
            DigitalDescriptor l = new DigitalDescriptor();
            l.Exponent = 0 - Exponent;
            return l;
        }

        #endregion
    }
}
