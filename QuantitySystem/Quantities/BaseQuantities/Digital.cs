﻿namespace QuantitySystem.Quantities.BaseQuantities;

public class Digital<T> : AnyQuantity<T>
{
    public Digital() : base(1) { }

    public Digital(float exponent) : base(exponent) { }

    private static QuantityDimension _Dimension = new()
    {
        Digital = new DimensionDescriptors.DigitalDescriptor(1)
    };

    public override QuantityDimension Dimension => _Dimension * Exponent;

    public static implicit operator Digital<T>(T value)
    {
        Digital<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}