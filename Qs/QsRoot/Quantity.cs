using Qs.Types;
using QuantitySystem;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using System.Globalization;

namespace QsRoot;

public static class Quantity
{
    /// <summary>
    /// Returns the dimension of value quantity.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static QsValue Dimension(QsParameter value)
    {
        if (value.QsNativeValue is QsScalar s)
        {
            return new QsText(s.Unit.UnitDimension.ToString());
        }

        return new QsText("Works on scalar quantities");
    }

    /// <summary>
    /// Returns the name of the quantity associated with this value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static QsValue FromValue(QsParameter value)
    {
        if (value.QsNativeValue is QsScalar s)
        {
            var qt = s.Unit.QuantityType.Name;
            return new QsText(qt.Substring(0, qt.Length - 2));
        }

        return new QsText("Works on scalar quantities");
    }

    /// <summary>
    /// Resturns the name of the quantity associated with this dimension
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public static QsValue FromDimension(QsParameter dimension)
    {
        var ss = dimension.ParameterRawText;
        if (dimension.QsNativeValue is QsText qsText)
        {
            ss = qsText.Text;
        }
        var q = QuantityDimension.Parse(ss);

        var qt = QuantityDimension.GetQuantityTypeFrom(q).Name;
        return new QsText(qt.Substring(0, qt.Length - 2));
    }

    /// <summary>
    /// Returns a value from the dimension of this quantity.
    /// </summary>
    /// <param name="dimension"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static QsValue FromDimension(QsParameter dimension, QsParameter value)
    {
        var ss = dimension.ParameterRawText;
        if (dimension.QsNativeValue is QsText qsText)
        {
            ss = qsText.Text;
        }
        var q = QuantityDimension.Parse(ss);

        var unit = Unit.DiscoverUnit(q);
        var qval = unit.GetThisUnitQuantity(double.Parse(value.ParameterRawText,  CultureInfo.InvariantCulture));

        return new QsScalar(ScalarTypes.NumericalQuantity)
        {
            NumericalQuantity = qval
        };
    }

    public static QsValue FromName(QsParameter name, QsParameter value)
    {
        var ss = name.ParameterRawText;
        if (name.QsNativeValue is QsText qsText)
        {
            ss = qsText.Text;
        }

        var qval = AnyQuantity<double>.Parse(ss);
        qval.Unit = Unit.DiscoverUnit(qval);
        qval.Value = double.Parse(value.ParameterRawText, CultureInfo.InvariantCulture);

        return new QsScalar(ScalarTypes.NumericalQuantity)
        {
            NumericalQuantity = qval
        };
    }

    public static QsValue Parse(string value) => QsValue.ParseScalar(value);

    public static QsValue InvertedDimension(QsParameter value)
    {
        if (value.QsNativeValue is not QsScalar s)
        {
            return new QsText("Works on scalar quantities");
        }

        var invertedDimension = s.Unit.UnitDimension.Invert();
        return new QsText(invertedDimension.ToString());
    }

    public static QsValue Name(QsParameter value)
    {
        if (value.QsNativeValue is QsScalar s)
        {
            return new QsText(s.Unit.QuantityType.Name.TrimEnd('`','1'));
        }

        return new QsText("Works on scalar quantities");
    }

    public static QsValue InvertedQuantityName(QsParameter value)
    {
        if (value.QsNativeValue is not QsScalar s)
        {
            return new QsText("Works on scalar quantities");
        }

        var invertedDimension = s.Unit.UnitDimension.Invert();
        var qp = QsParameter.MakeParameter(null, invertedDimension.ToString());
        return FromDimension(qp);
    }
}