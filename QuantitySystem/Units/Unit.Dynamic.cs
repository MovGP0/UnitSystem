using System.Globalization;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities;
using System.Text.RegularExpressions;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units;

public partial class Unit
{
    #region Dynamically created unit

    /// <summary>
    /// This list shouldn't been modified by child classes
    /// </summary>
    public List<Unit> SubUnits { get; private set; }

    /// <summary>
    /// Create the unit directly from the specified dimension in its SI base units.
    /// </summary>
    /// <param name="dimension"></param>
    public static Unit DiscoverUnit(QuantityDimension dimension) => DiscoverUnit(dimension, "Metric.SI");

    /// <summary>
    /// Create the unit directly from the specfied dimension based on the unit system given.
    /// </summary>
    /// <param name="dimension"></param>
    public static Unit DiscoverUnit(QuantityDimension dimension, string unitSystem)
    {
        List<Unit> subUnits = [];

        if (dimension.Currency.Exponent != 0)
        {
            Unit u = new Currency.Coin();
            u.UnitExponent = dimension.Currency.Exponent;
            u.UnitDimension = new QuantityDimension { Currency = new DimensionDescriptors.CurrencyDescriptor( dimension.Currency.Exponent )};
            subUnits.Add(u);
        }

        if (dimension.Mass.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(Mass<>), unitSystem);
            var u = (Unit)Activator.CreateInstance(unitType);

            u.UnitExponent = dimension.Mass.Exponent;
            u.UnitDimension = new QuantityDimension(dimension.Mass.Exponent, 0, 0);

            subUnits.Add(u);
        }

        if (dimension.Length.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(Length<>), unitSystem);

            var u = (Unit)Activator.CreateInstance(unitType);

            u.UnitExponent = dimension.Length.Exponent;
            u.UnitDimension = new QuantityDimension() { Length = dimension.Length };

