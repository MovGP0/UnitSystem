using System.Globalization;
using System.Reflection;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities;
using QuantitySystem.DimensionDescriptors;
using QuantitySystem.Quantities.DimensionlessQuantities;
using System.Text.RegularExpressions;

namespace QuantitySystem;

/// <summary>
/// Quantity Dimension.
/// dim Q = Lα Mβ Tγ Iδ Θε Nζ Jη
/// </summary>
public sealed partial class QuantityDimension
{
    public QuantityDimension()
    {
    }

    /// <summary>
    /// basic constructor for MLT Dimensions.
    /// </summary>
    /// <param name="mass"></param>
    /// <param name="length"></param>
    /// <param name="time"></param>
    public QuantityDimension(float mass, float length, float time)
    {
        Mass = new MassDescriptor(mass);
        Length = new LengthDescriptor(length, 0);
        Time = new TimeDescriptor(time);
    }

    public QuantityDimension(float mass, float length, float time, float temperature)
    {
        Mass = new MassDescriptor(mass);
        Length = new LengthDescriptor(length, 0);
        Time = new TimeDescriptor(time);
        Temperature = new TemperatureDescriptor(temperature);
    }

    public QuantityDimension(float mass, float length, float time, float temperature, float electricalCurrent, float amountOfSubstance, float luminousIntensity)
    {
        Mass = new MassDescriptor(mass);
        Length = new LengthDescriptor(length, 0);
        Time = new TimeDescriptor(time);
        Temperature = new TemperatureDescriptor(temperature);
        ElectricCurrent = new ElectricCurrentDescriptor( electricalCurrent);
        AmountOfSubstance = new AmountOfSubstanceDescriptor(amountOfSubstance);
        LuminousIntensity = new LuminousIntensityDescriptor(luminousIntensity);
    }

    public static QuantityDimension operator +(QuantityDimension firstDimension, QuantityDimension secondDimension)
        => Add(firstDimension, secondDimension);

    public static QuantityDimension Add(QuantityDimension firstDimension, QuantityDimension secondDimension)
    {
        return new()
        {
            Mass = firstDimension.Mass.Add(secondDimension.Mass),
            Length = firstDimension.Length.Add(secondDimension.Length),
            Time = firstDimension.Time.Add( secondDimension.Time),
            Temperature = firstDimension.Temperature.Add(secondDimension.Temperature),
            ElectricCurrent = firstDimension.ElectricCurrent.Add(secondDimension.ElectricCurrent),
            AmountOfSubstance = firstDimension.AmountOfSubstance.Add( secondDimension.AmountOfSubstance),
            LuminousIntensity = firstDimension.LuminousIntensity.Add(secondDimension.LuminousIntensity),
            Currency = firstDimension.Currency.Add(secondDimension.Currency),
            Digital = firstDimension.Digital.Add(secondDimension.Digital)
        };
    }

    public static QuantityDimension operator -(QuantityDimension firstDimension, QuantityDimension secondDimension)
        => Subtract(firstDimension, secondDimension);

    public static QuantityDimension Subtract(QuantityDimension firstDimension, QuantityDimension secondDimension)
    {
        return new QuantityDimension
        {
            Mass = firstDimension.Mass.Subtract(secondDimension.Mass),
            Length = firstDimension.Length.Subtract(secondDimension.Length),
            Time = firstDimension.Time.Subtract(secondDimension.Time),
            Temperature = firstDimension.Temperature.Subtract(secondDimension.Temperature),
            ElectricCurrent = firstDimension.ElectricCurrent.Subtract(secondDimension.ElectricCurrent),
            AmountOfSubstance = firstDimension.AmountOfSubstance.Subtract(secondDimension.AmountOfSubstance),
            LuminousIntensity = firstDimension.LuminousIntensity.Subtract(secondDimension.LuminousIntensity),
            Currency = firstDimension.Currency.Subtract(secondDimension.Currency),
            Digital = firstDimension.Digital.Subtract(secondDimension.Digital)
        };
    }

