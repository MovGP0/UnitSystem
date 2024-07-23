using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Attributes;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units;

public partial class Unit
{
    /// <summary>
    /// Returns quantity based on current unit instance.
    /// </summary>
    /// <typeparam name="T">Quatntity Storage Type</typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    internal AnyQuantity<T> MakeQuantity<T>(T value)
    {
        // create the corresponding quantity
        var qty = GetThisUnitQuantity<T>();

        // assign the unit to the created quantity
        qty.Unit = this;

        // assign the value to the quantity
        qty.Value = value;

        return qty;
    }

    /// <summary>
    /// Returns quantity from the current unit type.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static AnyQuantity<double> QuantityOf<TUnit>(double value) where TUnit:Unit, new()
    {
        Unit unit = new TUnit();
        return unit.MakeQuantity(value);
    }

    #region Helper Properties
    private static Type[]? unitTypes;

    /// <summary>
    /// All Types inherited from Unit Type.
    /// </summary>
    public static Type[] UnitTypes
    {
        get
        {
            if (unitTypes == null)
            {
                var allTypes = Assembly.GetExecutingAssembly().GetTypes();

                var types = from si in allTypes
                            where si.IsSubclassOf(typeof(Unit))
                            select si;

                unitTypes = types.ToArray();
            }

            return unitTypes;
        }
    }
    #endregion

    #region Helper Functions and Properties

    /// <summary>
    /// Gets the default unit type of quantity type parameter based on the unit system (namespace)
    /// under the Units name space.
    /// The Default Unit Type for length in Imperial is Foot for example.
    /// </summary>
    /// <param name="quantityType">quantity type</param>
    /// <param name="unitSystem">The Unit System or explicitly the namespace under Units Namespace</param>
    /// <returns>Unit Type Based on the unit system</returns>
    public static Type GetDefaultUnitTypeOf(Type quantityType, string unitSystem)
    {
        unitSystem = unitSystem.ToUpperInvariant();

        if (unitSystem.Contains("METRIC.SI"))
        {
            var oUnitType = GetDefaultSIUnitTypeOf(quantityType);
            return oUnitType;
        }

        //getting the generic type
        if (!quantityType.IsGenericTypeDefinition)
        {
            //the passed type is AnyQuantity<object> for example
            //I want to get the type without type parameters AnyQuantity<>

            quantityType = quantityType.GetGenericTypeDefinition();

        }


        //predictor of default unit.
        Func<Type, bool> SearchForQuantityType = unitType =>
        {
            //search in the attributes of the unit type
            var info = unitType as MemberInfo;

            var attributes = (object[])info.GetCustomAttributes(true);

            //get the UnitAttribute
            var ua = attributes.OfType<UnitAttribute>().SingleOrDefault();

            if (ua == null)
            {
                return false;
            }

            if (ua.QuantityType != quantityType)
            {
                return false;
            }

            if (ua is DefaultUnitAttribute)
            {
                //explicitly default unit.
                return true;
            }

            if (ua is MetricUnitAttribute mua)
            {
                // check if the unit has SystemDefault flag true or not.
                return mua.SystemDefault;
            }

            return false;
        };

        var currentUnitSystem = unitSystem;
        Type? systemUnitType = null;

        //search in upper namespaces also to get the default unit of the parent system.
        while (string.IsNullOrEmpty(currentUnitSystem) == false && systemUnitType == null)
        {
            // prepare the query that we will search in
            var systemUnitTypes =
                from ut in UnitTypes
                where ut.Namespace.ToUpperInvariant().EndsWith(currentUnitSystem, StringComparison.Ordinal)
                      || ut.Namespace.ToUpperInvariant().EndsWith("SHARED", StringComparison.Ordinal)
                select ut;

            // select the default by predictor from the query
            systemUnitType = systemUnitTypes.SingleOrDefault(SearchForQuantityType);

            currentUnitSystem = currentUnitSystem.LastIndexOf('.') < 0
                ? ""
                : currentUnitSystem[..currentUnitSystem.LastIndexOf('.')];
        }

        if (systemUnitType == null && unitSystem.Contains("METRIC"))
        {
            //try another catch for SI unit for this quantity
            //   because SI and metric units are disordered for now
            // so if the search of unit in parent metric doesn't exist then search for it in SI units.

            systemUnitType = GetDefaultSIUnitTypeOf(quantityType);
        }

        return systemUnitType;
    }

