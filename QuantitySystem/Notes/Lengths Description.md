﻿Angle is dimensionless because its two length divided



Lentgh Types

- Normal Length

- Arc Length

- Radius Length


so when defining angle in respect of lengths

Angle = Arc Length / Radius Length    (untis degree or radian)

Arc Exponent - Radius Exponent = 1-1 =0  Dimensionless number but its unit is Degree  :)

units are Degree and Radian


when dealing with Torque we are expressing Center and arm Radius length and force

so

Normal Length:   is a length between two points in straight forward direction

Arc Length : is a length between two points in curvature direction

Radius :  is  a length between center point and point.
----

Torque =    N . m  ==  F  LRadius

energy = N . m ==  F LNormal

Energy = Torque * angle = F LRadius *  LArc / LRad  which when summed together F * 1 + 1-1 then L = 1  which  is correct in expression and could be known from the program.


--------------------------------------------
I made a mistake  when trying to get energy  I discovered that although I have the length but not normal 
but not included in my dimension key value

thats mean I can't predict the energy due to length is not in normal exponent


any quantity that have Length in dimension should expressed in all available lenghts

Velocity  should have three variations.
however unless the length type was specified explicitly the length component should act in the three modes.

but how to implement this behaviour especially that my Quantities is expressing length in its normal state.
and the variations should be automatically generated

and what about defining two new quantities ???   it will be a problem when defining derived quantities as
  I will not know from which length I should derive.
  
how can I make the length component propagate to three 

AnyQuantity[] qs = AnyQuantity.PropagateWithRespectToLengths();

go through all internal quantities recursively and when found length with normal exponent
extract the other two components by cloning the current quantity and replacing the normal exponent with the other exponents

so Velocity will be 3 Velocities.
----

revising the previous concept I found that Arc Length  AL  can be eliminated completely and just 

it will be Normal Length, and Radius Length  (NL, RL)

if Angle is NL/RL   then it will be successfully stored

Torque =  F  RL
Work = Torque * Angle =  F RL  + NL - RL = F NL    and will be predicted correctly from the hash table

Angular Speed = Angle / Time =    NL RL-1 T-1

Torque * Angular Speed = F RL NL -RL -T =  F NL -T   which is correct.







