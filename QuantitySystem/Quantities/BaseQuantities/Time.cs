namespace QuantitySystem.Quantities.BaseQuantities;

public class Time<T>(float dimension) : AnyQuantity<T>(dimension)
{

    public Time() : this(1) { }

    private static QuantityDimension _Dimension = new(0, 0, 1);
    public override QuantityDimension Dimension
    {
        get
        {
            return  _Dimension * Exponent;
        }
    }


    public static implicit operator Time<T>(T value)
    {
        Time<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}