    public static QuantityDimension operator *(QuantityDimension dimension, float exponent)
        => Multiply(dimension, exponent);

    public static QuantityDimension Multiply(QuantityDimension dimension, float exponent)
    {
        return new QuantityDimension
        {
            Mass = dimension.Mass.Multiply(exponent),
            Length = dimension.Length.Multiply(exponent),
            Time = dimension.Time.Multiply(exponent),
            Temperature = dimension.Temperature.Multiply(exponent),
            ElectricCurrent = dimension.ElectricCurrent.Multiply(exponent),
            AmountOfSubstance = dimension.AmountOfSubstance.Multiply(exponent),
            LuminousIntensity = dimension.LuminousIntensity.Multiply(exponent),
            Currency = dimension.Currency.Multiply(exponent),
            Digital = dimension.Digital.Multiply(exponent)
        };
    }

    private static List<Type> CurrentQuantityTypes = [];

    /// <summary>
    /// holding Dimension -> Quantity instance  to be clonned.
    /// </summary>
    static Dictionary<QuantityDimension, Type> CurrentQuantitiesDictionary = new();

    /// <summary>
    /// Quantity Name -> Quantity Type  as all quantity names are unique
    /// </summary>
    static Dictionary<string, Type> CurrentQuantitiesNamesDictionary = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// holding Quantity -> Dimension
    /// </summary>
    static Dictionary<Type, QuantityDimension> CurrentDimensionsDictionary = new();

    public static Type[] AllQuantitiesTypes => CurrentQuantitiesNamesDictionary.Values.ToArray();

    public static string[] AllQuantitiesNames => CurrentQuantitiesNamesDictionary.Keys.ToArray();

    /// <summary>
    /// Cache all quantities with their Dimensions.
    /// </summary>
    static QuantityDimension()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        Type[] types = currentAssembly.GetTypes();

        var quantityTypes =
            from QuantityType in types
            where QuantityType.IsSubclassOf(typeof(BaseQuantity))
            select QuantityType;

        CurrentQuantityTypes.AddRange(quantityTypes);

