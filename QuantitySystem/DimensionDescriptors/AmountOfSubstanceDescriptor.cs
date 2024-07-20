namespace QuantitySystem.DimensionDescriptors
{
    public struct AmountOfSubstanceDescriptor : IDimensionDescriptor<AmountOfSubstanceDescriptor>
    {

        public AmountOfSubstanceDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }

        #region IDimensionDescriptor<AmountOfSubstanceDescriptor> Members

        public float Exponent
        {
            get;
            set;
        }

        public AmountOfSubstanceDescriptor Add(AmountOfSubstanceDescriptor dimensionDescriptor)
        {
            AmountOfSubstanceDescriptor desc = new AmountOfSubstanceDescriptor();
            desc.Exponent = Exponent + dimensionDescriptor.Exponent;
            return desc;

        }

        public AmountOfSubstanceDescriptor Subtract(AmountOfSubstanceDescriptor dimensionDescriptor)
        {
            AmountOfSubstanceDescriptor desc = new AmountOfSubstanceDescriptor();
            desc.Exponent = Exponent -  dimensionDescriptor.Exponent;
            return desc;
        }

        public AmountOfSubstanceDescriptor Multiply(float exponent)
        {
            AmountOfSubstanceDescriptor desc = new AmountOfSubstanceDescriptor();
            desc.Exponent = Exponent * exponent;
            return desc;
        }

        

        public AmountOfSubstanceDescriptor Invert()
        {
            AmountOfSubstanceDescriptor l = new AmountOfSubstanceDescriptor();
            l.Exponent = 0 - Exponent;
            
            return l;
        }

        #endregion
    }
}
