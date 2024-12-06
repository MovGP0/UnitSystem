namespace QuantitySystem.Quantities.BaseQuantities;

public class AmountOfSubstance<T>(float exponent) : AnyQuantity<T>(exponent)
{
    public AmountOfSubstance() : this(1) { }

    private static readonly QuantityDimension dimension = new(0, 0, 0, 0, 0, 1, 0);

    public override QuantityDimension Dimension => dimension * Exponent;

    public static implicit operator AmountOfSubstance<T>(T value)
    {
        return new AmountOfSubstance<T>
        {
            Value = value
        };
    }
}