using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.Scripting.Runtime;
using Qs.Types;

namespace Qs.Scripting;

public sealed class QsIntegerConvertBinder() : ConvertBinder(typeof(int), false)
{
    public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
    {
        var value = target.Value switch
        {
            QsScalar scalar => (int)scalar.NumericalQuantity.Value,
            QsVector vector => (int)vector[0].NumericalQuantity.Value,
            QsMatrix matrix => (int)matrix[0, 0].NumericalQuantity.Value,
            _ => 0
        };

        return target.Clone(Expression.Constant(value, typeof(int)));
    }
}
