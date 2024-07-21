using System.Globalization;
using System.Reflection;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities;

using QuantitySystem.DimensionDescriptors;
using QuantitySystem.Quantities.DimensionlessQuantities;
using System.Text.RegularExpressions;

namespace QuantitySystem
{
    /// <summary>
    /// Quantity Dimension.
    /// dim Q = Lα Mβ Tγ Iδ Θε Nζ Jη
    /// </summary>
    public class QuantityDimension
    {

        #region Dimension Physical Properties
        public MassDescriptor Mass { get; set; }
        public LengthDescriptor Length { get; set; }
        public TimeDescriptor Time { get; set; }
        public ElectricCurrentDescriptor ElectricCurrent { get; set; }
        public TemperatureDescriptor Temperature { get; set; }
        public AmountOfSubstanceDescriptor AmountOfSubstance { get; set; }
        public LuminousIntensityDescriptor LuminousIntensity { get; set; }
        #endregion

        #region Dimension Extra Properties
        public CurrencyDescriptor Currency { get; set; }
        public DigitalDescriptor Digital { get; set; }
        #endregion

        #region Constructors

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


        #endregion


        #region string representaions of important values

        /// <summary>
        /// The Quality in MLT form
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MLT")]
        public virtual string MLT
        {
            get
            {
                var mass = "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);
                var length = "L" + Length.Exponent.ToString(CultureInfo.InvariantCulture);
                var time = "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);

                return mass + length + time;
            }
        }

        #endregion


        #region Force Component

        /// <summary>
        /// returns the power of Force.
        /// </summary>
        public float ForceExponent
        {
            get
            {
                //M1L1T-2
                //take from MLT untill the M==0

                var TargetM = Mass.Exponent;


                float TargetF = 0;


                while (TargetM > 0)
                {
                    TargetM--;
                    TargetF++;
                }

                return TargetF;
            }
        }

        /// <summary>
        /// FLT powers based on Force Length, and Time.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FLT")]
        public string FLT
        {
            get
            {

                //M1L1T-2
                //take from MLT untill the M==0

                var TargetM = Mass.Exponent;

                var TargetL = Length.Exponent;

                var TargetT = Time.Exponent;

                float TargetF = 0;


                while (TargetM > 0)
                {
                    TargetM--;
                    TargetF++;

                    TargetL -= 1;
                    TargetT += 2;
                }


                var force = "F" + TargetF.ToString(CultureInfo.InvariantCulture);
                var length = "L" + TargetL.ToString(CultureInfo.InvariantCulture);
                var time = "T" + TargetT.ToString(CultureInfo.InvariantCulture);

                return force + length + time;

            }
        }


        #endregion

        #region Equality

        public override string ToString()
        {
            var dim = "";

            dim += "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);


            var lll = "";

            if (Length.VectorExponent != 0)
            {
                dim +=
                    string.Format("L{0}(S{1}V{2}M{3})",
                    Length.Exponent.ToString(CultureInfo.InvariantCulture),
                    Length.ScalarExponent.ToString(CultureInfo.InvariantCulture),
                    Length.VectorExponent.ToString(CultureInfo.InvariantCulture),
                    Length.MatrixExponent.ToString(CultureInfo.InvariantCulture)
                    );
            }
            else
            {
                dim += string.Format("L{0}",
                    Length.Exponent.ToString(CultureInfo.InvariantCulture));
            }
            
            dim += "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "I" + ElectricCurrent.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "O" + Temperature.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "N" + AmountOfSubstance.Exponent.ToString(CultureInfo.InvariantCulture);
            dim += "J" + LuminousIntensity.Exponent.ToString(CultureInfo.InvariantCulture);

            dim += "$" + Currency.Exponent.ToString(CultureInfo.InvariantCulture);
            
            dim += "D" + Digital.Exponent.ToString(CultureInfo.InvariantCulture);

            return dim;
        }

        public override bool Equals(object obj)
        {
            var QD = obj as QuantityDimension;


            if (QD != null)
            {
                if (!ElectricCurrent.Equals(QD.ElectricCurrent))
                    return false;

                if (!Length.Equals(QD.Length))
                    return false;
                
                if (!LuminousIntensity.Equals( QD.LuminousIntensity))
                    return false;

                if (!Mass.Equals(QD.Mass))
                    return false;

                if (!AmountOfSubstance.Equals(QD.AmountOfSubstance))
                    return false;

                if (!Temperature.Equals(QD.Temperature))
                    return false;

                if (!Time.Equals(QD.Time))
                    return false;

                if (!Currency.Equals(QD.Currency))
                    return false;

                if (!Digital.Equals(QD.Digital))
                    return false;

                return true;
            }
            else
            {
                return false; 
            }
        }

