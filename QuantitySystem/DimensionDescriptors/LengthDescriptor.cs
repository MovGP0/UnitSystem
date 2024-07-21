namespace QuantitySystem.DimensionDescriptors;

public struct LengthDescriptor :
    IDimensionDescriptor<LengthDescriptor>,
    IEquatable<LengthDescriptor>,
    IComparable<LengthDescriptor>,
    IComparable
{
    public LengthDescriptor(float normalExponent, float polarExponent):this()
    {
        ScalarExponent = normalExponent;
        VectorExponent = polarExponent;
    }

    public LengthDescriptor(float normalExponent, float polarExponent, float matrixExponent):this()
    {
        ScalarExponent = normalExponent;
        VectorExponent = polarExponent;
        MatrixExponent = matrixExponent;
    }

    public float ScalarExponent { get; set; }

    public float VectorExponent { get; set; }

    public float MatrixExponent { get; set; }

    public float Exponent
    {
        get => ScalarExponent + VectorExponent + MatrixExponent;
        set => throw new NotSupportedException();
    }

    public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
    {
        return new LengthDescriptor
        {
            ScalarExponent = ScalarExponent + dimensionDescriptor.ScalarExponent,
            VectorExponent = VectorExponent + dimensionDescriptor.VectorExponent,
            MatrixExponent = MatrixExponent + dimensionDescriptor.MatrixExponent
        };
    }

    public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
    {
        return new LengthDescriptor
        {
            ScalarExponent = ScalarExponent - dimensionDescriptor.ScalarExponent,
            VectorExponent = VectorExponent - dimensionDescriptor.VectorExponent,
            MatrixExponent = MatrixExponent - dimensionDescriptor.MatrixExponent
        };
    }

    public LengthDescriptor Multiply(float exponent)
    {
        return new LengthDescriptor
        {
            ScalarExponent = ScalarExponent * exponent,
            VectorExponent = VectorExponent * exponent,
            MatrixExponent = MatrixExponent * exponent
        };
    }

    public LengthDescriptor Invert()
    {
        return new LengthDescriptor
        {
            ScalarExponent = 0 - ScalarExponent,
            VectorExponent = 0 - VectorExponent,
            MatrixExponent = 0 - MatrixExponent
        };
    }

    public static LengthDescriptor NormalLength(int exponent)
        => new (exponent, 0);

    public static LengthDescriptor RadiusLength(int exponent)
        => new(0, exponent);

    public bool Equals(LengthDescriptor ld)
    {
        return ScalarExponent == ld.ScalarExponent
               && VectorExponent == ld.VectorExponent
               && MatrixExponent == ld.MatrixExponent;
    }

    public override bool Equals(object? obj)
        => obj is LengthDescriptor ld && Equals(ld);

    public override int GetHashCode()
        => HashCode.Combine(ScalarExponent, VectorExponent);

    public static bool operator ==(LengthDescriptor left, LengthDescriptor right)
        => left.Equals(right);

    public static bool operator !=(LengthDescriptor left, LengthDescriptor right)
        => !left.Equals(right);

    public int CompareTo(LengthDescriptor other)
    {
        var scalarExponentComparison = ScalarExponent.CompareTo(other.ScalarExponent);
        if (scalarExponentComparison != 0)
        {
            return scalarExponentComparison;
        }

        var vectorExponentComparison = VectorExponent.CompareTo(other.VectorExponent);
        if (vectorExponentComparison != 0)
        {
            return vectorExponentComparison;
        }

        return MatrixExponent.CompareTo(other.MatrixExponent);
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is LengthDescriptor other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(LengthDescriptor)}");
    }

    public static bool operator <(LengthDescriptor left, LengthDescriptor right)
        => left.CompareTo(right) < 0;

    public static bool operator >(LengthDescriptor left, LengthDescriptor right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(LengthDescriptor left, LengthDescriptor right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(LengthDescriptor left, LengthDescriptor right)
        => left.CompareTo(right) >= 0;
}