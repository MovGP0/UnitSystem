﻿Ways of creating Quantities
---------------------------

var f = SIUnitSystem.Default<Newton>(100);   // Force Quantity
var t = UnitSystem.Default<Second>(40);      // Time Quantity

// to create a strongly typed dervied quantity without corresponding units
IUnit vunit = SIUnitSystem.UnitOf<Velocity>();               //derived unit.
Velocity v = (Velocity)vunit.CreateThisUnitQuantity();       //Viscosity with derived unit.
v.Value = 100;

//or directly by
var vs = SIUnitSystem.UnitQuantityOf<Viscosity>(100);  //Viscosity with derived unit.

```
g  * m/s^2 = mN
kg * m/s^2 = N
Mg * m/s^2 = kN

g * km/s^2 = 1g * 1000m/1s^2 = 1000 g.m/s^2 = 1 kg.m/s^2 = 1 N

g * Mm/kS^2 = 1g * 1000000m/1000S^2 = 1000 g.m/s^2 = 1 kg.m/s^2 = 1 N //this is wrong because time have no prefixes but I am assuming
```

**Note:**  DerivedUnit should have base units with their prefixes when making 10 kg * 10 kg result is  100 kg.kg and prefixes will fused into Mega

```
kJ / s = kW
J / s = W
```
```
N * km = N * 1000m = 1000J = 1kJ //note the prefixes addition
```
so if result unit is strongly typed unit we can take the sum of prefixes of the two quantities
to produce a new prefix for the new quantity.

In the general quantity multiplication
the operation don't know the unit 
second unit system will always be converted to the first unit system :: this is a must

So we need to add

```csharp
     IUnit.Multiply(IUnit unit);
```
```
m^3 * m^2 = m^3.m^2 = m^5
```
