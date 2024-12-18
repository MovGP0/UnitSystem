﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class ElectricResistance<T> : DerivedQuantity<T>
{
    public ElectricResistance()
        : base(1, new ElectromotiveForce<T>(), new ElectricalCurrent<T>(-1))
    {
    }

    public ElectricResistance(float exponent)
        : base(exponent, new ElectromotiveForce<T>(exponent), new ElectricalCurrent<T>(-1 * exponent))
    {
    }


    public static implicit operator ElectricResistance<T>(T value)
    {
        ElectricResistance<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}