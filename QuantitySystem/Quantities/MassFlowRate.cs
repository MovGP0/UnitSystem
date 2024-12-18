﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class MassFlowRate<T> : DerivedQuantity<T>
{
    public MassFlowRate()
        : base(1, new Mass<T>(), new Time<T>(-1))
    {
    }

    public MassFlowRate(float exponent)
        : base(exponent, new Mass<T>(exponent), new Time<T>(-1 * exponent))
    {
    }


    public static implicit operator MassFlowRate<T>(T value)
    {
        MassFlowRate<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}