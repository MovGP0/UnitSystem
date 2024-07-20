using System.Diagnostics.Contracts;
using Qs.Types;
using Qs;

namespace QsRoot;

public static partial class QsMath
{
    #region Functions

    public static QsValue? Log(QsParameter val, QsParameter newBase)
    {
        if (!val.IsQsValue)
        {
            return null;
        }

        switch (val.ParameterValue)
        {
            case QsScalar qsScalar:
            {
                var q = qsScalar.NumericalQuantity;

                if (q.Dimension.IsDimensionless)
                {
                    var r = Math.Log(q.Value, ((QsScalar?)newBase.ParameterValue).NumericalQuantity.Value);
                    return r.ToQuantity().ToScalarValue();
                }

                throw new QsInvalidInputException("Non dimensionless number");
            }

            case QsVector vec:
            {
                var rv = new QsVector(vec.Count);

                foreach (var var in vec)
                {
                    if (var.NumericalQuantity.Dimension.IsDimensionless)
                    {
                        var r = Math.Log(var.NumericalQuantity.Value, ((QsScalar?)newBase.ParameterValue).NumericalQuantity.Value);
                        rv.AddComponent(r.ToQuantity().ToScalar());
                    }
                    else
                    {
                        throw new QsInvalidInputException("Non dimensionless component");
                    }
                }

                return rv;
            }

            case QsMatrix mat:
            {
                var rm = new QsMatrix();

                foreach (var vecRow in mat.Rows)
                {
                    rm.AddVector((QsVector)Log(QsParameter.MakeParameter(vecRow, string.Empty)));
                }
                return rm;
            }

            default:
                //not known may be ordinary string
                return null;
        }
    }

    public static QsValue Atan2(QsParameter x, QsParameter y)
    {
        if(x.QsNativeValue is not QsScalar xs)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(x));
        }

        if (y.QsNativeValue is not QsScalar ys)
        {
            throw new ArgumentException("QsNativeValue is not QsScalar", nameof(y));
        }

        var r = Math.Atan2(
            ys.NumericalQuantity.Value,
            xs.NumericalQuantity.Value);

        return r.ToQuantity().ToScalarValue();
    }

    public static QsValue Sqrt(QsParameter x)
    {
        Contract.Requires(x.QsNativeValue is not null);

        if (x.QsNativeValue is null)
        {
            throw new ArgumentException("QsNativeValue was null", nameof(x));
        }

        return x.QsNativeValue.PowerOperation("0.5".ToScalar());
    }

    #endregion

    public static QsValue? Max(QsParameter value)
    {
        if (value.QsNativeValue is QsScalar qsScalar)
        {
            return qsScalar;
        }

        if (value.QsNativeValue is QsVector vector)
        {
            return vector.Max();
        }

        if (value.QsNativeValue is QsMatrix matrix)
        {
            return matrix.ToArray().Max();
        }

        throw new QsException("Not implemented for above matrix");

    }

    public static QsValue? Min(QsParameter value)
    {
        if (value.QsNativeValue is QsScalar qsScalar)
        {
            return qsScalar;
        }

        if (value.QsNativeValue is QsVector vector)
        {
            return vector.Min();
        }

        if (value.QsNativeValue is QsMatrix matrix)
        {
            return matrix.ToArray().Min();
        }

        throw new QsException("Not implemented for above matrix");
    }
}