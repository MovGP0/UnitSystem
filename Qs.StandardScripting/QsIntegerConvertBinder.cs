using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.Scripting.Runtime;
using Qs.Types;

namespace Qs.Scripting;

public sealed class QsIntegerConvertBinder() : ConvertBinder(typeof(int), false)
{
    public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
    {
        if (target.Value is QsValue)
        {
            var v = (QsValue)target.Value;
            var rvalue = 0;

            if (v is QsScalar) rvalue = (int)((QsScalar)v).NumericalQuantity.Value;

            if (v is QsVector) rvalue = (int)((QsVector)v)[0].NumericalQuantity.Value;

            if (v is QsMatrix) rvalue = (int)((QsMatrix)v)[0, 0].NumericalQuantity.Value;

            return target.Clone(Expression.Constant(rvalue, typeof(int)));
        }
        else
        {
            return target.Clone(Expression.Constant(0, typeof(int)));
        }
    }
}