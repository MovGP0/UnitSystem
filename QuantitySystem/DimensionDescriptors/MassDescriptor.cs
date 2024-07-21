namespace QuantitySystem.DimensionDescriptors;

public struct MassDescriptor :
    IDimensionDescriptor<MassDescriptor>,
    IEquatable<MassDescriptor>,
    IComparable<MassDescriptor>, IComparable
{
    public MassDescriptor(float exponent):this()
    {
        Exponent = exponent;
    }

    public float Exponent { get; set; }

    public MassDescriptor Add(MassDescriptor dimensionDescriptor)
    {
        return new ()
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    public MassDescriptor Subtract(MassDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public MassDescriptor Multiply(float exponent)
    {
        return new()
        {
            Exponent = Exponent * exponent
        };
    }

    public MassDescriptor Invert()
    {
        return new()
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(MassDescriptor other)
        => Exponent.Equals(other.Exponent);

    public override bool Equals(object? obj)
        => obj is MassDescriptor other && Equals(other);

    public override int GetHashCode()
        => Exponent.GetHashCode();

    public static bool operator ==(MassDescriptor left, MassDescriptor right)
        => left.Equals(right);

    public static bool operator !=(MassDescriptor left, MassDescriptor right)
        => !left.Equals(right);

    public int CompareTo(MassDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is MassDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(MassDescriptor)}");
    }

    public static bool operator <(MassDescriptor left, MassDescriptor right)
        => left.CompareTo(right) < 0;

    public static bool operator >(MassDescriptor left, MassDescriptor right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(MassDescriptor left, MassDescriptor right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(MassDescriptor left, MassDescriptor right)
        => left.CompareTo(right) >= 0;
}