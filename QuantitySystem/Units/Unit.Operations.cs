using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units;

public partial class Unit
{
    /// <summary>
    /// Creat units path from the current unit instance to the default unit of the current
    /// unit system in the current quantity dimension.
    /// if the unit in the current system have no default unit and direct reference to SI
    /// then the path is stopped on the unit itself and shouldn't bypass it.
    /// </summary>
    /// <returns></returns>
    public UnitPathStack PathToDefaultUnit()
    {
        //from this unit get my path to the default unit.
        var path = new UnitPathStack();

        if (ReferenceUnit is not null) //check that first parent exist.
        {
            var refUnit = this;
            double refTimesNum = 1;
            double refTimesDen = 1;

            //double RefShift = 0.0;

            // do the iteration until we reach the default unit.
            while (refUnit!.IsDefaultUnit == false)
            {
                path.Push(
                    new UnitPathItem
                    {
                        Unit = refUnit,
                        Numerator = refTimesNum,
                        Denominator = refTimesDen,
                        //Shift = RefShift
                    }
                );

                refTimesNum = refUnit.ReferenceUnitNumerator;  //get the value before changing the RefUnit
                refTimesDen = refUnit.ReferenceUnitDenominator;
                //RefShift = RefUnit.ReferenceUnitShift;

                refUnit = refUnit.ReferenceUnit;

                // check the reference unit system or (namespace) if different throw exception.
                //  the exception prevent crossing the system boundary.
                //if (RefUnit.GetType().Namespace != this.GetType().Namespace) throw new UnitException("Unit system access violation");
            }

            // because of while there is another information should be put on the stack.
            path.Push(
                new UnitPathItem
                {
                    Unit = refUnit,
                    Numerator = refTimesNum,
                    Denominator = refTimesDen,
                    //Shift = RefShift
                }
            );
        }
        else
        {
            // no referenceUnit so this is SI unit because all my units ends with SI
            // and it is default unit because all si units have default units with the default prefix.
            if (QuantityType != typeof(DimensionlessQuantity<>))
            {
                path.Push(
                    new UnitPathItem
                    {
                        Unit = this,
                        Numerator = 1,
                        Denominator = 1,
                        //Shift = 0.0
                    }
                );
            }
        }

        return path;
    }


    /// <summary>
    /// Create units path from default unit in the dimension of the current unit system to the running unit instance.
    /// </summary>
    /// <returns></returns>
    public UnitPathStack PathFromDefaultUnit()
    {
        var forward = PathToDefaultUnit();
        var backward = new UnitPathStack();

        while (forward.Count > 0)
        {
            var upi = forward.Pop();

            if (upi.Unit.IsDefaultUnit)
            {
                upi.Numerator = 1;
                upi.Denominator = 1;
                //upi.Shift = 0;
            }
            else
            {
                upi.Numerator = upi.Unit.ReferenceUnitDenominator;  //invert the number
                upi.Denominator = upi.Unit.ReferenceUnitNumerator;
                //upi.Shift = 0 - upi.Unit.ReferenceUnitShift;
            }

            backward.Push(upi);
        }

        return backward;
    }

    /// <summary>
    /// String key to be used as hash between two units conversions
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public string UnitToUnitSymbol(Unit x, Unit y)
    {
        var key =  "[" + x.Symbol + ":" + x.UnitDimension.ToString() + "]" + "__" + "[" + y.Symbol + ":" + y.UnitDimension + "]";

        if(x is DynamicUnit && y is DynamicUnit)
        {
            key = "DYNAMIC__" + key;
        }

        return key;
    }

    private static readonly Dictionary<string, UnitPathStack> CachedPaths = new();

    public static event EventHandler? CacheCleared;

    public static void ClearDynamicUnitsCaching()
    {
        var dynamicKeys = CachedPaths.Keys.Where(k => k.StartsWith("DYNAMIC__")).ToArray();

        lock (CachedPaths)
        {
            foreach (var dynamicKey in dynamicKeys)
            {
                CachedPaths.Remove(dynamicKey);
            }

        }

        var dynamicTypes = CachedUnitsValues.Keys.Where(k=> k.BaseType == typeof(DynamicUnit)).ToArray();
        lock (CachedUnitsValues)
        {
            foreach(var  dynamicType in dynamicTypes)
            {
                CachedUnitsValues.Remove(dynamicType);
            }

        }

        CacheCleared?.Invoke(null, EventArgs.Empty);
    }

    public static void ClearUnitsCaching()
    {
        lock (CachedPaths) CachedPaths.Clear();
        lock (CachedUnitsValues) CachedUnitsValues.Clear();
        CacheCleared?.Invoke(null, EventArgs.Empty);
    }

    private static bool _EnableUnitsCaching = true;

    public static bool EnableUnitsCaching
    {
        get => _EnableUnitsCaching;
        set
        {
            _EnableUnitsCaching = value;
            ClearUnitsCaching();
        }
    }

