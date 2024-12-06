using Qs.Runtime;

namespace Qs.Types;

/// <summary>
/// Because I want to support passing function as a delegate to the function.
/// </summary>
public class QsParameter
{
    /// <summary>
    /// The parameter representation as a C# object
    /// </summary>
    public object? ParameterValue { get; private set; }

    /// <summary>
    /// Original value before evaluation.
    /// </summary>
    public string ParameterRawText { get; private set; }

    /// <summary>
    /// Namespace part in parameter text
    /// </summary>
    public string NamespaceName
    {
        get
        {
            string[] rv = ParameterRawText.Split(':');
            if (rv.Length == 2)
                return rv[0];
            return "";

        }
    }

    /// <summary>
    /// Variable Name or VariableName in namespace part in original parameter text
    /// </summary>
    public string NamespaceVariableName
    {
        get
        {
            string[] rv = ParameterRawText.Split(':');
            if (rv.Length == 2)
                return rv[1];
            return rv[0];
        }
    }

    public static QsParameter MakeParameter(object? value, string rawValue)
    {
        return new()
        {
            ParameterValue = value,
            ParameterRawText = rawValue
        };
    }

    /// <summary>
    /// The parameter value as a native qs value or null if fail.
    /// </summary>
    public QsValue? QsNativeValue => ParameterValue as QsValue;

    /// <summary>
    /// Get the quantity from the parameter body on the form  ([namespace:]variable)  x:var or var
    /// </summary>
    public QsValue GetIndirectQuantity(QsScope scope)
    {
        try
        {
            var q = QsEvaluator.GetScopeQsValue(scope, NamespaceName, NamespaceVariableName);
            return q;
        }
        catch (QsVariableNotFoundException e)
        {
            // add extra data to the exception about the parameter name itself.
            //  its like accumulating information on the same exception.

            e.ExtraData = ParameterRawText;

            // and throw it again
            throw e;
        }
    }


    /// <summary>
    /// if there are a value other than string then return true.
    /// </summary>
    public bool IsQsValue
    {
        get
        {
            if (ParameterValue is QsValue) return true;
            return false;
        }
    }

    /// <summary>
    /// Get the parameter value string if exist or the parameter body itself
    /// </summary>
    public string? UnknownValueText =>
        ParameterValue != null
            ? ParameterValue.ToString()
            : ParameterRawText;

    /// <summary>
    /// if we consider the <see cref="ParameterRawText"/> as a function name.
    /// then this function will get the actual function name which include parameters count.
    /// This function is only used in making expressions.
    /// </summary>
    /// <param name="paramCount"></param>
    /// <returns></returns>
    public string GetTrueFunctionName(int paramCount)
    {
        return QsFunction.FormFunctionSymbolicName(ParameterRawText, paramCount);
    }

}