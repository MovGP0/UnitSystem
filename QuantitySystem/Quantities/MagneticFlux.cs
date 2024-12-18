﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities;

public class MagneticFlux<T> : DerivedQuantity<T>
{
    public MagneticFlux()
        : base(1, new ElectromotiveForce<T>(), new Time<T>())
    {
    }

    public MagneticFlux(float exponent)
        : base(exponent, new ElectromotiveForce<T>(exponent), new Time<T>(exponent))
    {
    }


    public static implicit operator MagneticFlux<T>(T value)
    {
        MagneticFlux<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}