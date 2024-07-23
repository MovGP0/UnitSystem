namespace QuantitySystem.Quantities.BaseQuantities;

public class AmountOfSubstance<T> : AnyQuantity<T>
{
    public AmountOfSubstance() : base(1) { }

    public AmountOfSubstance(float exponent) : base(exponent) { }

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