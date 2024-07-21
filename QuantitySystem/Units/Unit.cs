﻿using QuantitySystem.Quantities.BaseQuantities;

using System.Reflection;
using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units;

public partial class Unit
{
    #region Fields
        
    protected string _Symbol;

    protected bool _IsDefaultUnit;
    private readonly bool _IsBaseUnit;


    protected Type _QuantityType;
    protected QuantityDimension _UnitDimension;



        
    //the reference unit information.
    protected readonly Unit _ReferenceUnit;

    protected readonly double _ReferenceUnitNumerator;
    protected readonly double _ReferenceUnitDenominator;



    private readonly bool _IsStronglyTyped = false;

    #endregion

    struct UnitValues
    {
        public string _Symbol;

        public bool _IsDefaultUnit;
        public bool _IsBaseUnit;


        public Type _QuantityType;
        public QuantityDimension _UnitDimension;


        public Unit _ReferenceUnit;

        public double _ReferenceUnitNumerator;
        public double _ReferenceUnitDenominator;

    }

    static Dictionary<Type, UnitValues> CachedUnitsValues = new Dictionary<Type, UnitValues>();


    /// <summary>
    /// Fill the instance of the unit with the attributes
    /// found on it.
    /// </summary>
    protected Unit()
    {
            //only called on the strongly typed units
            _IsStronglyTyped = true;            
            
            UnitValues uv;

            lock (CachedUnitsValues)
            {
                if (CachedUnitsValues.TryGetValue(GetType(), out uv))
                {
                    _Symbol = uv._Symbol;
                    _QuantityType = uv._QuantityType;
                    _UnitDimension = uv._UnitDimension;
                    _IsDefaultUnit = uv._IsDefaultUnit;
                    _IsBaseUnit = uv._IsBaseUnit;
                    _ReferenceUnit = uv._ReferenceUnit;
                    _ReferenceUnitNumerator = uv._ReferenceUnitNumerator;
                    _ReferenceUnitDenominator = uv._ReferenceUnitDenominator;
                }
                else
                {
                    //read the current attributes

                    MemberInfo info = GetType();

                    object[] attributes = (object[])info.GetCustomAttributes(true);

                    //get the UnitAttribute
                    var ua = (UnitAttribute)attributes.SingleOrDefault<object>(ut => ut is UnitAttribute);

                    if (ua != null)
                    {
                        _Symbol = ua.Symbol;
                        _QuantityType = ua.QuantityType;
                        _UnitDimension = QuantityDimension.DimensionFrom(_QuantityType);


                        if (ua is DefaultUnitAttribute)
                        {
                            _IsDefaultUnit = true;  //indicates that this unit is the default when creating the quantity in this system
                                                    //also default unit is the unit that relate its self to the SI Unit.
                        }
                        else
                        {
                            _IsDefaultUnit = false;
                        }
                    }
                    else
                    {
                        throw new UnitException("Unit Attribute not found");
                    }

                    if (_QuantityType.Namespace == "QuantitySystem.Quantities.BaseQuantities")
                    {
                        _IsBaseUnit = true;
                    }
                    else
                    {
                        _IsBaseUnit = false;
                    }

                    //Get the reference attribute
                    var dua = (ReferenceUnitAttribute)attributes.SingleOrDefault<object>(ut => ut is ReferenceUnitAttribute);

                    if (dua != null)
                    {
                        if (dua.UnitType != null)
                        {
                            _ReferenceUnit = (Unit)Activator.CreateInstance(dua.UnitType);
                        }
                        else
                        {
                            //get the SI Unit Type for this quantity
                            //first search for direct mapping
                            var SIUnitType = GetDefaultSIUnitTypeOf(_QuantityType);
                            if (SIUnitType != null)
                            {
                                _ReferenceUnit = (Unit)Activator.CreateInstance(SIUnitType);
                            }
                            else
                            {
                                //try dynamic creation of the unit.
                                _ReferenceUnit = new Unit(_QuantityType);
                            }
                        }

                        _ReferenceUnitNumerator = dua.Numerator;
                        _ReferenceUnitDenominator = dua.Denominator;

                    }


                    uv._Symbol = _Symbol;
                    uv._QuantityType = _QuantityType;
                    uv._UnitDimension = _UnitDimension;
                    uv._IsDefaultUnit = _IsDefaultUnit;
                    uv._IsBaseUnit = _IsBaseUnit;
                    uv._ReferenceUnit = _ReferenceUnit;
                    uv._ReferenceUnitNumerator = _ReferenceUnitNumerator;
                    uv._ReferenceUnitDenominator = _ReferenceUnitDenominator;


                    CachedUnitsValues.Add(GetType(), uv);

                }

            }

        }