    /// <summary>
    /// Gets the unit type of quantity type parameter based on SI unit system.
    /// The function is direct mapping from types of quantities to types of units.
    /// if function returns null then this quantity dosen't have a statically linked unit to it.
    /// this means the quantity should return a unit in runtime.
    /// </summary>
    /// <param name="qType">Type of Quantity</param>
    /// <returns>SI Unit Type</returns>
    public static Type? GetDefaultSIUnitTypeOf(Type qType)
    {
        var quantityType = qType;

        // getting the generic type
        if (!quantityType.IsGenericTypeDefinition)
        {
            // the passed type is AnyQuantity<object> for example
            // I want to get the type without type parameters AnyQuantity<>
            quantityType = quantityType.GetGenericTypeDefinition();
        }

        // don't forget to include second in si units it is shared between all metric systems
        var siUnitTypes =
            from si in UnitTypes
            where si.Namespace.ToUpperInvariant().EndsWith("SI", StringComparison.Ordinal)
                  || si.Namespace.ToUpperInvariant().EndsWith("SHARED", StringComparison.Ordinal)
            select si;

        bool SearchForQuantityType(Type unitType)
        {
            //search in the attributes of the unit type
            var info = unitType as MemberInfo;

            var attributes = info.GetCustomAttributes(true);

            //get the UnitAttribute
            var ua = attributes.OfType<UnitAttribute>().SingleOrDefault();

            if (ua != null)
            {
                return ua.QuantityType == quantityType;
            }

            return false;
        }

        return siUnitTypes.SingleOrDefault(SearchForQuantityType);
    }

    #endregion

    /// <summary>
    /// Find Strongly typed unit.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    private static Unit FindUnit (string un)
    {
        var unit = un.Replace("$", "\\$");

        var unitVectorModifier = false;

        if (unit.EndsWith("!", StringComparison.Ordinal))
        {
            //it is intended of Radius length
            unit = unit.TrimEnd('!');
            unitVectorModifier = true; //unit modifier have one use for now is to convert the Length Quantity into Length Quantity into RadiusLength quantity
        }

        foreach (var unitType in UnitTypes)
        {
            var ua = GetUnitAttribute(unitType);
            if (ua != null)
            {
                // units are case sensitive
                if (Regex.Match(ua.Symbol, "^" + unit + "$", RegexOptions.Singleline).Success)
                {
                    var u = (Unit)Activator.CreateInstance(unitType)!;

                    //test unit if it is metric so that we remove the default prefix that created with it
                    if (u is MetricUnit metricUnit)
                    {
                        metricUnit.UnitPrefix = MetricPrefix.None;
                    }

                    if (unitVectorModifier)
                    {
                        //test if the dimension is length and modify it to be radius length
                        if (u.QuantityType == typeof(Length<>))
                        {
                            u.QuantityType = typeof(Displacement<>);

                        }

                        if(u.QuantityType == typeof(Force<>))
                        {
                            u.QuantityType = typeof(ForceVector<>);
                        }
                    }

                    return u;
                }
            }
        }

        throw new UnitNotFoundException(un);
    }

