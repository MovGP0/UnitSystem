namespace QuantitySystem.DimensionDescriptors;

public struct CurrencyDescriptor :
    IDimensionDescriptor<CurrencyDescriptor>,
    IEquatable<CurrencyDescriptor>,
    IComparable<CurrencyDescriptor>,
    IComparable
{
    public CurrencyDescriptor(float exponent):this()
    {
        Exponent = exponent;
    }

    public float Exponent { get; set; }

    public CurrencyDescriptor Add(CurrencyDescriptor dimensionDescriptor)
    {
        return new CurrencyDescriptor
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    public CurrencyDescriptor Subtract(CurrencyDescriptor dimensionDescriptor)
    {
        return new CurrencyDescriptor
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public CurrencyDescriptor Multiply(float exponent)
    {
        return new CurrencyDescriptor
        {
            Exponent = Exponent * exponent
        };
    }

    public CurrencyDescriptor Invert()
    {
        return new CurrencyDescriptor
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(CurrencyDescriptor other)
        => Exponent.Equals(other.Exponent);

    public override bool Equals(object? obj)
        => obj is CurrencyDescriptor other && Equals(other);

    public override int GetHashCode()
        => Exponent.GetHashCode();

    public static bool operator ==(CurrencyDescriptor left, CurrencyDescriptor right)
        => left.Equals(right);

    public static bool operator !=(CurrencyDescriptor left, CurrencyDescriptor right)
        => !left.Equals(right);

    public int CompareTo(CurrencyDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is CurrencyDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(CurrencyDescriptor)}");
    }

    public static bool operator <(CurrencyDescriptor left, CurrencyDescriptor right) => left.CompareTo(right) < 0;

    public static bool operator >(CurrencyDescriptor left, CurrencyDescriptor right) => left.CompareTo(right) > 0;

    public static bool operator <=(CurrencyDescriptor left, CurrencyDescriptor right) => left.CompareTo(right) <= 0;

    public static bool operator >=(CurrencyDescriptor left, CurrencyDescriptor right) => left.CompareTo(right) >= 0;
}