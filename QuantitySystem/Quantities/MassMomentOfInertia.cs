﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class MassMomentOfInertia<T> : DerivedQuantity<T>
{
    public MassMomentOfInertia()
        : base(1, new Mass<T>(), new Displacement<T>(2))
    {
    }

    public MassMomentOfInertia(float exponent)
        : base(exponent, new Mass<T>(exponent), new Displacement<T>(2 * exponent))
    {
    }


    public static implicit operator MassMomentOfInertia<T>(T value)
    {
        MassMomentOfInertia<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}