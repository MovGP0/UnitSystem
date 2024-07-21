namespace QuantitySystem.DimensionDescriptors;

public struct LuminousIntensityDescriptor :
    IDimensionDescriptor<LuminousIntensityDescriptor>,
    IEquatable<LuminousIntensityDescriptor>,
    IComparable<LuminousIntensityDescriptor>, IComparable
{
    public LuminousIntensityDescriptor(float exponent):this()
    {
        Exponent = exponent;
    }

    public float Exponent { get; set; }

    public LuminousIntensityDescriptor Add(LuminousIntensityDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    public LuminousIntensityDescriptor Subtract(LuminousIntensityDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public LuminousIntensityDescriptor Multiply(float exponent)
    {
        return new()
        {
            Exponent = Exponent * exponent
        };
    }

    public LuminousIntensityDescriptor Invert()
    {
        return new()
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(LuminousIntensityDescriptor other)
    {
        return Exponent.Equals(other.Exponent);
    }

    public override bool Equals(object? obj)
    {
        return obj is LuminousIntensityDescriptor other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Exponent.GetHashCode();
    }

    public static bool operator ==(LuminousIntensityDescriptor left, LuminousIntensityDescriptor right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LuminousIntensityDescriptor left, LuminousIntensityDescriptor right)
    {
        return !left.Equals(right);
    }

    public int CompareTo(LuminousIntensityDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return 1;
        }

        return obj is LuminousIntensityDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(LuminousIntensityDescriptor)}");
    }

    public static bool operator <(LuminousIntensityDescriptor left, LuminousIntensityDescriptor right)
        => left.CompareTo(right) < 0;

    public static bool operator >(LuminousIntensityDescriptor left, LuminousIntensityDescriptor right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(LuminousIntensityDescriptor left, LuminousIntensityDescriptor right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(LuminousIntensityDescriptor left, LuminousIntensityDescriptor right)
        => left.CompareTo(right) >= 0;
}