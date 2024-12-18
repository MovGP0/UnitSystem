﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class AngularAcceleration<T> : DerivedQuantity<T>
{
    public AngularAcceleration()
        : base(1, new AngularVelocity<T>(), new Time<T>(-1))
    {
    }


    public AngularAcceleration(float exponent)
        : base(exponent, new AngularVelocity<T>(exponent), new Time<T>(-1 * exponent))
    {
    }

    public static implicit operator AngularAcceleration<T>(T value)
    {
        AngularAcceleration<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}