﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class AreaMomentOfInertia<T> : DerivedQuantity<T>
{

    public AreaMomentOfInertia()
        : base(1, new Length<T>(2, TensorRank.Scalar), new Length<T>(2, TensorRank.Vector))
    {
    }

    public AreaMomentOfInertia(float exponent)
        : base(exponent, new Length<T>(2 * exponent, TensorRank.Scalar), new Length<T>(2 * exponent, TensorRank.Vector))
    {
    }


    public static implicit operator AreaMomentOfInertia<T>(T value)
    {
        AreaMomentOfInertia<T> Q = new()
        {
            Value = value
        };

        return Q;
    }


}