        /// <summary>
        /// Equality here based on first level of exponent validation.
        /// Which means length explicitly is compared to the total dimension value not on radius and normal length values.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public bool IsEqual(QuantityDimension dimension)
        {
            if (ElectricCurrent.Exponent != dimension.ElectricCurrent.Exponent)
                return false;

            if (Length.Exponent != dimension.Length.Exponent)
                return false;

            if (LuminousIntensity.Exponent != dimension.LuminousIntensity.Exponent)
                return false;

            if (Mass.Exponent != dimension.Mass.Exponent)
                return false;

            if (AmountOfSubstance.Exponent != dimension.AmountOfSubstance.Exponent)
                return false;

            if (Temperature.Exponent != dimension.Temperature.Exponent)
                return false;

            if (Time.Exponent != dimension.Time.Exponent)
                return false;

            if (Currency.Exponent != dimension.Currency.Exponent)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            var hc = ToString().GetHashCode();
            return hc;
        }
        
        #endregion

        #region Dimension Calculations

        public static QuantityDimension operator +(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Add(firstDimension, secondDimension);
        }

        public static QuantityDimension Add(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {

            var QD = new QuantityDimension
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

            return QD;
        }

        public static QuantityDimension operator -(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            return Subtract(firstDimension, secondDimension);
        }

        public static QuantityDimension Subtract(QuantityDimension firstDimension, QuantityDimension secondDimension)
        {
            var QD = new QuantityDimension
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

            return QD;
        }

        public static QuantityDimension operator *(QuantityDimension dimension, float exponent)
        {
            return Multiply(dimension, exponent);
        }

        public static QuantityDimension Multiply(QuantityDimension dimension, float exponent)
        {
            var QD = new QuantityDimension
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


            return QD;

        }
        #endregion
    
    
    
        #region Quantities Preparation
        private static List<Type> CurrentQuantityTypes = [];


        /// <summary>
        /// holding Dimension -> Quantity instance  to be clonned.
        /// </summary>
        static Dictionary<QuantityDimension, Type> CurrentQuantitiesDictionary = new Dictionary<QuantityDimension, Type>();

        /// <summary>
        /// Quantity Name -> Quantity Type  as all quantity names are unique
        /// </summary>
        static Dictionary<string, Type> CurrentQuantitiesNamesDictionary = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// holding Quantity -> Dimension
        /// </summary>
        static Dictionary<Type, QuantityDimension> CurrentDimensionsDictionary = new Dictionary<Type, QuantityDimension>();

        public static Type[] AllQuantitiesTypes
        {
            get
            {
                return CurrentQuantitiesNamesDictionary.Values.ToArray();
            }
        }

        public static string[] AllQuantitiesNames
        {
            get
            {
                return CurrentQuantitiesNamesDictionary.Keys.ToArray();
            }
        }

       

        /// <summary>
        /// Cache all quantities with their Dimensions.
        /// </summary>
        static QuantityDimension()
        {

            var CurrentAssembly = Assembly.GetExecutingAssembly();
            Type[] types = CurrentAssembly.GetTypes();

            var QuantityTypes = from QuantityType in types
                                where QuantityType.IsSubclassOf(typeof(BaseQuantity))
                                select QuantityType;

            CurrentQuantityTypes.AddRange(QuantityTypes);

            //storing the quantity types with thier dimensions

            foreach (var QuantityType in CurrentQuantityTypes)
            {
                //cach the quantities that is not abstract types

                if (QuantityType.IsAbstract == false && QuantityType != typeof(DerivedQuantity<>))
                {
                    //make sure not to include Dimensionless quantities due to they are F0L0T0
                    if (QuantityType.BaseType.Name != typeof(DimensionlessQuantity<>).Name)
                    {

                        //store dimension as key and Quantity Type .
                        
                        //create AnyQuantity<Object>  Object container used just for instantiation
                        AnyQuantity<Object> Quantity = (AnyQuantity<Object>)Activator.CreateInstance(QuantityType.MakeGenericType(typeof(object)));

                        //store the Dimension and the corresponding Type;
                        CurrentQuantitiesDictionary.Add(Quantity.Dimension, QuantityType);

                        //store quantity type as key and corresponding dimension as value.
                        CurrentDimensionsDictionary.Add(QuantityType, Quantity.Dimension);

                        //store the quantity name and type with insensitive names
                        CurrentQuantitiesNamesDictionary.Add(QuantityType.Name[..^2], QuantityType);
                    
                    }
                }
            }

            
            
        }
        #endregion


        #region Quantity utilities

        /// <summary>
        /// Returns the quantity type or typeof(DerivedQuantity&lt;&gt;) without throwing exception
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Type GetQuantityTypeFrom(QuantityDimension dimension)
        {
            Type qType;
            if (CurrentQuantitiesDictionary.TryGetValue(dimension, out qType))
                return qType;
            else
                return typeof(DerivedQuantity<>);
        }

        /// <summary>
        /// Get the corresponding typed quantity in the framework of this dimension
        /// Throws QuantityNotFounException when there is corresponding one.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Type QuantityTypeFrom(QuantityDimension dimension)
        {
            try
            {
                var QuantityType = CurrentQuantitiesDictionary[dimension];


                return QuantityType;

            }
            catch (KeyNotFoundException ex)
            {
                var qnfe = new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);

                throw qnfe;
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
                var QuantityType = CurrentQuantitiesNamesDictionary[quantityName];


                return QuantityType;

            }
            catch (KeyNotFoundException ex)
            {
                var qnfe = new QuantityNotFoundException("Couldn't Find the quantity dimension in the dimensions Hash Key", ex);

                throw qnfe;
            }

        }