    /// <summary>
    /// Parse units with exponent and one division '/' with many '.'
    /// i.e. m/s m/s^2 kg.m/s^2
    /// </summary>
    /// <param name="units"></param>
    /// <returns></returns>
    public static Unit Parse(string units)
    {
        //  if found  treat store its value.
        //  m/s^2   m.K/m.s
        //  kg^2/in^2.s

        // sea
        //search for '/'

        var uny = units.Split('/');

        string[] numa = uny[0].Split('.');

        List<Unit> dunits = [];
        foreach (var num in numa)
        {
            dunits.Add(ParseUnit(num));
        }

        if (uny.Length > 1)
        {
            string[] dena = uny[1].Split('.');
            foreach (var den in dena)
            {
                var uu = ParseUnit(den);
                if (uu.SubUnits != null)
                {
                    //then it is unit with sub units in it
                    if (uu.SubUnits.Count == 1) uu = uu.SubUnits[0];
                }
                dunits.Add(uu.Invert());
            }
        }

        if (dunits.Count == 1) return dunits[0];

        //get the dimension of all units
        var ud = QuantityDimension.Dimensionless;

        foreach (var un in dunits)
        {
            ud += un.UnitDimension;
        }

        var uQType = QuantityDimension.GetQuantityTypeFrom(ud);
        return new Unit(uQType, dunits.ToArray());
    }

    /// <summary>
    /// Returns the unit corresponding to the passed string.
    /// Suppors units with exponent.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    internal static Unit ParseUnit(string un)
    {
        if (un == "1")
        {
            //this is dimensionless value
            return new Unit(typeof(Quantities.DimensionlessQuantities.DimensionlessQuantity<>));
        }

        //find '^'

        var upower = un.Split('^');

        var unit = upower[0];

        var power = 1;

        if (upower.Length > 1)
        {
            power = int.Parse(upower[1], CultureInfo.InvariantCulture);
        }

        Unit? finalUnit = null;

        //Phase 1: try direct mapping.
        try
        {
            finalUnit = FindUnit(unit);
        }
        catch(UnitNotFoundException)
        {
            //try to find if it as a Metric unit with prefix
            //loop through all prefixes.
            for (var i = 10; i >= -10; i -= 1)
            {
                if (i == 0) i--; //skip the None prefix
                if (unit.StartsWith(MetricPrefix.GetPrefix(i).Symbol, StringComparison.Ordinal))
                {
                    //found

                    var mp = MetricPrefix.GetPrefix(i);
                    var upart = unit[mp.Symbol.Length..];

                    //then it should be MetricUnit otherwise die :)

                    var u = FindUnit(upart) as MetricUnit;

                    if (u == null) goto nounit;
                    u.UnitPrefix = mp;

                    finalUnit = u;
                    break;
                }
            }
        }

        if (finalUnit == null)
        {
            goto nounit;
        }

        if (power > 1)
        {
            //discover the new type
            var ud = finalUnit.UnitDimension * power;

            Unit[] chobits = new Unit[power];  //what is chobits any way :O

            for (var iy = 0; iy < power; iy++)
            {
                chobits[iy] = (Unit)finalUnit.MemberwiseClone();
            }

            Type? uQType = null;
            uQType = QuantityDimension.GetQuantityTypeFrom(ud);
            finalUnit = new Unit(uQType, chobits);

        }

        return finalUnit;

        nounit:
        throw new UnitNotFoundException(un);

    }

    private static Dictionary<Type, UnitAttribute> UnitsAttributes = new();

    /// <summary>
    /// Get the unit attribute which hold the unit information.
    /// </summary>
    /// <param name="unitType"></param>
    /// <returns></returns>
    public static UnitAttribute? GetUnitAttribute(Type unitType)
    {
        if (!UnitsAttributes.TryGetValue(unitType, out var ua))
        {
            var attributes = unitType.GetCustomAttributes(typeof(UnitAttribute), true);

            //get the UnitAttribute
            ua = attributes.OfType<UnitAttribute>().SingleOrDefault();
            UnitsAttributes.Add(unitType, ua);
        }

        return ua;
    }

