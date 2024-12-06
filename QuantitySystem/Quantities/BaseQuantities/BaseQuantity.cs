namespace QuantitySystem.Quantities.BaseQuantities;

public abstract class BaseQuantity(float exponent)
{


    #region Construction

    private float _Exponent = exponent;

    public float Exponent => _Exponent;

    internal void SetExponent(float exp)
    {
        _Exponent = exp;
    }

    /// <summary>
    /// make 1/x operation.
    /// </summary>
    public virtual BaseQuantity Invert()
    {
        var bq = (BaseQuantity)MemberwiseClone();
        bq.SetExponent(0 - _Exponent);


        return bq;
    }

    #endregion

    #region M L T  processing

    public virtual QuantityDimension Dimension => new();

    #endregion




    #region Dimension Equality Algorithm

    /// <summary>
    /// Provides the Dimensional Equality algorithm.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        var bd = obj as BaseQuantity;

        if(bd!=null)
        {

            if (Dimension.IsDimensionless & bd.Dimension.IsDimensionless)
            {
                //why I've tested dimensioless in begining??
                //   because I want special dimensionless quantities like angle and solid angle to be treated
                //   as normal dimensionless values

                return true;
            }

            return Dimension.Equals(bd.Dimension);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return Dimension.GetHashCode();
    }

    #endregion



    /// <summary>
    /// Holds the internal quantity types of specific strongly typed quantity type.
    /// </summary>
    static Dictionary<Type, Tuple<Type, float>[]> QuantityTypeInternalQuantityTypes = new();

    public static Tuple<Type, float>[] GetInternalQuantities(Type quantity)
    {
        Tuple<Type, float>[] result;
        if (!QuantityTypeInternalQuantityTypes.TryGetValue(quantity, out result))
        {
            // not instantiated yet .. so we can instantiate it here
            Activator.CreateInstance(quantity.MakeGenericType(typeof(double)));

            return QuantityTypeInternalQuantityTypes[quantity];
        }

        return result;
    }

    public static void SetInternalQuantities(Type quantity, Tuple<Type, float>[] internalQuantities)
    {
        if (!QuantityTypeInternalQuantityTypes.ContainsKey(quantity))
            QuantityTypeInternalQuantityTypes[quantity] = internalQuantities;
    }

}