    #region Characterisitics


        
    public virtual string Symbol
    {
        get
        {

                //symbol in strong typed are fetched from the attributes

                if (IsStronglyTyped)
                {
                    return _Symbol;
                }
                else
                {

                    return _Symbol;
                }
            }
    }

    /// <summary>
    /// Determine if the unit is the default unit for the quantity type.
    /// </summary>
    public virtual bool IsDefaultUnit
    {
        get
        {
                //based on the current unit attribute
                return _IsDefaultUnit;
            }
    }

    /// <summary>
    /// The dimension that this unit represents.
    /// </summary>
    public QuantityDimension UnitDimension
    {
        get { return _UnitDimension; }
        internal set { _UnitDimension = value; }
    }


    /// <summary>
    /// Returns a unique key for the unit based on the unit type and the representative quantity type
    /// Also m! and m  while they are the same unit but they represents Polar and Regular Lenghts respectively.
    /// </summary>
    public Tuple<Type, Type> UniqueKey => new Tuple<Type, Type>(GetType(), QuantityType);


    /// <summary>
    /// The Type of the Quantity of this unit.
    /// </summary>
    public Type QuantityType
    {
        get
        {
                return _QuantityType;
            }
        internal set
        {
                _QuantityType = value;
                _UnitDimension = QuantityDimension.DimensionFrom(value);
            }
    }

    /// <summary>
    /// Tells if Unit is related to one of the seven base quantities.
    /// </summary>
    public bool IsBaseUnit
    {
        get
        {
                return _IsBaseUnit;
            }
    }



    /// <summary>
    /// The unit that serve a parent for this unit.
    /// and should take the same exponent of the unit.
    /// </summary>
    public virtual Unit ReferenceUnit
    {
        get 
        {
                if (_ReferenceUnit != null)
                {
                    if (_ReferenceUnit.UnitExponent != UnitExponent)
                        _ReferenceUnit.UnitExponent = UnitExponent;
                }

                return _ReferenceUnit; 
            }
    }

    /// <summary>
    /// How much the current unit equal to the reference unit.
    /// </summary>
    public double ReferenceUnitTimes
    {
        get { return ReferenceUnitNumerator / ReferenceUnitDenominator; }
    }

    public virtual double ReferenceUnitNumerator
    {
        get 
        {

                return Math.Pow(_ReferenceUnitNumerator, unitExponent);
            }
    }

    public virtual double ReferenceUnitDenominator
    {
        get
        {
                return Math.Pow(_ReferenceUnitDenominator, unitExponent);
            }
    }




    #endregion

    #region Operations

    /// <summary>
    /// Invert the current unit simply from numerator to denominator and vice versa.
    /// </summary>
    /// <returns></returns>
    public Unit Invert()
    {
            Unit unit = null;
            if (SubUnits != null)
            {
                //convert sub units if this were only a generated unit.

                List<Unit> InvertedUnits = [];

                foreach (var lun in SubUnits)
                {
                    InvertedUnits.Add(lun.Invert());

                }

                unit = new Unit(QuantityType, InvertedUnits.ToArray());
                
            }
            else
            {
                //convert exponent because this is a strongly typed unit.

                unit = (Unit)MemberwiseClone();
                unit.UnitExponent = 0 - UnitExponent;
                unit.UnitDimension = unit.UnitDimension.Invert();
                
                
            }
            return unit;
        }

    #region Manipulating quantities
    //I want to get away from Activator.CreateInstance because it is very slow :)
    // so I'll cach resluts 
    static Dictionary<Type, object> Qcach = new Dictionary<Type, object>();