    /// <summary>
    /// Returns the unit of strongly typed metric unit to unit with sub units as base units
    /// and add the prefix to the expanded base units.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static Unit ExpandMetricUnit(Unit unit)
    {
        List<Unit> defaultUnits = [];

        if (unit.IsBaseUnit)
        {
            //if baseunit then why we would convert it
            // put it immediately
            return unit;
        }

        var qdim = QuantityDimension.DimensionFrom(unit.QuantityType);

        if (unit is MetricUnit metricUnit)
        {
            // pure unit without derived units like Pa, N, and L
            var u = DiscoverUnit(qdim);

            var baseUnits = u.SubUnits;

            // add prefix to the first unit in the array

            ((MetricUnit)baseUnits[0]).UnitPrefix += metricUnit.UnitPrefix;

            defaultUnits.AddRange(baseUnits);
        }

        return new Unit(unit._QuantityType, defaultUnits.ToArray());
    }

    /// <summary>
    /// Takes string of the form number and unit i.e. "50.34 &lt;kg>"
    /// and returns Quantity of the discovered unit.
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static AnyQuantity<double> ParseQuantity(string quantity)
    {
        if (TryParseQuantity(quantity, out var aty))
        {
            return aty;
        }

        throw new QuantityException("Couldn't parse to quantity");
    }

    private const string DoubleNumber = @"[-+]?\d+(\.\d+)*([eE][-+]?\d+)*";
    private const string UnitizedNumber = "^(?<num>" + DoubleNumber + @")\s*<(?<unit>([\w\$\.\^/!]+)?)>$";
    private static Regex UnitizedNumberRegex = new(UnitizedNumber, RegexOptions.Compiled | RegexOptions.NonBacktracking);

    public static bool TryParseQuantity(string quantity, out AnyQuantity<double> qty)
    {
        double val;

        var um = UnitizedNumberRegex.Match(quantity.Trim());
        if (um.Success)
        {
            var varUnit = um.Groups["unit"].Value;
            val = double.Parse(um.Groups["num"].Value, CultureInfo.InvariantCulture);

            var un = Parse(varUnit);
            qty = un.GetThisUnitQuantity(val);

            return true;
        }

        if (double.TryParse(quantity, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
        {
            qty = DiscoverUnit(QuantityDimension.Dimensionless).GetThisUnitQuantity(val);
            return true;
        }

        qty = default;
        return false;
    }

    public Unit RaiseUnitPower(float power)
    {
        //make short-cut when power equal zero return dimensionless unit immediatly
        //  because if I left the execution to the end      //  the dimensionless unit is created and wrapping the original unit under sub unit
        //    and this made errors in conversion between dimensionless units :).
        if (power == 0)
        {
            return DiscoverUnit(QuantityDimension.Dimensionless);
        }

        var u = (Unit)MemberwiseClone();

        if (SubUnits != null)
        {
            u.SubUnits = new List<Unit>(SubUnits.Count);

            for(var i=0; i<SubUnits.Count;i++)
            {
                u.SubUnits.Add(SubUnits[i].RaiseUnitPower(power));
            }
        }
        else
        {
            u.UnitExponent *= power;   //the exponent is changing in strongly typed units
        }

        u.UnitDimension = UnitDimension * power; //must change the unit dimension of the unit
        // however because the unit is having sub units we don't have to modify the exponent of it
        //  note: unit that depend on sub units is completly unaware of its exponent
        //    or I should say it is always equal = 1

        u._QuantityType = QuantityDimension.GetQuantityTypeFrom(u.UnitDimension);
        if (u.SubUnits == null && u.UnitExponent == 1)
        {
            //no sub units and exponent ==1  then no need to processing
            return u;
        }

        if (u.SubUnits == null)
        {
            //exponent != 1  like ^5 ^0.3  we need processing
            return new Unit(u._QuantityType, u);
        }

        // consist of sub units definitely we need processing.
        return new Unit(u._QuantityType, u.SubUnits.ToArray());
    }
}