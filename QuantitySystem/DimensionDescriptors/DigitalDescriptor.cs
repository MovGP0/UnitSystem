namespace QuantitySystem.DimensionDescriptors;

public struct DigitalDescriptor(float exponent) :
    IDimensionDescriptor<DigitalDescriptor>,
    IEquatable<DigitalDescriptor>,
    IComparable<DigitalDescriptor>,
    IComparable
{
    public float Exponent { get; set; } = exponent;

    public DigitalDescriptor Add(DigitalDescriptor dimensionDescriptor)
    {
        return new DigitalDescriptor
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    public DigitalDescriptor Subtract(DigitalDescriptor dimensionDescriptor)
    {
        return new DigitalDescriptor
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public DigitalDescriptor Multiply(float exponent)
    {
        return new DigitalDescriptor
        {
            Exponent = Exponent * exponent
        };
    }

    public DigitalDescriptor Invert()
    {
        return new DigitalDescriptor
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(DigitalDescriptor other)
        => Exponent.Equals(other.Exponent);

    public override bool Equals(object? obj)
        => obj is DigitalDescriptor other && Equals(other);

    public override int GetHashCode() => Exponent.GetHashCode();

    public static bool operator ==(DigitalDescriptor left, DigitalDescriptor right)
        => left.Equals(right);

    public static bool operator !=(DigitalDescriptor left, DigitalDescriptor right)
        => !left.Equals(right);

    public int CompareTo(DigitalDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is DigitalDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(DigitalDescriptor)}");
    }

    public static bool operator <(DigitalDescriptor left, DigitalDescriptor right)
        => left.CompareTo(right) < 0;

    public static bool operator >(DigitalDescriptor left, DigitalDescriptor right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(DigitalDescriptor left, DigitalDescriptor right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(DigitalDescriptor left, DigitalDescriptor right)
        => left.CompareTo(right) >= 0;
}