namespace QuantitySystem.Quantities.BaseQuantities;

public class ElectricalCurrent<T>(float exponent) : AnyQuantity<T>(exponent)
{
    public ElectricalCurrent() : this(1) { }

    private static QuantityDimension _Dimension = new(0, 0, 0, 0, 1, 0, 0);
    public override QuantityDimension Dimension => _Dimension * Exponent;


    public static implicit operator ElectricalCurrent<T>(T value)
    {
        ElectricalCurrent<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}