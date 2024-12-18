﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class Velocity<T> : DerivedQuantity<T>
{
    public Velocity()
        : base(1, new Displacement<T>(), new Time<T>(-1))
    {
    }

    public Velocity(float exponent)
        : base(exponent, new Displacement<T>(exponent), new Time<T>(-1 * exponent))
    {
    }


    public static implicit operator Velocity<T>(T value)
    {
        Velocity<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}