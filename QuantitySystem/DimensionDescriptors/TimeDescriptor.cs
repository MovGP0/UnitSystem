namespace QuantitySystem.DimensionDescriptors;

public struct TimeDescriptor :
    IDimensionDescriptor<TimeDescriptor>,
    IEquatable<TimeDescriptor>,
    IComparable<TimeDescriptor>,
    IComparable
{
    public TimeDescriptor(float exponent):this()
    {
        Exponent = exponent;
    }

    public float Exponent { [Pure] get; set; }

    [Pure]
    public TimeDescriptor Add(TimeDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    [Pure]
    public TimeDescriptor Subtract(TimeDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    [Pure]
    public TimeDescriptor Multiply(float exponent)
    {
        return new()
        {
            Exponent = Exponent * exponent
        };
    }

    [Pure]
    public TimeDescriptor Invert()
    {
        return new()
        {
            Exponent = 0 - Exponent
        };
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TimeDescriptor other) => Exponent.Equals(other.Exponent);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is TimeDescriptor other && Equals(other);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
        => Exponent.GetHashCode();

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(TimeDescriptor left, TimeDescriptor right)
        => left.Equals(right);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(TimeDescriptor left, TimeDescriptor right)
        => !left.Equals(right);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(TimeDescriptor other)
        => Exponent.CompareTo(other.Exponent);

    [Pure]
    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return 1;
        }

        return obj is TimeDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(TimeDescriptor)}");
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(TimeDescriptor left, TimeDescriptor right)
        => left.CompareTo(right) < 0;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(TimeDescriptor left, TimeDescriptor right)
        => left.CompareTo(right) > 0;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(TimeDescriptor left, TimeDescriptor right)
        => left.CompareTo(right) <= 0;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(TimeDescriptor left, TimeDescriptor right)
        => left.CompareTo(right) >= 0;
}