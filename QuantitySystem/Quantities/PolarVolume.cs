﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class PolarVolume<T> : DerivedQuantity<T>
{
    public PolarVolume()
        : base(1, new Displacement<T>(3))
    {
    }

    public PolarVolume(float exponent)
        : base(exponent, new Displacement<T>(3 * exponent))
    {
    }


    public static implicit operator PolarVolume<T>(T value)
    {
        PolarVolume<T> Q = new()
        {
            Value = value
        };

        return Q;
    }
}