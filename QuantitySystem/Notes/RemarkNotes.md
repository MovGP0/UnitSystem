Hello
-----

I discovered that I have a distracted thoughts so I am trying to collect my thoughts here sequentially
the file will describe what I will make and what I already made because I didn't keep track of things last months

Ideas:
------

1] DimensionlessQuantity is simply any variable that is have no dimensions
  so that the current valuetypes of c# like int,float, and double are dimensionless
  however in my implementation the you should declare DimensionlessQuantity explicitly

  by using DimensionlessQuantity you can multiply or divide it to any Quantity without dimension information
  being loss

  Tasks:
  - Create instance from DimensionlessQuantity    |  Done
  - Multiplication and Division                   |  need Test

2] Make Console Calculator to evaulate Quantities
  to illustrate the quantites calculations and other facilities to the library

  Tasks:
  - Console with Prompt Qs>
  - Options
	a] Write unit in <> print the unit information and its quantity 
		Qs> <m>

	b] Support Variables Accept variables
		Qs> a=1<m>	  
		Qs> b=1<ft>   

	c] Simple operations 
		+ - / *
		a+b
		c = a+ b

3] 	adding two dimensionless variable made an error 
	reason: dimensionless unit is dervied unit without refernce unit that made the program add the same unit in
			the unit path
	(Fixed):  don't execute the logic when the quantities are dimensional
	
4]  Multiplieng two dimensionless units or dividing them make do logic on the units
	suggestion: 
		- cancel the behavior hard coded
		- make a special unit for the dimensionless unit with <1>
			however this will remove the dynamic creation of unit which may be needed in other calculations
			especially if we need those units again in another calculations.
			
			so let the original behaviour and add exception if that if one of the two units quantity dimensionless.

	(Fixed)
		
5]  Fix the unit formation from multiplication
    current behaviour for density*velocity    	
```
Qs> rho = 1[Density]
  Density`1: 1 <kg/m^3>

Qs> v=1[Viscosity]
  Viscosity`1: 1 <Pa.s>

Qs> rho * v
  DerivedQuantity`1: 1 <<kg/m^3>^2>
```
That was wrong after fixation 
the result was
```
    DerivedQuantity`1: 1 <<kg/m^3>.<Pa.s>>
```
Partially (Fixed)

6]  When summing force of units <kg.<m/s^2>> + <N> 
	error occured because conversion is not done correctly 
	unit <kg.<m/s^2>> can't be converted because the algorithm is searching for the default unit
	   and there is no default unit for the mixed units 
	unit <N> don't have default unit because it self is default unit in SI
	
	how to cure the mixed  unit default unit behaviour
		- loop through all sub units
		- for every sub unit get its default unit.
		   - if sub unit is another mixed unit then repeat the search recursively to get 
		     the default units.
		     
		     question: I am casting to the left units, how can I reach the left units from right units
		     i'll invent two terms
		        CollapseUnit & ExpandUnit
		        
		        ExpandUnit: the process that expand unit into its basic units or more precious the default units
		        
		        CollapseUnit: the process that find the closest strongly typed unit for group of units
		        
		     Another thing: if we know the fact that Dimensions are equal then we can say that expanding units to its 
		                    default units will lead to the same units (in the other system) regardless of order
		                    
		                    then we make re-order to the right units to map the corresponding units in the left unit
								based on their corresponding base quantities.
		                    
		                    then we construct the unitpath between every unit and the corresponding one
								+ will result into unit paths with the same count to the base units discovered
								+ and the conversion factor will be the multiplication of all unit path factors.
		                    
		                    and due to we will cast to the left unit 
		                    
		                    we don't have to preserve information about how we reached the expanded unit
		                    
		                    we only need to come up with the conversion factor
		                    
		                    for example
		                    
		                    <N> + <oz.<m/s^2>>   <= oz:Ounce an imperial unit its default unit is the lbm:Pound
		                    
		                    Expand(<N>) 
								<kg.<m/s^2>>
							
							Expand(<oz.<m/s^2>>)
								<lbm.<m/s^2>>
							
							then mapping lbm <-> kg  and the rest of units
							then come up with the UnitPath object for every two mapped units
								that hold all the information.
								
							
			
						but when expanding how can I preserve the conversion factors to the default units
						I reached ???? 
						(Done)
						
						
						

7] gill to litre conversion was error
	why? because pint which is the default unit of volume in imperial was pointing to litre which wasn't
		default unit in SI and this is wrong because default units should reference default units 
		
		old: pointed to litre
		    [DefaultUnit("pt", typeof(Volume<>))]
			[ReferenceUnit(0.5682, UnitType = typeof(Metric.Litre))]
			public sealed class Pint : Unit
			{

			}
						
		New(fix): pointed to m^3
			[DefaultUnit("pt", typeof(Volume<>))]
			[ReferenceUnit(0.5682, 1000)]
			public sealed class Pint : Unit
			{

			}
							
	(Fixed)
							
		                    
8] Constructor of unit that takes dimension have a slight problem
	if it was discovered that the unit is from base unit then it will return a unit with sub units
	contain only one unit and the unit will be not base

	Proposed solution 
	move the constructor to static creation function.

	(Fixed)

9] Unit object didn't hold the dimension information so conversion to any unit is valid :S
   proposed solution: add dimension information to the unit.    
     error occured because I was instantiating unit from the DimensionLessQuantity 
			hint: NEVER instantiate unit from Quantity because units depending on Quantities
			      and if quantity is never cached then its corresponding unit will never find it

   (Fixed)

10] now we are version 1.0.3 in library
	-summing or subtracting two derived quantities result in exception.
	[Fixed]
	
11] when multiplying or dividing SI units the result combined unit is not aware of the prefix.

	    d1 = 2 <ft/km>
		d2 = 3 <rod/Mm>

		d2/d1  =   3/2 = 1.5 = 0.0015 <rod/ft>

		rod       km
		---   *   ---   difference of expontent will give 10^-3    
		Mm         ft

		[Fixed]

12]
		also what if we multiplied Yotta * Yotta  the result will be overflow 
		and if exponent lie in wrong direction
		[Fixed]   
		     in   1.0.6.2

13] The whole prefix thing have been revised again.		     

14] Functions to be added {added}
	this leads to more integration into DLR  now the the console is based on DLR
