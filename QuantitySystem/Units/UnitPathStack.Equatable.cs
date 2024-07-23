namespace QuantitySystem.Units;

public partial class UnitPathStack : IEquatable<UnitPathStack>
{
    public bool Equals(UnitPathStack? up)
    {
        if (up == null) return false;

        if (up.Count != Count) return false;

        for (var ix = 0; ix < Count; ix++)
        {
            if (!this.ElementAt(ix).Equals(up.ElementAt(ix)))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
        => obj is UnitPathStack ups && Equals(ups);

    public override int GetHashCode()
    {
        HashCode hashCode = new();

        for (var ix = 0; ix < Count; ix++)
        {
            var element = this.ElementAt(ix);
            hashCode.Add(element);
        }

        return hashCode.ToHashCode();
    }

    public static bool operator ==(UnitPathStack? left, UnitPathStack? right) => Equals(left, right);

    public static bool operator !=(UnitPathStack? left, UnitPathStack? right) => !Equals(left, right);
}