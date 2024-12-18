﻿using System.Globalization;
using Qs.Types;


namespace Qs.Runtime;

public partial class QsSequence
{

    /// <summary>
    /// Take the average of the sequence.
    /// Corresponds To: S[i!!k]
    /// </summary>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <returns></returns>
    public QsValue Average(int fromIndex, int toIndex)
    {
 
        var n = toIndex - fromIndex + 1;
        if (Parameters.Length > 0)
        {
            // this is a call to form symbolic element
            // like g[n](x) ..> x^n
            /// and calling g[0++2]                //  the output should be x^0 + x^1 + x^2
            //  and be parsed into function  (QsFunction)


            var porma = string.Empty;  // the parameters separated by comma ','
            foreach (var prm in Parameters)
            {
                porma += prm.Name + ", ";
            }
            porma = porma.TrimEnd(',', ' ');

            var FunctionBody = "(" + JoinElementsWithOperation(fromIndex, toIndex, "+") + ")/" + n.ToString(CultureInfo.InvariantCulture);

            var FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;

            var qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);
            return qs;
        }

        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex);

        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }



    #region Average Functions
    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }


    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0, arg1);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }
    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }
    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }
    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }
    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }
    public QsValue Average(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5, QsParameter arg6)
    {
 
        FixIndices(ref fromIndex, ref toIndex);

        var tot = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        var n = toIndex - fromIndex + 1;
        var count = new QsScalar { NumericalQuantity = Qs.ToQuantity((double)n) };
        return tot / count;
    }

    #endregion


}