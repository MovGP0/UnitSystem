﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class Volume<T> : DerivedQuantity<T>
{
    public Volume()
        : base(1, new Length<T>(3))
    {
    }

    public Volume(float exponent)
        : base(exponent, new Length<T>(3 * exponent))
    {
    }

    public static implicit operator Volume<T>(T value)
    {
        Volume<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}