using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities;

public class DimensionlessQuantity<T>(float exponent, params AnyQuantity<T>[] internalQuantities)
    : AnyQuantity<T>(exponent)
{

    public DimensionlessQuantity()
        : this(1, null)
    {

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
        return internalQuantities;
    }



    public static implicit operator DimensionlessQuantity<T>(T value)
    {
        DimensionlessQuantity<T> Q = new()
        {
            Value = value
        };

        return Q;


    }






}