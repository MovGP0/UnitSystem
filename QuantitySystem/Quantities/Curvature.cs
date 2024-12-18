﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class Curvature<T> : DerivedQuantity<T>
{
    public Curvature()
        : base(1, new Displacement<T>(-1))
    {
    }

    public Curvature(float exponent)
        : base(exponent, new Displacement<T>(-1 * exponent))
    {
    }


    public static implicit operator Curvature<T>(T value)
    {
        Curvature<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}