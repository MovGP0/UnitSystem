namespace QuantitySystem.Units;

public sealed partial class UnitPathItem : IEquatable<UnitPathItem>
{
    public override bool Equals(object? obj)
        => obj is UnitPathItem upi && Equals(upi);

    public override int GetHashCode()
        => HashCode.Combine(Unit, Numerator, Denominator);

    public static bool operator ==(UnitPathItem? left, UnitPathItem? right)
        => Equals(left, right);

    public static bool operator !=(UnitPathItem? left, UnitPathItem? right)
        => !Equals(left, right);

    public bool Equals(UnitPathItem? upi)
    {
        if (upi is null)
        {
            return false;
        }

        return Unit.GetType() == upi.Unit.GetType()
               && Numerator == upi.Numerator
               && Denominator == upi.Denominator;
    }
}