    /// <summary>
    /// Gets the path to the unit starting from current unit.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public UnitPathStack PathToUnit(Unit unit)
    {
        lock (CachedPaths)
        {
            #region Caching
            //because this method can be a lengthy method we try to check for cached pathes first.
            UnitPathStack cachedPath;
            if (EnableUnitsCaching)
            {
                if (CachedPaths.TryGetValue(UnitToUnitSymbol(this, unit), out cachedPath))
                {
                    return (UnitPathStack)cachedPath.Clone();   //<--- Clone

                    //Why CLONE :D ??  because the unit path is a stack and I use Pop all the time
                    // during the application, and there were hidden error that poping from unit path in the
                    // cached store will not get them back again ;)
                    //  I MUST return cloned copy of the UnitPath.

                }
            }
            #endregion

            #region Validity of Conversion

            if (!UnitDimension.IsDimensionless || !unit.UnitDimension.IsDimensionless)
            {
                //why I've tested dimensioless in begining??
                //   because I want special dimensionless quantities like angle and solid angle to be treated
                //   as normal dimensionless values

                if (UnitDimension.Equals(unit.UnitDimension) == false)
                {
                    throw new UnitsNotDimensionallyEqualException();
                }
            }

            #endregion

            // Extra note after all this years  .. if the coming unit is inverted which mean exponent = -1
            //   the units path  elements are also inverted like the source unit.

            //test if one of the units are not strongly typed
            //  because this needs special treatment. ;)
            if (IsStronglyTyped == false || unit.IsStronglyTyped == false)
            {
                #region Complex units

                //the unit is not strongly typed so we need to make conversion to get its conversion
                // Source unit ==> SI Base Units
                // target unit ==> SI BaseUnits

                var sourcePath = PathToSIBaseUnits();
                var targetPath = unit.PathToSIBaseUnits();

                var tito = new UnitPathStack();

                while (sourcePath.Count > 0)
                {
                    tito.Push(sourcePath.Pop());
                }

                //we have to invert the target
                while (targetPath.Count > 0)
                {
                    var upi = targetPath.Pop();
                    upi.Invert();
                    tito.Push(upi);
                }

                //first location in cache look below for the second location.
                CachedPaths[UnitToUnitSymbol(this, unit)] = (UnitPathStack)tito.Clone();
                return tito;

                #endregion
            }

            // 1- Get Path default unit to current unit.

            var fromMeToDefaultUnit = PathToDefaultUnit();

            // 2- Get Path From Default unit to the passed unit.

            var fromDefaultUnitToTargetUnit = unit.PathFromDefaultUnit();

            // 3- check if the two units are in the same unit system
            //  if the units share the same parent don't jump

            UnitPathStack? systemsPath = null;

            var noBoundaryCross = false;

            if (UnitSystem == unit.UnitSystem)
            {
                noBoundaryCross = true;
            }
            else
            {
                //test for that units parents are the same

                var thisParent = UnitSystem.IndexOf('.') > -1 ?
                    UnitSystem[..UnitSystem.IndexOf('.')] :
                    UnitSystem;

                var targetParent = unit.UnitSystem.IndexOf('.') > -1 ?
                    unit.UnitSystem[..unit.UnitSystem.IndexOf('.')] :
                    unit.UnitSystem;

                if (thisParent == targetParent)
                {
                    noBoundaryCross = true;
                }
            }

            if (noBoundaryCross)
            {
                //no boundary cross should occur

                //if the two units starts with Metric then no need to cross boundaries because
                //they have common references in metric.
            }
            else
            {

                //then we must go out side the current unit system
                //all default units are pointing to the SIUnit system this is a must and not option.

                //get the default unit of target


                // to cross the boundary
                // we should know the non SI system that we will cross it
                // we have two options

                // 1- FromMeToDefaultUnit if (Me unit is another system (not SI)
                //     in this case we will take the top unit to get its reference
                // 2- FromDefaultUnitToTargetUnit (where default unit is not SI)
                //     and in this case we will take the last bottom unit of stack and get its reference

                systemsPath = new UnitPathStack();

                UnitPathItem defaultPItem;
                UnitPathItem refUpi;

                var sourceDefaultUnit = fromMeToDefaultUnit.Peek().Unit;

                if (sourceDefaultUnit.UnitSystem != "Metric.SI" && sourceDefaultUnit.GetType() != typeof(Shared.Second))
                {
                    //from source default unit to the si
                    defaultPItem = fromMeToDefaultUnit.Peek();
                    refUpi = new UnitPathItem
                    {
                        Numerator = defaultPItem.Unit.ReferenceUnitNumerator,
                        Denominator = defaultPItem.Unit.ReferenceUnitDenominator,
                        //Shift = DefaultPItem.Unit.ReferenceUnitShift,
                        Unit = defaultPItem.Unit.ReferenceUnit
                    };
                }
                else
                {
                    // from target default unit to si
                    defaultPItem = fromDefaultUnitToTargetUnit.ElementAt(fromDefaultUnitToTargetUnit.Count - 1);
                    refUpi = new UnitPathItem
                    {
                        //note the difference here
                        //I made the opposite assignments because we are in reverse manner

                        Numerator = defaultPItem.Unit.ReferenceUnitDenominator, // <=== opposite
                        Denominator = defaultPItem.Unit.ReferenceUnitNumerator, // <===
                        //Shift = 0-DefaultPItem.Unit.ReferenceUnitShift,
                        Unit = defaultPItem.Unit.ReferenceUnit
                    };
                }

                if (refUpi.Unit != null)
                {
                    systemsPath.Push(refUpi);
                }

                // NOTE: if refUpi.Unit is null:
                // both default units were SI units without references
                // when define units in unit cloud for quantity
                // either make all units reference SI units without default unit
                // or make one default unit and make the rest of units reference it.
            }

            //combine the two paths
            var total = new UnitPathStack();

            // we are building the conversion stairs
            // will end like a stack

            //begin from me unit to default unit
            for (var i = fromMeToDefaultUnit.Count - 1; i >= 0; i--)
            {
                total.Push(fromMeToDefaultUnit.ElementAt(i));
            }

            var one = new Unit(typeof(DimensionlessQuantity<>));

            //cross the system if we need to .
            if (systemsPath != null)
            {
                total.Push(new UnitPathItem { Denominator = 1, Numerator = 1, Unit = one });
                for (var i = systemsPath.Count - 1; i >= 0; i--)
                {
                    total.Push(systemsPath.ElementAt(i));
                }
                total.Push(new UnitPathItem { Denominator = 1, Numerator = 1, Unit = one });
            }

            // from default unit to target unit
            for (var i = fromDefaultUnitToTargetUnit.Count - 1; i >= 0; i--)
            {
                total.Push(fromDefaultUnitToTargetUnit.ElementAt(i));
            }

            //another check if the units are inverted then
            // go through all items in path and invert it also

            if (IsInverted && unit.IsInverted)
            {
                foreach (var upi in total)
                {
                    // only invert the item if it is already not inverted
                    if(!upi.IsInverted) upi.Invert();
                }
            }

            //Second location in cache  look above for the first one in the same function here :D
            CachedPaths[UnitToUnitSymbol(this, unit)] = total.Clone();
            return total;
        }
    }

