﻿using QuantitySystem.Units;
using System.Linq.Expressions;

namespace QuantitySystem.Quantities.BaseQuantities;

/// <summary>
/// This class hold the mathmatical operations of quantity.
/// </summary>
public abstract partial class AnyQuantity<T>(float exponent) : BaseQuantity(exponent) 
{
    #region constructors

    protected AnyQuantity() : this(1) { }

    #endregion

    #region Value & Unit

    public T Value { get; set; }

    public Unit? Unit { get; set; }

    public override string ToString()
    {
        var qname = GetType().Name;
        qname = qname[..^2];

        return qname + ": " + Value + " " + (Unit is not null ? Unit.Symbol : "");
    }

    /// <summary>
    /// Text represent the unit part.
    /// </summary>
    public string UnitText
    {
        get
        {
            var un = string.Empty;
            if (Unit is not null)
            {
                un = Unit.Symbol.Trim();

                if (un[0] != '<') un = "<" + un + ">";
            }
            return un;
        }
    }

    public string ToShortString() => Value + UnitText;

    #endregion

    public static DerivedQuantity<T> ConstructDerivedQuantity<T>(params AnyQuantity<T>[] quantities) => new(1, quantities);

    #region Generic Helper Calculations

    /// <summary>
    /// Multiply scalar value by generic values
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    /// <param name="factor"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Q MultiplyScalarByGeneric<Q>(double factor, Q value)
    {
        if (factor == 1.0)
        {
            return value;  // return the same value
        }

        var expr = Expression.Multiply(Expression.Constant(factor), Expression.Constant(value));

        // Construct Lambda function which return one object.
        Expression<Func<Q>> cq = Expression.Lambda<Func<Q>>(expr);

        // compile the function
        Func<Q> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    public static T DivideScalarByGeneric(double factor, T value)
    {
        if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
        {
            return (T)(object)(factor / (double)(object)value);
        }

        var expr = Expression.Divide(Expression.Constant(factor), Expression.Constant(value));

        // Construct Lambda function which return one object.
        Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

        // compile the function
        Func<T> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    public static T MultiplyGenericByGeneric(T firstValue, T secondValue)
    {
        var expr = Expression.Multiply(Expression.Constant(firstValue), Expression.Constant(secondValue));

        // Construct Lambda function which return one object.
        Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

        // compile the function
        Func<T> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    public static T DivideGenericByGeneric(T firstValue, T secondValue)
    {
        var expr = Expression.Divide(Expression.Constant(firstValue), Expression.Constant(secondValue));

        // Construct Lambda function which return one object.
        Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

        // compile the function
        Func<T> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static T RaiseGenericByScalar(T value, double factor)
    {
        if (typeof(T) == typeof(decimal) || typeof(T) == typeof(double) || typeof(T) == typeof(float) || typeof(T) == typeof(int) || typeof(T) == typeof(short))
        {
            return (T)(object)Math.Pow((double)(object)value, factor);

        }

        var m = typeof(T).GetMethod("Pow", [typeof(T), typeof(double)]);
        var expr = Expression.Power(Expression.Constant(value), Expression.Constant(factor), m );

        // Construct Lambda function which return one object.
        Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

        // compile the function
        Func<T> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    /// <summary>
    /// Raise power of generic to generic
    /// </summary>
    /// <param name="value"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static T RaiseGenericByGeneric(T value, T factor)
    {
        var expr = Expression.Power(Expression.Constant(value), Expression.Constant(factor));

        // Construct Lambda function which return one object.
        Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

        // compile the function
        Func<T> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    /// <summary>
    /// Remainder of two generic objects  a % b
    /// </summary>
    /// <param name="firstValue"></param>
    /// <param name="secondValue"></param>
    /// <returns></returns>
    public static T ModuloGenericByGeneric(T firstValue, T secondValue)
    {
        var expr = Expression.Modulo(Expression.Constant(firstValue), Expression.Constant(secondValue));

        // Construct Lambda function which return one object.
        Expression<Func<T>> cq = Expression.Lambda<Func<T>>(expr);

        // compile the function
        Func<T> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }

    #endregion

    public override BaseQuantity Invert()
    {
        AnyQuantity<T> q = (AnyQuantity<T>)base.Invert();

        q.Value = DivideScalarByGeneric(1.0, q.Value);

        if (q.Unit is not null)
        {
            q.Unit = Unit.Invert();
        }

        return q;
    }

    /// <summary>
    /// Parse the input name and return the quantity object from it.
    /// </summary>
    /// <typeparam name="T">container type of the value</typeparam>
    /// <param name="quantityName"></param>
    /// <returns></returns>
    public static AnyQuantity<T> Parse(string quantityName)
    {
        var quantityType = QuantityDimension.QuantityTypeFrom(quantityName);

        if (quantityType == null)
        {
            throw new QuantityNotFoundException();
        }
        else
        {

            //QuantityType = QuantityType.MakeGenericType(typeof(T));
            //AnyQuantity<T> qty = (AnyQuantity<T>)Activator.CreateInstance(QuantityType);

            AnyQuantity<T> qty = QuantityDimension.QuantityFrom<T>(QuantityDimension.DimensionFrom(quantityType));
            return qty;
        }

    }


    #region ICloneable Members

    public AnyQuantity<T> Clone()
    {
        var t = MemberwiseClone();
        var t2 = (AnyQuantity<T>)t;
        if (t2.Unit != null) t2.Unit = (Unit)Unit.Clone();
        return t2;
    }

    #endregion
}