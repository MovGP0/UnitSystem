﻿there are two unit converstions 

first: in the same Unit System


second: crossing the unit system



in the first kind:

conversiont occur in two passes:
	Pass 1: unit goes to the default unit of the unit system.
	Pass 2: From Default unit goes to the desired unit.
	
	
in the second kind:

Conversion occur as the same firt kind but an extra step in the middle

the Default Unit from the First System is Transfered to the Default Unit of the Targeted System


Terms:
	Any Default Unit should reference the SIUnit
	
------
Applying this thinking we need following operations:

0) GetUnitSystemDefaultUnit
1) DiscoverDefaultUnitPath
2) DiscoverUnitPath


UnitPath Structure Implemented as a Stack
also the path is able to be converted to ExpressionTree for execution.

