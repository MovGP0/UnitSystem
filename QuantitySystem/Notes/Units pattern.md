﻿untis is linked to the dimension

SI units:
--------
DerivedSIUnit
  creation methods
       from dimension :   every exponent => unit, mass => kg which means Gram with prefix kilo
       2) Derived dimension hold array of base units
       3) base unit could be special unit wich hold base units
       
       from array of units:  this case we already have a set of units which will form a derived unit
       
       from quantity: //I don't want this pattern
       
       var l1 = SIUnitSystem.Kilo<Gram>(100);
       var l2 = EEUnitSystem.Default<Pound>(100);
       
       var l = l1 + l2;  //l units is kilo gram
       var l = l2 + l1;  //l units is Pound
