using System.Linq.Expressions;
using System.Diagnostics.Contracts;

namespace Qs;

/// <summary>
/// Because Lambda Expression requires that everything should be prepared this class helps in making the expression later
/// after completing all required fields
/// </summary>
class SimpleLambdaBuilder(string functionName, Type type)
{
    private List<ParameterExpression> _params = [];
    private readonly List<KeyValuePair<ParameterExpression, bool>> _visibleVars = [];
    private Expression _body;

    private static int _lambdaId; //for generating unique lambda name


    // TODO: Complete member initialization

    /// <summary>
    /// The body of the lambda. This must be non-null.
    /// </summary>
    public Expression Body
    {
        get
        {
            return _body;
        }
        set
        {

            _body = value;
        }
    }

    /// <summary>
    /// Creates a parameter on the lambda with a given name and type.
    /// 
    /// Parameters maintain the order in which they are created,
    /// however custom ordering is possible via direct access to
    /// Parameters collection.
    /// </summary>
    public ParameterExpression Parameter(Type type, string name)
    {
        Contract.Requires(type != null);

        var result = Expression.Parameter(type, name);
        _params.Add(result);
        _visibleVars.Add(new KeyValuePair<ParameterExpression, bool>(result, false));
        return result;
    }

    /// <summary>
    /// List of lambda's parameters for direct manipulation
    /// </summary>
    public List<ParameterExpression> Parameters
    {
        get
        {
            return _params;
        }
    }



    private static Type GetLambdaType(Type returnType, IList<ParameterExpression> parameterList)
    {
        Contract.Requires(returnType != null);

        var action = returnType == typeof(void);
        var paramCount = parameterList == null ? 0 : parameterList.Count;

        Type[] typeArgs = new Type[paramCount + (action ? 0 : 1)];
        for (var i = 0; i < paramCount; i++)
        {
            Contract.Requires(parameterList[i]!=null);
            typeArgs[i] = parameterList[i].Type;
        }

        Type delegateType;
        if (action)
            delegateType = Expression.GetActionType(typeArgs);
        else
        {
            typeArgs[paramCount] = returnType;
            delegateType = Expression.GetFuncType(typeArgs);
        }
        return delegateType;
    }


    /// <summary>
    /// Creates the LambdaExpression from the builder.
    /// After this operation, the builder can no longer be used to create other instances.
    /// </summary>
    /// <returns>New LambdaExpression instance.</returns>
    public LambdaExpression MakeLambda()
    {

        var lambda = Expression.Lambda(
            GetLambdaType(type, _params),
            _body,
            functionName + "$" + Interlocked.Increment(ref _lambdaId),
            _params
        );

        return lambda;
    }



    internal static SimpleLambdaBuilder Create(Type type, string functionName)
    {
        return new SimpleLambdaBuilder(functionName, type);
    }

}