using Qs.Types;
using Qs;
using System.Diagnostics.Contracts;

namespace QsRoot;

public static class Vector
{
    /// <summary>
    /// Returns <see cref="QsVector"/> from to...
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static QsValue Range(QsParameter from, QsParameter to)
    {
        Contract.Requires(from.QsNativeValue is QsScalar);
        Contract.Requires(to.QsNativeValue is QsScalar);

        if (from.QsNativeValue is not QsScalar fromScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(from));
        }

        if (to.QsNativeValue is not QsScalar toScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(to));
        }

        var fd = fromScalar.NumericalQuantity.Value;
        var td = ((QsScalar)to.QsNativeValue).NumericalQuantity.Value;

        var vec = new QsVector();

        if (td >= fd)
        {
            for (var vl = fd; vl <= td; vl++)
            {
                vec.AddComponent(vl);
            }
        }
        else
        {
            for (var vl = fd; vl >= td; vl--)
            {
                vec.AddComponent(vl);
            }

        }

        return vec;
    }

    /// <summary>
    /// Returns <see cref="QsVector"/> from to...  with specified interval
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static QsValue Range(QsParameter from, QsParameter to, QsParameter step)
    {
        Contract.Requires(from.QsNativeValue is QsScalar);
        Contract.Requires(to.QsNativeValue is QsScalar);
        Contract.Requires(step.QsNativeValue is QsScalar);
            
        if (from.QsNativeValue is not QsScalar fromScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(from));
        }
            
        if (to.QsNativeValue is not QsScalar toScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(to));
        }

        if (step.QsNativeValue is not QsScalar stepScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(step));
        }

        var fd = fromScalar.NumericalQuantity.Value;
        var td = toScalar.NumericalQuantity.Value;
        var stepd = stepScalar.NumericalQuantity.Value;

        var vec = new QsVector();

        if (td >= fd)
        {
            for (var vl = fd; vl <= td; vl += stepd)
            {
                vec.AddComponent(vl);
            }
        }
        else
        {
            for (var vl = fd; vl >= td; vl -= stepd)
            {
                vec.AddComponent(vl);
            }
        }

        return vec;
    }

    /// <summary>
    /// Returns the count of vector
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static QsValue Count(QsParameter vector)
    {
        Contract.Requires(vector.QsNativeValue is QsVector);

        if (vector.QsNativeValue is not QsVector qsVector)
        {
            throw new ArgumentException("QsNativeValue is not QsVector", nameof(vector));
        }

        return qsVector.Count.ToScalarValue();
    }

    /// <summary>
    /// Returns <see cref="QsVector"/> with constant value
    /// </summary>
    /// <param name="count"></param>
    /// <param name="constant"></param>
    /// <returns></returns>
    public static QsValue ConstantRange(QsParameter count, QsParameter constant)
    {
        Contract.Requires(count.QsNativeValue is QsScalar);
        Contract.Requires(constant.QsNativeValue is QsScalar);

        if (count.QsNativeValue is not QsScalar countScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(count));
        }
            
        if (constant.QsNativeValue is not QsScalar constantScalar)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(constant));
        }
            
        var countd = countScalar.NumericalQuantity.Value;

        var icount = (int)countd;

        var v = new QsVector(icount);

        for (var i = 0; i < icount; i++)
        {
            v.AddComponent(constantScalar);
        }

        return v;
    }
}