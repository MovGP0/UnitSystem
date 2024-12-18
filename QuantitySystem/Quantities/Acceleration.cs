﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class Acceleration<T> : DerivedQuantity<T>
{
    public Acceleration()
        : base(1, new Speed<T>(), new Time<T>(-1))
    {
    }

    public Acceleration(float exponent)
        : base(exponent, new Speed<T>(exponent), new Time<T>(-1 * exponent))
    {
    }


    public static implicit operator Acceleration<T>(T value)
    {
        Acceleration<T> Q = new()
        {
            Value = value
        };

        return Q;
    }


}