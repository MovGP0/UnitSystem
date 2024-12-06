namespace QuantitySystem.DimensionDescriptors;

public struct TemperatureDescriptor(float exponent) :
    IDimensionDescriptor<TemperatureDescriptor>,
    IEquatable<TemperatureDescriptor>
{
    public float Exponent { get; set; } = exponent;

    public TemperatureDescriptor Add(TemperatureDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent + dimensionDescriptor.Exponent
        };
    }

    public TemperatureDescriptor Subtract(TemperatureDescriptor dimensionDescriptor)
    {
        return new()
        {
            Exponent = Exponent - dimensionDescriptor.Exponent
        };
    }

    public TemperatureDescriptor Multiply(float exponent)
    {
        return new()
        {
            Exponent = Exponent * exponent
        };
    }

    public TemperatureDescriptor Invert()
    {
        return new()
        {
            Exponent = 0 - Exponent
        };
    }

    public bool Equals(TemperatureDescriptor other)
        => Exponent.Equals(other.Exponent);

    public override bool Equals(object? obj)
        => obj is TemperatureDescriptor other && Equals(other);

    public override int GetHashCode()
        => Exponent.GetHashCode();

    public static bool operator ==(TemperatureDescriptor left, TemperatureDescriptor right)
        => left.Equals(right);

    public static bool operator !=(TemperatureDescriptor left, TemperatureDescriptor right)
        => !left.Equals(right);
}