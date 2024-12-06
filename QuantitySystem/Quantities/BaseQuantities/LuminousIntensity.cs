namespace QuantitySystem.Quantities.BaseQuantities;

public class LuminousIntensity<T>(float exponent) : AnyQuantity<T>(exponent)
{
    public LuminousIntensity() : this(1) { }

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