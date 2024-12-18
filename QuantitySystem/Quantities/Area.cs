﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class Area<T> : DerivedQuantity<T>
{
    public Area()
        : base(1, new Length<T>(2))
    {
    }

    public Area(float exponent)
        : base(exponent, new Length<T>(2 * exponent))
    {
    }


    public static implicit operator Area<T>(T value)
    {
        Area<T> Q = new()
        {
            Value = value
        };

        return Q;
    }


}