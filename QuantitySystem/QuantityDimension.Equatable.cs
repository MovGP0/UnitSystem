namespace QuantitySystem;

public sealed partial class QuantityDimension : IEquatable<QuantityDimension>
{
    public override bool Equals(object? obj) => obj is QuantityDimension qd && Equals(qd);

    /// <summary>
    /// Equality here based on first level of exponent validation.
    /// Which means length explicitly is compared to the total dimension value not on radius and normal length values.
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public bool Equals(QuantityDimension? dimension)
    {
        if (dimension is null)
        {
            return false;
        }

        return ElectricCurrent.Exponent == dimension.ElectricCurrent.Exponent &&
               Length.Exponent == dimension.Length.Exponent &&
               LuminousIntensity.Exponent == dimension.LuminousIntensity.Exponent &&
               Mass.Exponent == dimension.Mass.Exponent &&
               AmountOfSubstance.Exponent == dimension.AmountOfSubstance.Exponent &&
               Temperature.Exponent == dimension.Temperature.Exponent &&
               Time.Exponent == dimension.Time.Exponent &&
               Currency.Exponent == dimension.Currency.Exponent;
    }

    public override int GetHashCode() => ToString().GetHashCode();
}