    /// <summary>
    /// Gets the quantity of this unit based on the desired container.
    /// <see cref="QuantityType"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public AnyQuantity<T> GetThisUnitQuantity<T>()
    {

            AnyQuantity<T> Quantity = null;

            var qt = QuantityType.MakeGenericType(typeof(T));
                
            object j;
            if (Qcach.TryGetValue(qt, out j))
            {
                Quantity = (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();  //optimization for created quantities before

            }
            else
            {
                Quantity = (AnyQuantity<T>)Activator.CreateInstance(QuantityType.MakeGenericType(typeof(T)));
                Qcach.Add(qt, Quantity);

            }

            Quantity.Unit = this;


            return Quantity;
        }

        
    public AnyQuantity<T> GetThisUnitQuantity<T>(T value)
    {

            AnyQuantity<T> Quantity = null;
            if (QuantityType != typeof(DerivedQuantity<>) && QuantityType != null)
            {
                var qt = QuantityType.MakeGenericType(typeof(T));
                
                object j;
                if (Qcach.TryGetValue(qt, out j))
                {

                    Quantity = (AnyQuantity<T>)((AnyQuantity<T>)j).Clone();  //optimization for created quantities before
                }
                else
                {

                    Quantity = (AnyQuantity<T>)Activator.CreateInstance(qt);

                    Qcach.Add(qt, Quantity);
                }
            }
            else
            {
                //create it from the unit dimension
                Quantity = new DerivedQuantity<T>(UnitDimension);

            }
            Quantity.Unit = this;

            Quantity.Value = value;

            if (IsOverflowed) Quantity.Value =
                 AnyQuantity<T>.MultiplyScalarByGeneric(GetUnitOverflow(), value);
            
            return Quantity;
        }

    #endregion



    #endregion


    #region Properties

    private float unitExponent = 1;

    public float UnitExponent
    {
        get
        {
                return unitExponent;
            }
        set
        {
                unitExponent = value;
                
            }
    }

    public const string MixedSystem = "MixedSystem";

    public string UnitSystem 
    { 
        get 
        { 
                //based on the current namespace of the unit
                //return the text of the namespace          // after Unit.

                if (IsStronglyTyped)
                {
                    var UnitType = GetType();

                    var ns = UnitType.Namespace[(UnitType.Namespace.LastIndexOf("Units.", StringComparison.Ordinal) + 6)..];
                    return ns;
                }
                else
                {
                    //mixed system
                    // check all sub units if there unit system is the same then
                    //  return it 

         if (SubUnits.Count > 0)
                    {
                        var ns = SubUnits[0].UnitSystem;

                        var suidx = 1;

                        while (suidx < SubUnits.Count)
                        {
                            if (SubUnits[suidx].UnitSystem != ns && SubUnits[suidx].UnitSystem != "Shared")
                            {
                                ns = MixedSystem;
                                break;
                            }
                            suidx++;
                        }

                        return ns;
                    }
                    else
                        return "Unknown";

                }
            } 
    }


    /// <summary>
    /// Determine if the unit is inverted or not.
    /// </summary>
    public bool IsInverted
    {
        get
        {
                if (UnitExponent < 0) return true;
                else
                    return false;
            }
    }

    public bool IsStronglyTyped
    {
        get
        {
                return _IsStronglyTyped;
            }
    }
    #endregion

    public string Name => GetType().Name;

    public string QuantityTypeName => QuantityType.Name;


    public override string ToString()
    {
            return Name + " " + Symbol;
        }


    #region ICloneable Members

    public Unit Clone()
    {
            return (Unit)MemberwiseClone();
        }

    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
            var u = obj as Unit;
            if(!ReferenceEquals(u, null))
            {
                if (Symbol.Equals(u.Symbol, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

    public override int GetHashCode()
    {
            return Symbol.GetHashCode();
        }

    public static  bool operator ==(Unit lhs, Unit rhs)
    {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((object)lhs == null || (object)rhs == null)
            {
                return false;
            }

            return lhs.Equals(rhs);

        }

    public static bool operator !=(Unit lhs, Unit rhs)
    {
            return !(lhs == rhs);

        }
}