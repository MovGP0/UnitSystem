﻿To distribute this application
--------------------
1) on Code Plex
2) on Code Project as an article
3) adding Units support
4) adding PLinq sample to support scientific calculations of CFD projects
-------------------------------------------------------------------------

The dimensions are a very critical subject in the making scientific applications
when you make your variables have a dimension you make sure that the variable will 
never summed to another different dimension

using the new  var    keyword in C# 3.0 and lambada expressions you can build 
a real dimension frame work.

----------

what about cloning create all types that derive from BaseQuantity
then putting its mlt as key and its object in the value
then cloning it whenever possible
but MLT sometimes are equal

-------
let me imagine a sample of use :)
```csharp
Force f = Force.SIUnit.Kilo<Newton>(50);
Length l = Length.SIUnit.Default<meter>(60);

var work = f * l;
```
but here the Newton is really unit associated with force which makes it un-logical to assign it again
the logical could be.
```csharp
Force f = SIUnit.Kilo<Newton>(50); //which return Force Quantity
```
so from unit I can return the Quantity
```csharp
Length l = SIUnit.Default<meter>(10); //notice that Default will be the default
```
let it be interface IUnitSystem and SIUnitSystem implements IUnitSystem
```csharp
Force f = SIUnitSystem.SIUnit.Default<Meter>(100); // so what is Meter
```
Meter is Type which is nested in SIUnitSystem

so let's interpret it this way  {Because I have SIUnitSystem  I don't need SIUnit
Force f = SIUnitSystem.Default<Newton>(100);  //return newton but where is the value store :) ??

so the same concept is applied to other systems
```csharp
Force f = EEUnitSystem.Default<Pound>(100);
```
and for the shared Units between all systems
there is a shared UnitSystem
```csharp
Time = UnitSystem.Default<Second>(100);
```
what about 
```csharp
Force f  = UnitSystem.Default<Newton>(100);
```

this way you can make a quantity that is SIUnit but without adressing SIPrefixe

why I am differentiating between system?
    because every system have something special for it 
    SISystem have prefixes ,... { I don't know others yet }

the current pattern until now
```
IUnit
  | ----------- Unit 
  |              |       
  |              |       
  |--- ISIUnit -->>-- SIUnit
  |              |
  |              |               
  |--- IEEUnit -->>-- EEUnit 
```
```
IUnit
Unit : IUnit

ISIUnit : IUnit
SIUnit : Unit, ISIUnit

IEEUnit : IUnit
EEUnit : Unit, IEEUnit
```
this is good until now
but what if I am dealing with derived unit quantity like velocity which is m/s

I think I have to predict the Unit by
```csharp
ISIUnit siunit = SIUnitSysyem.UnitOf<Velocity>();
```
this will ensure that I have the SI associated unit to the quantity Velocity
```csharp
siunit.CreateThisUnitQuantity();  //to return a quantity with this unit.

{
	// what about [this is future need not now]
	Velocity v = (Velocity)UnitSystem.FormQuantity(100, "m/s");
}
```

converting value of SIUnit Kilo to Mega for example require 
converting value from Kilo to default
then back from default to Mega.

------------------------------------------------------------------
QuantityDimension class
   I think QuantityDimension can make the Unit based on its internal basic exponents
   as shown in the next pseudo-code
   
   1 -  direct mapping of current exponents to strongly typed UnitSystem
        - this will require that the system unit be passed to the forming function
   2 - if direct mapping didn't succeed then the class should predict the untis from dimensions
        another issue here Moment Quantity and Work Quantity have the same Dimension but not 
        the same units
        as far as I know Moment units = <N.m>
        but Work units  = <J>
        
        so the unit creation should be based on
            A) Unit System.
            B) Quantity.
        
        also making the dimension class create the quantity will be faster because it is
          clonning the quantity not creating a new one.