        static Dictionary<Type, object> QuantitiesCached = new Dictionary<Type, object>();

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
                var QuantityType = QuantityTypeFrom(dimension);

                //the quantity type now is without container type we should generate it

                var QuantityWithContainerType = QuantityType.MakeGenericType(typeof(T));

                object j;
                if (QuantitiesCached.TryGetValue(QuantityWithContainerType, out j))
                {
                    return (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();
                }
                else
                {
                    j = Activator.CreateInstance(QuantityWithContainerType);
                    QuantitiesCached.Add(QuantityWithContainerType, j);
                    return (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();
                }
            }

        
        }


        /// <summary>
        /// Returns the quantity dimenstion based on the quantity type.
        /// </summary>
        /// <param name="quantityType"></param>
        /// <returns></returns>
        public static QuantityDimension DimensionFrom(Type quantityType)
        {
            if (!quantityType.IsGenericTypeDefinition)
            {
                //the passed type is AnyQuantity<object> for example
                //I want to get the type without type parameters AnyQuantity<>
                quantityType = quantityType.GetGenericTypeDefinition();
            }

            try
            {
                return CurrentDimensionsDictionary[quantityType];
            }
            catch (KeyNotFoundException ex)
            {
                //if key not found and quantityType is really Quantity
                //then return dimensionless Quantity

                if (quantityType.BaseType.GetGenericTypeDefinition() == typeof(DimensionlessQuantity<>))
                    return Dimensionless;
                else
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
            var m = int.Parse(mlt.Substring(m_index + 1, l_index - m_index - 1), CultureInfo.InvariantCulture);

            return m;

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

            var l = int.Parse(mlt.Substring(l_index + 1, t_index - l_index - 1), CultureInfo.InvariantCulture);

            return l;

        }

        /// <summary>
        /// Extract the Time Power from MLT string.
        /// </summary>
        /// <param name="MLT"></param>
        /// <returns></returns>
        private static int ExponentOfTime(string mlt)
        {
            var t_index = mlt.IndexOf('T');
            var t = int.Parse(mlt.Substring(t_index + 1, mlt.Length - t_index - 1), CultureInfo.InvariantCulture);

            return t;


        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mlt"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MLT")]
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
                // parsing started with numbers which will be most probably 
                //   on the format of numbers with spaces
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

                var qd = new QuantityDimension(M, L, T, I, O, N, J)
                {
                    Currency = new CurrencyDescriptor(C),
                    Digital = new DigitalDescriptor(D)
                };

                return qd;

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


        #endregion


        public static QuantityDimension Dimensionless
        {
            get
            {
                return new QuantityDimension();
            }
        }

        public bool IsDimensionless
        {
            get
            {
                if (
                    Mass.Exponent == 0 && Length.Exponent == 0 && Time.Exponent == 0 &&
                    ElectricCurrent.Exponent == 0 && Temperature.Exponent == 0 && AmountOfSubstance.Exponent == 0 &&
                    LuminousIntensity.Exponent == 0  && Currency.Exponent == 0 && Digital.Exponent == 0
                    )
                    return true;
                else
                    return false;
            }
        }



        public QuantityDimension Invert()
        {
            var qd = new QuantityDimension
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

            return qd;

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
}
