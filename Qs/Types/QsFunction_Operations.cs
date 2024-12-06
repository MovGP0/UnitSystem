using Qs.Runtime;
using ParticleLexer;
using ParticleLexer.StandardTokens;
//using Microsoft.Scripting.Ast;
using SymbolicAlgebra;
using QuantitySystem.Quantities.BaseQuantities;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Qs.Types;

/// <summary>
/// Function that declared in Qs
/// </summary>
public partial class QsFunction
{

    /// <summary>
    /// if you give it x,y,x,x,y,y will return string of "x,y"
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    internal static string RemoveRedundantParameters(params string[] parameters)
    {
        Dictionary<string, bool> rp = new(StringComparer.OrdinalIgnoreCase);
        foreach (var prm in parameters)
        {
            if (!rp.ContainsKey(prm))
                rp[prm] = true;
        }

        var all = string.Empty;
        foreach (var k in rp.Keys)
            all += "," + k;

        return all.TrimStart(',');
    }

    public override QsValue Identity => throw new NotImplementedException();


    /// <summary>
    /// Join any set of given functions and returns the tokens of the new function along
    /// with the new function text.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="functionDeclaration"></param>
    /// <param name="functions"></param>
    /// <returns></returns>
    internal static Token JoinFunctionsArrayTokensWithOperation(string operation, out string functionDeclaration, params QsFunction[] functions)
    {
        var fname = "_";

        var fbody = "";
        List<string> fparamList = [];
        foreach (var f in functions)
        {
            fbody += " " + f.FunctionBody + " " + operation;
            fparamList.AddRange(f.ParametersNames);
        }
        fbody = fbody.TrimEnd(operation.ToCharArray());

        string[] fparam = fparamList.ToArray();

        var fNameParamPart = fname + "(" + RemoveRedundantParameters(fparam) + ") =";
        functionDeclaration = fNameParamPart + fbody;

        var fTokens = new Token();

        // add first part tokens
        foreach (var t in TokenizeFunction(fNameParamPart))
        {
            fTokens.AppendSubToken(t);
        }

        for (var i = 0; i < functions.Length; i++ )
        {
            // prepare function body tokens
            foreach (var t in functions[i].FunctionBodyToken)
            {
                fTokens.AppendSubToken(t);
            }

            if (i < functions.Length - 1) fTokens.AppendSubToken(Token.ParseText(operation.Trim())[0]);  //operation is only one charachter
        }


        return fTokens;
    }


    internal static Token JoinFunctionsBodiesTokensWithOperation(string operation, params QsFunction[] functions)
    {

        var fbody = "";
        foreach (var f in functions)
        {
            fbody += " " + f.FunctionBody + " " + operation;
        }

        fbody = fbody.TrimEnd(operation.ToCharArray());


        var fTokens = new Token();

        for (var i = 0; i < functions.Length; i++)
        {
            // prepare function body tokens
            foreach (var t in functions[i].FunctionBodyToken)
            {
                fTokens.AppendSubToken(t);
            }

            if (i < functions.Length - 1) fTokens.AppendSubToken(Token.ParseText(operation.Trim())[0]);  //operation is only one charachter
        }


        return fTokens;
    }