    public UnitPathStack PathToSIBaseUnits()
    {
        if (IsStronglyTyped)
        {
            //get the corresponding unit in the SI System
            var innerUnitType = GetDefaultSIUnitTypeOf(QuantityType);

            if (innerUnitType == null && QuantityType == typeof(Displacement<>))
            {
                innerUnitType = GetDefaultSIUnitTypeOf(typeof(Length<>));
            }

            if (innerUnitType == null)
            {
                //some quantities don't have strongly typed si units

                //like knot unit there are no corresponding velocity unit in SI
                //  we need to replace the knot unit with mixed unit to be able to do the conversion

                // first we should reach default unit
                var path = PathToDefaultUnit();

                //then test the system of the current unit if it was other than Metric.SI
                //    then we must jump to SI otherwise we are already in default SI
                if (UnitSystem == "Metric.SI" && UnitExponent == 1f)
                {
                    //because no unit in SI with exponent = 1 don't have direct unit type
                    throw new NotImplementedException("Impossible reach by logic");
                }

                // We should cross the system boundary

                var defaultPItem = path.Peek();
                if (defaultPItem.Unit.ReferenceUnit is not null)
                {
                    var refUpi = new UnitPathItem
                    {
                        Numerator = defaultPItem.Unit.ReferenceUnitNumerator,
                        Denominator = defaultPItem.Unit.ReferenceUnitDenominator,
                        //Shift = DefaultPItem.Unit.ReferenceUnitShift,
                        Unit = defaultPItem.Unit.ReferenceUnit
                    };

                    path.Push(refUpi);
                }
                return path;
            }

            var siUnit = (Unit)Activator.CreateInstance(innerUnitType)!;
            siUnit.UnitExponent = UnitExponent;
            siUnit.UnitDimension = UnitDimension;

            var up = PathToUnit(siUnit);

            if (!siUnit.IsBaseUnit)
            {
                if (siUnit.UnitDimension.IsDimensionless && siUnit.IsStronglyTyped)
                {
                    //for dimensionless units like radian, stradian
                    //do nothing.
                }
                else
                {
                    //expand the unit
                    var expandedUnit = ExpandMetricUnit((MetricUnit)siUnit);
                    var expath = expandedUnit.PathToSIBaseUnits();

                    while (expath.Count > 0)
                    {
                        up.Push(expath.Pop());
                    }
                }
            }

            return up;
        }

        var paths = new UnitPathStack();
        foreach (var un in SubUnits)
        {
            var up = un.PathToSIBaseUnits();

            while (up.Count > 0)
            {
                var upi = up.Pop();

                if (un.IsInverted)
                {
                    // only invert the item if it is already not inverted.
                    if (!upi.IsInverted) upi.Invert();
                }

                paths.Push(upi);
            }
        }

        return paths;
    }
}