using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class DimensionlessQuantity<T> : AnyQuantity<T>
    {

        public DimensionlessQuantity()
            : base(1)
        {
            
        }


        private AnyQuantity<T>[] InternalQuantities;

        public DimensionlessQuantity(float exponent, params AnyQuantity<T>[] internalQuantities)
            : base(exponent)
        {
            InternalQuantities = internalQuantities;

            
            
        }

        public override QuantityDimension Dimension
        {
            get
            {
                return QuantityDimension.Dimensionless;
            }
        }




        public AnyQuantity<T>[] GetInternalQuantities()
        {
            return InternalQuantities;
        }



        public static implicit operator DimensionlessQuantity<T>(T value)
        {
            DimensionlessQuantity<T> Q = new DimensionlessQuantity<T>
            {
                Value = value
            };

            return Q;

            
        }


        



    }
}