        // storing the quantity types with their dimensions
        foreach (var quantityType in CurrentQuantityTypes)
        {
            //cache the quantities that is not abstract types
            if (quantityType.IsAbstract || quantityType == typeof(DerivedQuantity<>))
            {
                continue;
            }

            //make sure not to include Dimensionless quantities due to they are F0L0T0
            if (quantityType.BaseType.Name == typeof(DimensionlessQuantity<>).Name)
            {
                continue;
            }

            //store dimension as key and Quantity Type .
            //create AnyQuantity<Object>  Object container used just for instantiation
            var quantity = (AnyQuantity<Object>)Activator.CreateInstance(quantityType.MakeGenericType(typeof(object)))!;

            //store the Dimension and the corresponding Type;
            CurrentQuantitiesDictionary.Add(quantity.Dimension, quantityType);

            //store quantity type as key and corresponding dimension as value.
            CurrentDimensionsDictionary.Add(quantityType, quantity.Dimension);

            //store the quantity name and type with insensitive names
            CurrentQuantitiesNamesDictionary.Add(quantityType.Name[..^2], quantityType);
        }
    }

    /// <summary>
    /// Returns the quantity type or typeof(DerivedQuantity&lt;&gt;) without throwing exception
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public static Type GetQuantityTypeFrom(QuantityDimension dimension)
    {
        return CurrentQuantitiesDictionary.TryGetValue(dimension, out var qType)
            ? qType
            : typeof(DerivedQuantity<>);
    }

    /// <summary>
    /// Get the corresponding typed quantity in the framework of this dimension
    /// Throws <see cref="QuantityNotFoundException"/> when there is corresponding one.
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public static Type QuantityTypeFrom(QuantityDimension dimension)
    {
        try
        {
            return CurrentQuantitiesDictionary[dimension];
        }
        catch (KeyNotFoundException ex)
        {
            throw new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);
        }
    }

    /// <summary>
    /// Gets the quantity type from the name and throws QuantityNotFounException if not found.
    /// </summary>
    /// <param name="quantityName"></param>
    /// <returns></returns>
    public static Type QuantityTypeFrom(string quantityName)
    {
        try
        {
            return CurrentQuantitiesNamesDictionary[quantityName];
        }
        catch (KeyNotFoundException ex)
        {
            throw new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);
        }
    }

    static Dictionary<Type, object> QuantitiesCached = new();

    /// <summary>
    /// Returns Strongly typed Any Quantity From the dimension based on the discovered quantities discovered when
    /// framework initiated.
    /// Throws <see cref="QuantityNotFoundException"/> when quantity is not found.
    /// </summary>
    /// <typeparam name="T">The value container of the Quantity</typeparam>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public static AnyQuantity<T> QuantityFrom<T>(QuantityDimension dimension)
    {
        lock (QuantitiesCached)
        {
            var quantityType = QuantityTypeFrom(dimension);

            //the quantity type now is without container type we should generate it

            var quantityWithContainerType = quantityType.MakeGenericType(typeof(T));

            if (QuantitiesCached.TryGetValue(quantityWithContainerType, out var j))
            {
                return ((AnyQuantity<T>)j).Clone();
            }

            j = Activator.CreateInstance(quantityWithContainerType)!;
            QuantitiesCached.Add(quantityWithContainerType, j);
            return ((AnyQuantity<T>)j).Clone();
        }
    }

    /// <summary>
    /// Returns the quantity dimension based on the quantity type.
    /// </summary>
    /// <param name="quantityType"></param>
    /// <returns></returns>
    public static QuantityDimension DimensionFrom(Type quantityType)
    {
        if (!quantityType.IsGenericTypeDefinition)
        {
            // the passed type is AnyQuantity<object> for example
            // I want to get the type without type parameters AnyQuantity<>
            quantityType = quantityType.GetGenericTypeDefinition();
        }

        try
        {
            return CurrentDimensionsDictionary[quantityType];
        }
        catch (KeyNotFoundException ex)
        {
            // if key not found and quantityType is really Quantity
            // then return dimensionless Quantity

            if (quantityType.BaseType.GetGenericTypeDefinition() == typeof(DimensionlessQuantity<>))
            {
                return Dimensionless;
            }

            throw new DimensionNotFoundException("UnExpected Exception. TypeOf<" + quantityType.ToString() + "> have no dimension associate with it", ex);
        }
    }

    /// <summary>
    /// Extract the Mass power from MLT string.
    /// </summary>
    /// <param name="mlt"></param>
    /// <returns></returns>
    private static int ExponentOfMass(string mlt)
    {
        var m_index = 0;
        var l_index = mlt.IndexOf('L');
        return int.Parse(mlt.Substring(m_index + 1, l_index - m_index - 1), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Extract the Length Power from MLT string.
    /// </summary>
    /// <param name="MLT"></param>
    /// <returns></returns>
    private static int ExponentOfLength(string mlt)
    {
        var l_index = mlt.IndexOf('L');
        var t_index = mlt.IndexOf('T');
        return int.Parse(mlt.Substring(l_index + 1, t_index - l_index - 1), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Extract the Time Power from MLT string.
    /// </summary>
    /// <param name="MLT"></param>
    /// <returns></returns>
    private static int ExponentOfTime(string mlt)
    {
        var t_index = mlt.IndexOf('T');
        return int.Parse(mlt.Substring(t_index + 1, mlt.Length - t_index - 1), CultureInfo.InvariantCulture);
    }

    public static QuantityDimension ParseMLT(string mlt)
    {
        var m = ExponentOfMass(mlt);
        var l = ExponentOfLength(mlt);
        var t = ExponentOfTime(mlt);

        return new QuantityDimension(m, l, t);
    }

    public static QuantityDimension Parse(string dimension)
    {
        dimension = dimension.Trim();

        // M L T I O N J   C
        List<float> exps = [];
        if (char.IsNumber(dimension[0]))
        {
            // pars// parsing started with numbers which will be most probably          //   on the format of numbers with spaces
            string[] dims = dimension.Split(' ');
            foreach (var dim in dims)
            {
                var dimtrimmed = dim.Trim();

                if (!string.IsNullOrEmpty(dimtrimmed))
                {
                    exps.Add(float.Parse(dimtrimmed));
                }
            }

            var M = exps.Count > 0 ? exps[0] : 0;
            var L = exps.Count > 1 ? exps[1] : 0;
            var T = exps.Count > 2 ? exps[2] : 0;
            var I = exps.Count > 3 ? exps[3] : 0;
            var O = exps.Count > 4 ? exps[4] : 0;
            var N = exps.Count > 5 ? exps[5] : 0;
            var J = exps.Count > 6 ? exps[6] : 0;
            var C = exps.Count > 7 ? exps[7] : 0;
            var D = exps.Count > 8 ? exps[8] : 0;

            return new QuantityDimension(M, L, T, I, O, N, J)
            {
                Currency = new CurrencyDescriptor(C),
                Digital = new DigitalDescriptor(D)
            };
        }
        else
        {
            // the format is based on letter and number

            var dumber  = dimension.ToUpperInvariant();

            var mts = Regex.Matches(dumber, @"(([MmLlTtIiOoNnJjCcDd])(\-*[0-9]+))+?");

            var edps = new Dictionary<char,float>();

            foreach(Match m in mts)
            {
                edps.Add(m.Groups[2].Value[0], float.Parse(m.Groups[3].Value));
            }

            if (!edps.ContainsKey('M')) edps['M'] = 0;
            if (!edps.ContainsKey('L')) edps['L'] = 0;
            if (!edps.ContainsKey('T')) edps['T'] = 0;
            if (!edps.ContainsKey('I')) edps['I'] = 0;
            if (!edps.ContainsKey('O')) edps['O'] = 0;
            if (!edps.ContainsKey('N')) edps['N'] = 0;
            if (!edps.ContainsKey('J')) edps['J'] = 0;
            if (!edps.ContainsKey('C')) edps['C'] = 0;
            if (!edps.ContainsKey('D')) edps['D'] = 0;

            var qd = new QuantityDimension(edps['M']
                , edps['L'], edps['T'], edps['I'], edps['O']
                , edps['N'], edps['J'])
            {
                Currency = new CurrencyDescriptor(edps['C']),
                Digital = new DigitalDescriptor(edps['D'])
            };

            return qd;
        }
    }

    public static QuantityDimension Dimensionless => new();

    public QuantityDimension Invert()
    {
        return new QuantityDimension
        {
            Mass = Mass.Invert(),
            Length = Length.Invert(),
            Time = Time.Invert(),
            ElectricCurrent = ElectricCurrent.Invert(),
            Temperature = Temperature.Invert(),
            AmountOfSubstance = AmountOfSubstance.Invert(),
            LuminousIntensity = LuminousIntensity.Invert(),
            Currency = Currency.Invert(),
            Digital = Digital.Invert()
        };
    }

    public QuantityDimension(QuantityDimension dimension)
    {
        Mass = dimension.Mass;
        Length = dimension.Length;
        Time = dimension.Time;
        ElectricCurrent = dimension.ElectricCurrent;
        Temperature = dimension.Temperature;
        AmountOfSubstance = dimension.AmountOfSubstance;
        LuminousIntensity = dimension.LuminousIntensity;
        Currency = dimension.Currency;
        Digital = dimension.Digital;
    }
}