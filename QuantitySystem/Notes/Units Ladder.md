﻿Current format:

Pressure<double> zz = SIUnit.Mega<Pascal>(800.30);


the units creation is built over three layers of resolution

1) Flat Units  (Base Quantities)
when passing the QuantityType to the constructor
the unit is created based on the passed type
if the type is not having standard symbol 

creation is done using the dimensions of the quantity based on the base units
        { which is analogous to the basic quantities }

So that passing Force will result in <N>

        

2) Quantity Units
The passed argument is instance of the quantity
based on the internal quantities of the quantity the default unit for each quantity
will be used to create the unit

for example Viscosity internal quantities   Pressure * Time
Pressure: Pa
Second: s

then unit is <Pa.s>


