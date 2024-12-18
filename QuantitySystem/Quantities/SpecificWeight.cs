﻿namespace QuantitySystem.Quantities;

public class SpecificWeight<T> : DerivedQuantity<T>
{
    public SpecificWeight()
        : base(1, new Force<T>(), new Volume<T>(-1))
    {
    }

    public SpecificWeight(float exponent)
        : base(exponent, new Force<T>(exponent), new Volume<T>(-1 * exponent))
    {
    }


    public static implicit operator SpecificWeight<T>(T value)
    {
        SpecificWeight<T> Q = new()
        {
            Value = value
        };

        return Q;
    }


}