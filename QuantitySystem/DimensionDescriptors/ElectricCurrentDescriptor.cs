namespace QuantitySystem.DimensionDescriptors;

public struct ElectricCurrentDescriptor :
    IDimensionDescriptor<ElectricCurrentDescriptor>,
    IEquatable<ElectricCurrentDescriptor>,
    IComparable<ElectricCurrentDescriptor>,
    IComparable
{
    public ElectricCurrentDescriptor(float exponent):this()
    {
        Exponent = exponent;
    }

    public float Exponent { [Pure] get; set; }

    public ElectricCurrentDescriptor Add(ElectricCurrentDescriptor dimensionDescriptor)
    {
        return new ElectricCurrentDescriptor
        {
            Exponent = Exponent+ dimensionDescriptor.Exponent
        };
    }

    public ElectricCurrentDescriptor Subtract(ElectricCurrentDescriptor dimensionDescriptor)
    {
        return new ElectricCurrentDescriptor
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public ElectricCurrentDescriptor Multiply(float exponent)
    {
        return new ElectricCurrentDescriptor
        {
            Exponent = Exponent * exponent
        };
    }

    public ElectricCurrentDescriptor Invert()
    {
        return new ElectricCurrentDescriptor
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(ElectricCurrentDescriptor other)
        => Exponent.Equals(other.Exponent);

    public override bool Equals(object? obj)
        => obj is ElectricCurrentDescriptor other && Equals(other);

    public override int GetHashCode()
        => Exponent.GetHashCode();

    public static bool operator ==(ElectricCurrentDescriptor left, ElectricCurrentDescriptor right)
        => left.Equals(right);

    public static bool operator !=(ElectricCurrentDescriptor left, ElectricCurrentDescriptor right)
        => !left.Equals(right);

    public int CompareTo(ElectricCurrentDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is ElectricCurrentDescriptor other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ElectricCurrentDescriptor)}");
    }

    public static bool operator <(ElectricCurrentDescriptor left, ElectricCurrentDescriptor right)
        => left.CompareTo(right) < 0;

    public static bool operator >(ElectricCurrentDescriptor left, ElectricCurrentDescriptor right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(ElectricCurrentDescriptor left, ElectricCurrentDescriptor right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(ElectricCurrentDescriptor left, ElectricCurrentDescriptor right)
        => left.CompareTo(right) >= 0;
}