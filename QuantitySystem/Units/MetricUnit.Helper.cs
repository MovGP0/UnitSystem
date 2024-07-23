using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units;

public abstract partial class MetricUnit
{
    private static AnyQuantity<double> MakeQuantity(MetricUnit unit, MetricPrefix siPrefix, double value)
    {
        //assign its prefix
        unit.UnitPrefix = siPrefix;

        //create the corresponding quantity
        var qty = unit.GetThisUnitQuantity<double>();

        //assign the unit to the created quantity
        qty.Unit = unit;

        //assign the value to the quantity
        qty.Value = value;

        return qty;
    }

    public static AnyQuantity<double> Yotta<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Yotta, value);

    public static AnyQuantity<double> Zetta<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Zetta, value);

    public static AnyQuantity<double> Exa<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Exa, value);

    public static AnyQuantity<double> Peta<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Peta, value);

    public static AnyQuantity<double> Tera<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Tera, value);

    public static AnyQuantity<double> Giga<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Giga, value);

    public static AnyQuantity<double> Mega<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Mega, value);

    public static AnyQuantity<double> Kilo<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Kilo, value);

    public static AnyQuantity<double> Hecto<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Hecto, value);

    public static AnyQuantity<double> Deka<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Deka, value);

    public static AnyQuantity<double> None<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.None, value);

    public static AnyQuantity<double> Deci<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Deci, value);

    public static AnyQuantity<double> Centi<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Centi, value);

    public static AnyQuantity<double> Milli<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Milli, value);

    public static AnyQuantity<double> Micro<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Micro, value);

    public static AnyQuantity<double> Nano<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Nano, value);

    public static AnyQuantity<double> Pico<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Pico, value);

    public static AnyQuantity<double> Femto<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Femto, value);

    public static AnyQuantity<double> Atto<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Atto, value);

    public static AnyQuantity<double> Zepto<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Zepto, value);

    public static AnyQuantity<double> Yocto<TUnit>(double value) where TUnit : MetricUnit, new()
        => MakeQuantity(new TUnit(), MetricPrefix.Yocto, value);

    /// <summary>
    /// Return the SI Unit of strongly typed quantity.
    /// </summary>
    /// <typeparam name="TQuantity"></typeparam>
    /// <returns></returns>
    public static MetricUnit UnitOf<TQuantity>() where TQuantity : BaseQuantity, new()
    {
        //try direct mapping
        var defaultType = GetDefaultSIUnitTypeOf(typeof(TQuantity));
        var unit = defaultType != null ? Activator.CreateInstance(defaultType) as MetricUnit : null;

        if (unit is not null)
        {
            return unit;
        }

        // if failed you should generate it
        // try first the child quantities in the quantity instance if its base is derived quantity
        // and DerivedQuantity itself.

        var dimension = QuantityDimension.DimensionFrom(typeof(TQuantity));

        //return a derived unit.
        //return new DerivedSIUnit(dimension);
        throw new NotImplementedException();
    }

    public static AnyQuantity<double> GetUnitizedQuantityOf<TQuantity>(double value) where TQuantity : BaseQuantity, new()
    {
        var unit = UnitOf<TQuantity>();
        var aq = unit.GetThisUnitQuantity<double>();
        aq.Value = value;
        return aq;
    }
}