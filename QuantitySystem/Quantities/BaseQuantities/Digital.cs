namespace QuantitySystem.Quantities.BaseQuantities;

public class Digital<T>(float exponent) : AnyQuantity<T>(exponent)
{
    public Digital() : this(1) { }

    private static QuantityDimension _Dimension = new()
    {
        Digital = new DimensionDescriptors.DigitalDescriptor(1)
    };

    public override QuantityDimension Dimension => _Dimension * Exponent;

    public static implicit operator Digital<T>(T value)
    {
        Digital<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}