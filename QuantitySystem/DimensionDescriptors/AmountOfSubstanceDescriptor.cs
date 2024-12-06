namespace QuantitySystem.DimensionDescriptors;

public struct AmountOfSubstanceDescriptor(float exponent) :
    IDimensionDescriptor<AmountOfSubstanceDescriptor>,
    IEquatable<AmountOfSubstanceDescriptor>,
    IComparable<AmountOfSubstanceDescriptor>,
    IComparable
{
    public float Exponent { get; set; } = exponent;

    public AmountOfSubstanceDescriptor Add(AmountOfSubstanceDescriptor dimensionDescriptor)
    {
        return new AmountOfSubstanceDescriptor
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    public AmountOfSubstanceDescriptor Subtract(AmountOfSubstanceDescriptor dimensionDescriptor)
    {
        return new AmountOfSubstanceDescriptor
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public AmountOfSubstanceDescriptor Multiply(float exponent)
    {
        return new AmountOfSubstanceDescriptor
        {
            Exponent = Exponent * exponent
        };
    }

    public AmountOfSubstanceDescriptor Invert()
    {
        return new AmountOfSubstanceDescriptor
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(AmountOfSubstanceDescriptor other)
        => Exponent.Equals(other.Exponent);

    public override bool Equals(object? obj)
        => obj is AmountOfSubstanceDescriptor other && Equals(other);

    public override int GetHashCode()
        => Exponent.GetHashCode();

    public static bool operator ==(AmountOfSubstanceDescriptor left, AmountOfSubstanceDescriptor right)
        => left.Equals(right);

    public static bool operator !=(AmountOfSubstanceDescriptor left, AmountOfSubstanceDescriptor right)
        => !left.Equals(right);

    public int CompareTo(AmountOfSubstanceDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is AmountOfSubstanceDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(AmountOfSubstanceDescriptor)}");
    }

    public static bool operator <(AmountOfSubstanceDescriptor left, AmountOfSubstanceDescriptor right) => left.CompareTo(right) < 0;

    public static bool operator >(AmountOfSubstanceDescriptor left, AmountOfSubstanceDescriptor right) => left.CompareTo(right) > 0;

    public static bool operator <=(AmountOfSubstanceDescriptor left, AmountOfSubstanceDescriptor right) => left.CompareTo(right) <= 0;

    public static bool operator >=(AmountOfSubstanceDescriptor left, AmountOfSubstanceDescriptor right) => left.CompareTo(right) >= 0;
}