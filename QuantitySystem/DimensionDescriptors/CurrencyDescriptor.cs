namespace QuantitySystem.DimensionDescriptors
{
    public struct CurrencyDescriptor : IDimensionDescriptor<CurrencyDescriptor>
    {

        public CurrencyDescriptor(float exponent):this()
        {
            Exponent = exponent;
        }

        #region IDimensionDescriptor<MoneyDescriptor> Members

        public float Exponent
        {
            get;
            set;
        }

        public CurrencyDescriptor Add(CurrencyDescriptor dimensionDescriptor)
        {
            CurrencyDescriptor desc = new CurrencyDescriptor();
            desc.Exponent = Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public CurrencyDescriptor Subtract(CurrencyDescriptor dimensionDescriptor)
        {
            CurrencyDescriptor desc = new CurrencyDescriptor();
            desc.Exponent = Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public CurrencyDescriptor Multiply(float exponent)
        {
            CurrencyDescriptor desc = new CurrencyDescriptor();
            desc.Exponent = Exponent * exponent;
            return desc;
        }

        public CurrencyDescriptor Invert()
        {
            CurrencyDescriptor l = new CurrencyDescriptor();
            l.Exponent = 0 - Exponent;
            return l;
        }

        #endregion
    }
}
