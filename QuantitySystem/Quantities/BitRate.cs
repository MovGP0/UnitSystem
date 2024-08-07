﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class BitRate<T> : DerivedQuantity<T>
{

    public BitRate()
        : base(1, new Digital<T>(1), new Time<T>(-1))
    {
    }

    public BitRate(float exponent)
        : base(exponent, new Digital<T>(1 * exponent), new Time<T>(-1 * exponent))
    {
    }


    public static implicit operator BitRate<T>(T value)
    {
        BitRate<T> Q = new()
        {
            Value = value
        };

        return Q;
    }


}