            subUnits.Add(u);
        }

        if (dimension.Time.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(Time<>), unitSystem);
            var u = (Unit)Activator.CreateInstance(unitType)!;

            u.UnitExponent = dimension.Time.Exponent;
            u.UnitDimension = new QuantityDimension() { Time = dimension.Time };

            subUnits.Add(u);
        }

        if (dimension.Temperature.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(Temperature<>), unitSystem);
            var u = (Unit)Activator.CreateInstance(unitType)!;

            u.UnitExponent = dimension.Temperature.Exponent;
            u.UnitDimension = new QuantityDimension() { Temperature = dimension.Temperature };

            subUnits.Add(u);
        }

        if (dimension.LuminousIntensity.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(LuminousIntensity<>), unitSystem);
            var u = (Unit)Activator.CreateInstance(unitType)!;

            u.UnitExponent = dimension.LuminousIntensity.Exponent;
            u.UnitDimension = new QuantityDimension() { LuminousIntensity = dimension.LuminousIntensity };

            subUnits.Add(u);
        }

        if (dimension.AmountOfSubstance.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(AmountOfSubstance<>), unitSystem);
            var u = (Unit)Activator.CreateInstance(unitType)!;

            u.UnitExponent = dimension.AmountOfSubstance.Exponent;
            u.UnitDimension = new QuantityDimension(0, 0, 0, 0, 0, dimension.AmountOfSubstance.Exponent, 0);

            subUnits.Add(u);
        }

        if (dimension.ElectricCurrent.Exponent != 0)
        {
            var unitType = GetDefaultUnitTypeOf(typeof(ElectricalCurrent<>), unitSystem);
            var u = (Unit)Activator.CreateInstance(unitType)!;

            u.UnitExponent = dimension.ElectricCurrent.Exponent;
            u.UnitDimension = new QuantityDimension() { ElectricCurrent = dimension.ElectricCurrent };

            subUnits.Add(u);
        }

        if (dimension.Currency.Exponent != 0)
        {
            Unit u = new Currency.Coin();
            u.UnitExponent = dimension.Currency.Exponent;
            u.UnitDimension *= dimension.Currency.Exponent;

            subUnits.Add(u);
        }

        if (dimension.Digital.Exponent != 0)
        {
            Unit u = new Digital.Bit();
            u.UnitExponent = dimension.Digital.Exponent;
            u.UnitDimension *= dimension.Digital.Exponent;

            subUnits.Add(u);
        }

        Unit? un = null;

        try
        {
            var qType = QuantityDimension.QuantityTypeFrom(dimension);
            un = new Unit(qType, subUnits.ToArray());
        }
        catch (QuantityNotFoundException)
        {
            un = new Unit(null, subUnits.ToArray());

        }

        return un;
    }

    /// <summary>
    /// Construct a unit based on the quantity type in SI Base units.
    /// Any Dimensionless quantity will return  in its unit.
    /// </summary>
    /// <param name="quantityType"></param>
    public Unit(Type quantityType)
    {

            SubUnits = [];

            //try direct mapping first to get the unit

            var InnerUnitType = GetDefaultSIUnitTypeOf(quantityType);

            if (InnerUnitType == null)
            {
                var dimension = QuantityDimension.DimensionFrom(quantityType);


                if (dimension.Mass.Exponent != 0)
                {
                    Unit u = new Metric.SI.Gram();
                    u.UnitExponent = dimension.Mass.Exponent;
                    u.UnitDimension *= dimension.Mass.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Length.Exponent != 0)
                {
                    Unit u = new Metric.SI.Metre();
                    u.UnitExponent = dimension.Length.Exponent;
                    u.UnitDimension *= dimension.Length.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Time.Exponent != 0)
                {
                    Unit u = new Shared.Second();
                    u.UnitExponent = dimension.Time.Exponent;
                    u.UnitDimension *= dimension.Time.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Temperature.Exponent != 0)
                {
                    Unit u = new Metric.SI.Kelvin();
                    u.UnitExponent = dimension.Temperature.Exponent;
                    u.UnitDimension *= dimension.Temperature.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.LuminousIntensity.Exponent != 0)
                {
                    Unit u = new Metric.SI.Candela();
                    u.UnitExponent = dimension.LuminousIntensity.Exponent;
                    u.UnitDimension *= dimension.LuminousIntensity.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.AmountOfSubstance.Exponent != 0)
                {
                    Unit u = new Metric.SI.Mole();
                    u.UnitExponent = dimension.AmountOfSubstance.Exponent;
                    u.UnitDimension *= dimension.AmountOfSubstance.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.ElectricCurrent.Exponent != 0)
                {
                    Unit u = new Metric.SI.Ampere();
                    u.UnitExponent = dimension.ElectricCurrent.Exponent;
                    u.UnitDimension *= dimension.ElectricCurrent.Exponent;

                    SubUnits.Add(u);
                }


                if (dimension.Currency.Exponent != 0)
                {
                    Unit u = new Currency.Coin();
                    u.UnitExponent = dimension.Currency.Exponent;
                    u.UnitDimension *= dimension.Currency.Exponent;

                    SubUnits.Add(u);
                }

                if (dimension.Digital.Exponent != 0)
                {
                    Unit u = new Digital.Bit();
                    u.UnitExponent = dimension.Digital.Exponent;
                    u.UnitDimension *= dimension.Digital.Exponent;

                    SubUnits.Add(u);
                }
            }

            else
            {
                //subclass of AnyQuantity
                //use direct mapping with the exponent of the quantity

                var un = (Unit)Activator.CreateInstance(InnerUnitType);

                SubUnits.Add(un);

            }

            _Symbol = GenerateUnitSymbolFromSubBaseUnits();


            _IsDefaultUnit = true;

            //the quantity may be derived quantity which shouldn't be referenced :check here.
            _QuantityType = quantityType;


            UnitDimension = QuantityDimension.DimensionFrom(_QuantityType);

            _IsBaseUnit = false;

        }


    /// <summary>
    /// Construct a unit based on the default units of the internal quantities of passed quantity instance.
    /// Dimensionless quantity will return their native sub quantities units.
    /// this connstructor is useful like when you pass torque quantity it will return "N.m"
    /// but when you use Energy Quantity it will return J.
    /// </summary>
    /// <param name="quantity"></param>
    public static Unit DiscoverUnit(BaseQuantity quantity)
    {
        var m_QuantityType = quantity.GetType();

        var gen_q = m_QuantityType.GetGenericTypeDefinition() ;

        if (gen_q == typeof(Currency<>)) return new Currency.Coin();
        if (gen_q == typeof(Digital<>)) return new Digital.Bit();

        if (gen_q == typeof(Displacement<>))
        {
            //because all length units associated with the Length<> Type
            m_QuantityType = typeof(Length<>).MakeGenericType(m_QuantityType.GetGenericArguments()[0]);
        }

        if (quantity.Dimension.IsDimensionless)
        {
            var QtyType = m_QuantityType;
            if (!QtyType.IsGenericTypeDefinition)
            {
                QtyType = QtyType.GetGenericTypeDefinition();

            }

            if (QtyType == typeof(DimensionlessQuantity<>))
            {
                return DiscoverUnit(QuantityDimension.Dimensionless);
            }
        }

        List<Unit> subUnits = [];

        //try direct mapping first to get the unit

        var innerUnitType = GetDefaultSIUnitTypeOf(m_QuantityType);

        if (innerUnitType == null) //no direct mapping so get it from the inner quantities
        {
            BaseQuantity[] InternalQuantities;

            // I can't cast BaseQuantity to AnyQuantity<object> very annoying
            // so I used reflection.

            var GIQ = m_QuantityType.GetMethod("GetInternalQuantities");

            // casted the array to BaseQuantity array also
            InternalQuantities = GIQ.Invoke(quantity, null) as BaseQuantity[];

            foreach (var InnerQuantity in InternalQuantities)
            {
                //try to get the quantity direct unit
                var l2_InnerUnitType = GetDefaultSIUnitTypeOf(InnerQuantity.GetType());

                if (l2_InnerUnitType == null)
                {
                    //this means for this quantity there is no direct mapping to SI Unit
                    // so we should create unit for this quantity

                    var un = DiscoverUnit(InnerQuantity);
                    if (un.SubUnits != null && un.SubUnits.Count > 0)
                    {
                        subUnits.AddRange(un.SubUnits);
                    }
                    else
                    {
                        subUnits.Add(un);
                    }
                }
                else
                {
                    //found :) create it with the exponent
                    var un = (Unit)Activator.CreateInstance(l2_InnerUnitType);
                    un.UnitExponent = InnerQuantity.Exponent;
                    un.UnitDimension = InnerQuantity.Dimension;

                    subUnits.Add(un);
                }
            }
        }
        else
        {
            //subclass of AnyQuantity
            //use direct mapping with the exponent of the quantity

            var un = (Unit)Activator.CreateInstance(innerUnitType);
            un.UnitExponent = quantity.Exponent;
            un.UnitDimension = quantity.Dimension;

            return un;
        }

        return new Unit(m_QuantityType, subUnits.ToArray());
    }

    /// <summary>
    /// This constructor creates a unit from several units.
    /// </summary>
    /// <param name="units"></param>
    internal Unit(Type? quantityType, params Unit[] units)
    {
        SubUnits = [];

        foreach (var un in units)
        {
            SubUnits.Add(un);
        }

        SubUnits = GroupUnits(SubUnits); //group similar units

        _Symbol = GenerateUnitSymbolFromSubBaseUnits();

        // if the passed type is AnyQuantity<object> for example
        //     then I want to get the type without type parameters AnyQuantity<>
        if (quantityType != null)
        {
            if (!quantityType.IsGenericTypeDefinition)
            {
                quantityType = quantityType.GetGenericTypeDefinition();
            }
        }

        if (quantityType != typeof(DerivedQuantity<>) && quantityType != null)
        {
            if (quantityType != typeof(DimensionlessQuantity<>)) _IsDefaultUnit = true;

            _QuantityType = quantityType;

            //get the unit dimension from the passed type.
            UnitDimension = QuantityDimension.DimensionFrom(quantityType);
        }
        else
        {
            //passed type is derivedQuantity which indicates that the units representing unknow derived quantity to the system
            //so that quantityType should be kept as derived quantity type.
            _QuantityType = quantityType;

            //get the unit dimension from the passed units.
            UnitDimension = QuantityDimension.Dimensionless;
            foreach (var uu in SubUnits)
                UnitDimension += uu.UnitDimension;
        }

        _IsBaseUnit = false;
    }

    #endregion

    /// <summary>
    /// Take the sub units recursively and return all of in a flat list.
    /// </summary>
    /// <param name="units"></param>
    /// <returns></returns>
    private static List<Unit> FlattenUnits(List<Unit> units)
    {
        List<Unit> all = [];
        foreach (var un in units)
        {
            if (un.IsStronglyTyped)
                all.Add(un);
            else
                all.AddRange(un.SubUnits);
        }
        return all;
    }

    /// <summary>
    /// Group all similar units to remove units that reached exponent zero
    /// also keep track of prefixes of metric units.
    /// </summary>
    /// <param name="bulk_units"></param>
    /// <returns></returns>
    private List<Unit> GroupUnits(List<Unit> bulk_units)
    {
        List<Unit> units = FlattenUnits(bulk_units);

        if (units.Count == 1) return units;

        List<Unit> groupedUnits = [];

        Dictionary<Tuple<Type, Type>, Unit> us = new();

        foreach (var un in units)
        {
             if (us.ContainsKey(un.UniqueKey))
             {
                 //check for prefixes before accumulating units
                //   otherwise I'll lose the UnitExponent value.
                if (un is MetricUnit metricUnit)
                {
                    //check prefixes to consider milli+Mega for example for overflow

                    var accumPrefix = ((MetricUnit)us[un.UniqueKey]).UnitPrefix;
                    var sourcePrefix = metricUnit.UnitPrefix;

                    try
                    {
                        //Word about MetricPrefix
                        //   The prefix takes the unit exponent as another exponent to it
                        //  so if we are talking about cm^2 actually it is c^2*m^2
                        //  suppose we multiply cm*cm this will give cm^2
                        //     so no need to alter the prefix value
                        // however remain a problem of different prefixes
                        // for example km * cm = ?m^2
                        //  k*c = ?^2
                        //    so ? = (k+c)/2  ;)
                        //  if there is a fraction remove the prefixes totally and substitute them      //  in the overflow flag.

                        // about division
                        // km / cm = ?<1>
                        // k/c = ?   or in exponent k-c=?

                        double targetExponent = us[un.UniqueKey].UnitExponent + un.UnitExponent;
                        double accumExponent = accumPrefix.Exponent * us[un.UniqueKey].UnitExponent;
                        double sourceExponent = sourcePrefix.Exponent * un.UnitExponent;

                        var resultExponent = accumExponent + sourceExponent;

                        if (!(us[un.UniqueKey].IsInverted ^ un.IsInverted))
                        {
                            // multiplication
                            if (resultExponent % targetExponent == 0)
                            {
                                //we can get the symbol of the sqrt of this
                                var unknown = resultExponent / targetExponent;

                                ((MetricUnit)us[un.UniqueKey]).UnitPrefix = MetricPrefix.FromExponent(unknown);
                            }
                            else
                            {
                                //we can't get the approriate symbol because we have a fraction
                                // like  kilo * centi = 3-2=1    1/2=0.5   or 1%2=1      // so we will take the whole fraction and make an overflow

                                ((MetricUnit)us[un.UniqueKey]).UnitPrefix = MetricPrefix.None;
                                if (resultExponent != 0)
                                {
                                    unitOverflow += Math.Pow(10, resultExponent);
                                    _IsOverflowed = true;
                                }
                            }
                        }
                        else
                        {
                            //division
                            //resultExponent = (accumExponent - sourceExponent);

                            ((MetricUnit)us[un.UniqueKey]).UnitPrefix = MetricPrefix.None;

                            if (resultExponent != 0)   //don't overflow in case of zero exponent target because there is not prefix in this case
                            {
                                unitOverflow += Math.Pow(10, resultExponent);
                                _IsOverflowed = true;
                            }
                        }
                    }
                    catch(MetricPrefixException mpe)
                    {
                        ((MetricUnit)us[un.UniqueKey]).UnitPrefix = mpe.CorrectPrefix;
                        unitOverflow += Math.Pow(10, mpe.OverflowExponent);
                        _IsOverflowed = true;
                    }

                }
                us[un.UniqueKey].UnitExponent += un.UnitExponent;
                us[un.UniqueKey].UnitDimension += un.UnitDimension;
             }
             else
             {
                 us[un.UniqueKey] = (Unit)un.MemberwiseClone();
             }
        }
        foreach (var un in us.Values)
        {
            if (un.UnitExponent != 0)
            {
                groupedUnits.Add(un);
            }
            else
            {
                //zero means units should be skipped
                // however we are testing for prefix if the unit is metric
                //  if the unit is metric and deprecated the prefix should be taken into consideration
                if (un is MetricUnit mu && mu.UnitPrefix.Exponent != 0)
                {
                    _IsOverflowed = true;
                    unitOverflow += Math.Pow(10, mu.UnitPrefix.Exponent);
                }
            }
        }

        return groupedUnits;
    }

    #region overflow code

    protected bool _IsOverflowed;

    /// <summary>
    /// Overflow flag.
    /// </summary>
    public bool IsOverflowed => _IsOverflowed;

    protected double unitOverflow;

    /// <summary>
    /// This method get the overflow from multiplying/divding metric units with different
    /// prefixes and then the unit exponent goes to ZERO,
    /// or when result prefix is over
    /// the value should be used to be multiplied by the quantity that units were associated to.
    /// after the execution of this method the overflow flag is reset again.
    /// </summary>
    public double GetUnitOverflow()
    {
        var u =  unitOverflow;
        unitOverflow = 0.0;
        _IsOverflowed = false;
        return u;
    }

    #endregion

    #region Unit Symbol processing

    /// <summary>
    /// adjust the symbol string.
    /// </summary>
    /// <returns></returns>
    private string GenerateUnitSymbolFromSubBaseUnits()
    {
        var unitNumerator = "";
        var unitDenominator = "";

        void ConcatenateUnit(string symbol, float exponent)
        {
            if (exponent > 0)
            {
                if (unitNumerator.Length > 0) unitNumerator += ".";

                unitNumerator += symbol;

                if (exponent > 1)
                {
                    unitNumerator += "^" + exponent.ToString(CultureInfo.InvariantCulture);
                }

                if (exponent is < 1 and > 0)
                {
                    unitNumerator += "^" + exponent.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (exponent < 0)
            {
                if (unitDenominator.Length > 0) unitDenominator += ".";

                unitDenominator += symbol;

                // validate less than -1
                if (exponent < -1)
                {
                    unitDenominator += "^" + Math.Abs(exponent).ToString(CultureInfo.InvariantCulture);
                }

                // validate between -1 and 0
                if (exponent is > -1 and < 0)
                {
                    unitDenominator += "^" + Math.Abs(exponent).ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        foreach (var unit in SubUnits)
        {
            ConcatenateUnit(unit.Symbol, unit.UnitExponent);
        }

        //return <UnitNumerator / UnitDenominator>
        string FormatUnitSymbol()
        {
            var unitSymbol = "<";

            if (unitNumerator.Length > 0)
                unitSymbol += unitNumerator;
            else
                unitSymbol += "1";

            if (unitDenominator.Length > 0) unitSymbol += "/" + unitDenominator;

            unitSymbol += ">";

            return unitSymbol;
        }

        var preFinalSymbol = FormatUnitSymbol();
        var finalSymbol = preFinalSymbol;

        var m = UnitPatternRegex().Match(preFinalSymbol);

        while (m.Success)
        {
            finalSymbol = finalSymbol.Replace(m.Groups[0].Value, "/" + m.Groups[1].Value);
            m = m.NextMatch();
        }

        return finalSymbol;
    }

    /// <summary>
    /// Matches <c>.&lt;1/UNIT&gt;</c> for replacement.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"\.<1/(.+?)>", RegexOptions.Compiled | RegexOptions.NonBacktracking)]
    private static partial Regex UnitPatternRegex();

    #endregion
}