    /// <summary>
    /// Take the value and operation in text and return the current function operationed by value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private QsFunction FOperation(QsValue value, string operation)
    {
        if (value is QsFunction)
        {
            var fn2 = (QsFunction)value;

            var fParameters = RemoveRedundantParameters(ParametersNames.Union(fn2.ParametersNames).ToArray());

            var thisFunctionBody  = FunctionBody;
            foreach(var p in ParametersNames)
            {
                thisFunctionBody = thisFunctionBody.Replace(p, "$" + p);
            }

            var targetFunctionBody  = fn2.FunctionBody;
            foreach(var p in fn2.ParametersNames)
            {
                targetFunctionBody = targetFunctionBody.Replace(p, "$" + p);
            }

            // form the expressions that will be parsed.
            var fpt = "(" + thisFunctionBody + ")" + operation + "(" + targetFunctionBody + ")";

            fpt = fpt.Replace("!", "__FAC__"); // replacing the ! sign with __FAC__ to include the '!' sign into the calculations {because '!' is operator in parsing so it doesn't enter the algebraic calculations}
            //evaulate fpt

            try
            {
                var sc = (QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(fpt);
                var FuncBody = sc.SymbolicQuantity.Value.ToString().Replace("__FAC__", "!");
                var WholeFunction = "_(" + fParameters + ") = " + FuncBody;
                return ParseFunction(QsEvaluator.CurrentEvaluator, WholeFunction);
            }
            catch(QsIncompleteExpression)
            {
                // something happened make the operation in old fashion
                string WholeFunction;
                var FunctionToken = JoinFunctionsArrayTokensWithOperation(operation, out WholeFunction,this,fn2);
                return ParseFunction(QsEvaluator.CurrentEvaluator, WholeFunction, FunctionToken);
            }
        }

        if (value is QsScalar)
        {
            var svl = (QsScalar)value;

            var fname = "_";

            var fbody = FunctionBody;
            if (svl.ScalarType == ScalarTypes.SymbolicQuantity) fbody = "(" + fbody + ")" + operation + "(" + svl.SymbolicQuantity.Value + ")";
            else if (svl.ScalarType == ScalarTypes.NumericalQuantity) fbody = "(" + fbody + ")" + operation + svl.NumericalQuantity.Value;
            else if (svl.ScalarType == ScalarTypes.RationalNumberQuantity) fbody = "(" + fbody + ")" + operation + svl.RationalQuantity.Value.Value;
            else if (svl.ScalarType == ScalarTypes.FunctionQuantity) fbody = "(" + fbody + ")" + operation + "(" + svl.FunctionQuantity.Value.FunctionBody + ")";
            else throw new QsException("Operation '" + operation + "' for the target scalar type (" + svl.ScalarType + ") is not supported");


            var fb = SymbolicVariable.Parse(fbody).ToQuantity().ToScalar();

            var FuncBody = string.Empty;
            if(fb.ScalarType == ScalarTypes.SymbolicQuantity)
                FuncBody = fb.SymbolicQuantity.Value.ToString().Replace("__FAC__", "!");
            else
                FuncBody = fb.ToExpressionParsableString();

            string[] functionParametersArray = ParametersNames; // this is the available parameters for the original function.

            if (svl.ScalarType == ScalarTypes.SymbolicQuantity)
            {

                List<string> newParametersList = [..functionParametersArray];

                newParametersList.AddRange(svl.SymbolicQuantity.Value.InvolvedSymbols);

                functionParametersArray = newParametersList.ToArray();
            }

            if (svl.ScalarType == ScalarTypes.FunctionQuantity)
            {

                List<string> newParametersList = [..functionParametersArray];

                newParametersList.AddRange(svl.FunctionQuantity.Value.ParametersNames);

                functionParametersArray = newParametersList.ToArray();
            }

            var f = fname + "(" + RemoveRedundantParameters(functionParametersArray) + ") = " + FuncBody;
            return ParseFunction(QsEvaluator.CurrentEvaluator, f);
        }
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the body of the function as a symbolic quantities.
    /// </summary>
    public string SymbolicBodyText
    {
        get
        {
            var fbody = FunctionBody;
            foreach (var p in ParametersNames)
            {
                fbody = fbody.Replace(p, "$" + p);
            }

            fbody = fbody.Replace("!", "__FAC__");

            return "(" + fbody + ")";
        }
    }


    public SymbolicVariable ToSymbolicVariable()
    {
        return SymbolicVariable.Parse(FunctionBody);
    }

    public AnyQuantity<SymbolicVariable> ToSymbolicQuantity()
    {

        var fv = SymbolicVariable.Parse(FunctionBody);
        return fv.ToQuantity();


    }

    /// <summary>
    /// Returns the function body as symbolic quantity scalar.
    /// </summary>
    public QsScalar ToSymbolicScalar()
    {

        //return (QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(SymbolicBodyText);
        var fv = SymbolicVariable.Parse(FunctionBody);
        return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = fv.ToQuantity() };

    }


    internal static string FormFunctionHeadFromFunctions(string functionName, params QsFunction[] functions)
    {
        var fname = functionName;

        List<string> fparamList = [];
        foreach (var f in functions)
        {
            fparamList.AddRange(f.ParametersNames);
        }

        string[] fparam = fparamList.ToArray();

        var fNameParamPart = fname + "(" + RemoveRedundantParameters(fparam) + ")";

        return fNameParamPart;
    }

    public override QsValue AddOperation(QsValue value)
    {
        return FOperation(value, Operator.Plus);
    }

    public override QsValue SubtractOperation(QsValue value)
    {
        return FOperation(value, Operator.Minus);
    }

    public override QsValue MultiplyOperation(QsValue value)
    {

        return FOperation(value, Operator.Multiply);
    }

    public override QsValue DivideOperation(QsValue value)
    {
        return FOperation(value, Operator.Divide);
    }

    public override QsValue PowerOperation(QsValue value)
    {
        return FOperation(value, Operator.Power);
    }

    public override QsValue ModuloOperation(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue LeftShiftOperation(QsValue times)
    {
        throw new NotImplementedException();
    }

    public override QsValue RightShiftOperation(QsValue times)
    {
        throw new NotImplementedException();
    }

    public override bool LessThan(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool GreaterThan(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool LessThanOrEqual(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool GreaterThanOrEqual(QsValue value)
    {
        throw new NotImplementedException();
    }
    public static bool operator ==(QsFunction a, QsFunction b)
    {
        // If both are null, or both are same instance, return true.
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if ((a is null) || (b is null))
        {
            return false;
        }

        return a.Equality(b);
    }

    public static bool operator !=(QsFunction a, QsFunction b)
    {
        return !(a == b);
    }


    static PropertyInfo ExpressionDebugView = typeof(Expression).GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);

    public override bool Equality(QsValue value)
    {
        if(value is QsFunction func)
        {
            if (ReferenceEquals(func, this)) return true;

            var thisDebugView = ExpressionDebugView.GetValue(_FunctionExpression.Body) as string;
            var funcDebugView = ExpressionDebugView.GetValue(func._FunctionExpression.Body) as string;

            return thisDebugView.Equals(funcDebugView);

        }
        return false;
    }

    public override bool Inequality(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue DotProductOperation(QsValue value)
    {
        return FOperation(value, Operator.DotProduct);
    }

    public override QsValue CrossProductOperation(QsValue value)
    {
        return FOperation(value, Operator.CrossProduct);
    }

    public override QsValue TensorProductOperation(QsValue value)
    {
        return FOperation(value, Operator.TensorProduct);
    }

    public override QsValue NormOperation()
    {
        throw new NotImplementedException();
    }

    public override QsValue AbsOperation()
    {
        throw new NotImplementedException();
    }

    public override QsValue GetIndexedItem(QsParameter[] indices)
    {
        throw new NotImplementedException();
    }

    public override void SetIndexedItem(QsParameter[] indices, QsValue value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Differentiate operation for function.
    /// </summary>
    /// <param name="value">object of <see cref="QsScalar"/> that hold <see cref="AnyQuantity&lt;SymbolicVariable&gt;"/></param>
    /// <returns></returns>
    public override QsValue DifferentiateOperation(QsValue value)
    {
        var sval = (QsScalar)value;

        if (sval.ScalarType == ScalarTypes.SymbolicQuantity)
        {
            var dsv = sval.SymbolicQuantity.Value;

            var fname = "_";
            var WholeFunction = string.Empty;
            if (FunctionBodyToken[0].TokenClassType == typeof(CurlyBracketGroupToken))
            {
                // vector differentiation
                // take every term in the vector and differentiate it
                var vcs = QsVar.VectorComponents(FunctionBodyToken[0]);
                var sc = new StringBuilder();
                sc.Append(fname + "(" + RemoveRedundantParameters(ParametersNames) + ") = ");
                sc.Append("{ ");
                foreach (var c in vcs)
                {
                    var nsv = SymbolicVariable.Parse(c);
                    var times = (int)dsv.SymbolPower;
                    while (times > 0)
                    {
                        nsv = nsv.Differentiate(dsv.Symbol);
                        times--;
                    }
                    sc.Append(nsv);
                    sc.Append(" ");
                }
                sc.Append("}");

                WholeFunction = sc.ToString();
            }
            else
            {
                var nsv = ToSymbolicVariable();
                var times = (int)dsv.SymbolPower;
                while (times > 0)
                {
                    nsv = nsv.Differentiate(dsv.Symbol);
                    times--;
                }

                WholeFunction = fname + "(" + RemoveRedundantParameters(ParametersNames) + ") = " + nsv;
            }


            return ParseFunction(QsEvaluator.CurrentEvaluator, WholeFunction);
        }

        return base.DifferentiateOperation(value);
    }
}