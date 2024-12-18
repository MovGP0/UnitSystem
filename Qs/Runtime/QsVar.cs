﻿using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ParticleLexer;
using ParticleLexer.QsTokens;
using ParticleLexer.StandardTokens;
using Qs.Numerics;
using Qs.Runtime.Operators;
using Qs.Types;
using SymbolicAlgebra;
using Qs.Types.Operators;
using QsRoot;
using ParticleLexer.CommonTokens;

namespace Qs.Runtime;

class QsVar
{

    private readonly QsEvaluator evaluator;

    public QsEvaluator Evaluator => evaluator;


    public class ExprOp
    {
        public Expression QuantityExpression { get; set; }
        public string Operation { get; set; }

        public ExprOp Previous { get; set; }

        private ExprOp _Next;

        public ExprOp Next
        {
            get => _Next;
            set
            {
                _Next = value;
                if (value != null) value.Previous = this;
            }
        }
    }


    public QsVarParseModes ParseMode { get; set; }

    SimpleLambdaBuilder lambdaBuilder = null;




    private QsFunction Function;


    /// <summary>
    /// Evaluate the function body taking into considerations
    /// the parameters of the lambda function.
    /// </summary>
    /// <param name="evaluator"></param>
    /// <param name="line"></param>
    /// <param name="lb"></param>
    public QsVar(QsEvaluator evaluator, string line, QsFunction function, SimpleLambdaBuilder lb, out Token tokenizedBody)
    {

        this.evaluator = evaluator;
        if (function != null)
        {
            lambdaBuilder = lb;

            Function = function;

            ParseMode = QsVarParseModes.Function;
        }

        ResultExpression = ParseArithmatic(line, out tokenizedBody);

    }


    public static Expression ParseToExpression(QsEvaluator evaluator, string line)
    {
        var v = new QsVar(evaluator, line);
        return v.ResultExpression;
    }


    /// <summary>
    /// Evaluates Normal Calculations.
    /// </summary>
    /// <param name="evaluator"></param>
    /// <param name="line"></param>
    public QsVar(QsEvaluator evaluator, string line)
    {
        this.evaluator = evaluator;

        ResultExpression = ParseArithmatic(line);

    }

    public QsVar(QsEvaluator evaluator, Token expression)
    {
        this.evaluator = evaluator;

        ResultExpression = ParseArithmatic(expression);

    }

    private QsSequence Sequence= null;

    /// <summary>
    /// Evaulate the line for use in sequence body.
    /// </summary>
    /// <param name="evaluator"></param>
    /// <param name="line"></param>
    /// <param name="sequenceName"></param>
    public QsVar(QsEvaluator evaluator, string line, QsSequence sequence, SimpleLambdaBuilder lb)
    {
        this.evaluator = evaluator;

        if (sequence != null)
        {
            lambdaBuilder = lb;
            Sequence = sequence;
            ParseMode = QsVarParseModes.Sequence;
        }


        ResultExpression = ParseArithmatic(line);
    }


    /// <summary>
    /// Merge operators that have more than one letter.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static Token MergeOperators(Token token)
    {
        var tok = token.MergeTokens<PowerDotToken>();

        //tok = tok.MergeTokens(new PowerCrossToken()); removed

        tok = tok.MergeTokens<TensorProductToken>();

        return tok;
    }

    internal Expression ParseArithmatic(string codeLine)
    {
        Token dummy;
        return ParseArithmatic(codeLine, out dummy);
    }


    /// <summary>
    /// Takes the expression text and returns it into tokens.
    /// </summary>
    /// <param name="codeLine"></param>
    /// <returns></returns>
    public static Token TokenzieExpression(string codeLine)
    {
        var tokens = Token.ParseText(codeLine);

        tokens = tokens.TokenizeTextStrings();

        // assemble all spaces
        //tokens = tokens.MergeTokens<MultipleSpaceToken>();
        tokens = tokens.MergeRepetitiveTokens<MultipleSpaceToken, SingleSpaceToken>();

        // assemble all units <*>    //before tokenization of tensor operator
        tokens = tokens.MergeTokens<UnitToken>();

        tokens = tokens.MergeTokens<DoubleVerticalBarToken>();

        tokens = tokens.MergeMultipleWordTokens(
            // object assignment
            typeof(PointerOperatorToken),

            // assemble '<|'
            typeof(LeftTensorToken),

            // assemble '|>'
            typeof(RightTensorToken),

            // assemble '<<'
            typeof(LeftShiftToken),

            // assemble '>>'
            typeof(RightShiftToken),

            // _|  and  |_  tokens
            typeof(LeftAbsoluteToken),
            typeof(RightAbsoluteToken),

            // _||  and   ||_   tokens
            typeof(LeftNormToken),
            typeof(RightNormToken),

            // ..
            typeof(VectorRangeToken),

            typeof(EqualityToken),
            typeof(InEqualityToken),
            typeof(LessThanOrEqualToken),
            typeof(GreaterThanOrEqualToken)
        );

        tokens = tokens.MergeTokens<WordToken>();                 //Discover words

        tokens = tokens.MergeMultipleWordTokens(
            typeof(WhenStatementToken),
            typeof(OtherwiseStatementToken),
            typeof(AndStatementToken),
            typeof(OrStatementToken),
            typeof(LoopStatementToken),
            typeof(OnStatementToken),
            typeof(TrueToken),
            typeof(FalseToken)
        );

        // merge the $ + Word into Symbolic and get the symbolic variables.
        tokens = tokens.MergeTokens<SymbolicToken>();
        tokens = tokens.MergeSequenceTokens<SymbolicToken>(typeof(DollarToken), typeof(WordToken));
        tokens = tokens.MergeSequenceTokens<SymbolicQuantityToken>(typeof(SymbolicToken), typeof(UnitToken));

        tokens = tokens.MergeTokens<NumberToken>();               //discover the numbers
        tokens = tokens.MergeTokens<UnitizedNumberToken>();
        tokens = tokens.MergeSequenceTokens<UnitizedNumberToken>(typeof(PeriodToken), typeof(UnitizedNumberToken));
        tokens = tokens.MergeSequenceTokens<NumberToken>(typeof(PeriodToken), typeof(NumberToken));


        tokens = tokens.MergeSequenceTokens<ConstantToken>(typeof(PercentToken), typeof(WordToken));

        // discover the complex numbers
        tokens = tokens.MergeTokens<ComplexNumberToken>();
        tokens = tokens.MergeSequenceTokens<ComplexQuantityToken>(typeof(ComplexNumberToken), typeof(UnitToken));

        // discover the quaternion numbers
        tokens = tokens.MergeTokens<QuaternionNumberToken>();
        tokens = tokens.MergeSequenceTokens<QuaternionQuantityToken>(typeof(QuaternionNumberToken), typeof(UnitToken));

        // discover the rational numbers
        tokens = tokens.MergeTokens<RationalNumberToken>();
        tokens = tokens.MergeSequenceTokens<RationalQuantityToken>(typeof(RationalNumberToken), typeof(UnitToken));

        // discover the functions created inline.
        tokens = tokens.MergeTokens<FunctionLambdaToken>();

        tokens = MergeOperators(tokens);

        // merge the function value  expressions
        //  @f
        tokens = tokens.MergeTokens<FunctionValueToken>();
        tokens = tokens.MergeSequenceTokens<FunctionQuantityToken>(typeof(FunctionValueToken), typeof(UnitToken));

        // merge the namespaces.
        tokens = tokens.MergeTokens<NamespaceToken>();
        tokens = tokens.MergeTokens<NameSpaceAndVariableToken>();
        //tokens = tokens.MergeSequenceTokens<NameSpaceAndVariableToken>(typeof(NameSpaceToken), typeof(WordToken));

        // manually merge function value and quantity into its namespace because it can't be done with regular expression
        //tokens = tokens.MergeSequenceTokens<NameSpaceAndVariableToken>(typeof(NameSpaceToken), typeof(FunctionValueToken));
        tokens = tokens.MergeSequenceTokens<NameSpaceAndVariableToken>(typeof(NamespaceToken), typeof(FunctionQuantityToken));


        // Assemble '\''/' into \/ to form the nabla operator
        tokens = tokens.MergeTokens<Nabla>();

        tokens = tokens.MergeTokensInGroups(
            new ParenthesisGroupToken(),                //  group (--()-) parenthesis
            new SquareBracketsGroupToken(),             //  [[][][]]
            new CurlyBracketGroupToken(),               //  {{}}{}
            new TensorGroupToken(),                     //  <| <| |> |>
            new NormGroupToken(),                       //  _|| _|| ||_ ||_
            new AbsoluteGroupToken()                   //  _| _|  |_ |_
        );

        //tokens = tokens.RemoveSpaceTokens();                           //remove all spaces
        tokens = tokens.RemoveAnySpaceTokens();

        tokens = tokens.DiscoverQsCalls(StringComparer.OrdinalIgnoreCase,
            ["When", "Otherwise", "And", "Or", "Loop", "On"]
        );

        tokens = tokens.DiscoverQsLoopsTokens();

        return tokens;
    }

    internal Expression ParseArithmatic(string codeLine, out Token tokenizedExpression)
    {

        var tokens = TokenzieExpression(codeLine);

        tokenizedExpression = tokens;

        return ParseArithmatic(tokens);

    }

    Stack<string> loopIteratorContainer = new();
    Dictionary<string, ParameterExpression> loopIteratorParameter = new();



    /// <summary>
    /// Main parsing method.
    /// </summary>
    /// <param name="toks">Tokens</param>
    /// <returns>Microsoft DLR Expression</returns>
    public ExprOp ParseOperations (Token toks)
    {
        // I remove the spaces here because the function called here sometimes have extra unneeded spaces
        // one scenario is _|| {3 4 5} >> 1 ||_
        // the magnitude token will take the inner tokens as it is then send it for evaluation again.

        var tokens = toks.RemoveSpaceTokens();                           //remove all spaces

        Expression quantityExpression = null;
        ExprOp eop = null;

        ExprOp FirstEop = null;

        var ix = 0;                 //this is the index in the discovered tokens

        while (ix < tokens.Count)
        {
            var qToken = tokens[ix];
            var q = qToken.TokenValue;

            var OperatorTokenText = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

            if (q == "+" || q == "-")
            {
                // unary prefix operator.

                //consume another token for number

                if (q == "+")
                {
                    //q = tokens[ix].TokenValue;
                    quantityExpression = Expression.Constant(QsScalar.One, typeof(QsValue));
                }
                else if(q == "-")
                {
                    quantityExpression = Expression.Constant(QsScalar.NegativeOne, typeof(QsValue));
                }
                else
                {
                    throw new QsException($"Unknown unary operator {q}");
                }

                if (ix == 0)
                {
                    //OperatorTokenText = "*";   // in case of first token that is sign we don't need the higher multiplication order
                    OperatorTokenText = "_h*";
                }
                else
                {
                    OperatorTokenText = "_h*";
                }
                ix--;
                goto ExpressionCompleted;

            }

            if (q.Equals("new", StringComparison.OrdinalIgnoreCase))
            {
                // creating object

                OperatorTokenText = string.Empty;
                ix++;

                Type dt;

                // the next token will be a class
                quantityExpression = CreateInstance(tokens[ix], out dt);

                goto ExpressionCompleted;
            }

            if (q.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                OperatorTokenText = string.Empty;
                ix++;

                quantityExpression = DeleteInstance(tokens[ix]);

                goto ExpressionCompleted;
            }

            var FactorialPostfix = false;
            var PercentagePostfix = false;

            if (!string.IsNullOrEmpty(OperatorTokenText))
            {
                if (OperatorTokenText == "!")
                {
                    // if the ! followed by Word then we want the  "operator !"  otherwise it is a normal factorial.

                    var afterTok = ix + 2 < tokens.Count ? tokens[ix + 2] : null;

                    if (afterTok == null)
                        FactorialPostfix = true;
                    else if (afterTok.TokenClassType == typeof(WordToken) || afterTok.TokenClassType == typeof(TextStringToken) || afterTok.TokenClassType == typeof(SequenceCallToken) || afterTok.TokenClassType == typeof(NamespaceToken))  //either direct word or text i.e. o!word or o!"Word"
                        FactorialPostfix = false;
                    else
                        FactorialPostfix = true;
                }

                if (OperatorTokenText == "%")
                {
                    // if the % followed by Word then we want the  "operator !"  otherwise it is a normal factorial.

                    var afterTok = ix + 2 < tokens.Count ? tokens[ix + 2] : null;

                    if (afterTok == null)
                        PercentagePostfix = true;
                    else if (afterTok.TokenClassType == typeof(NumberToken) || afterTok.TokenClassType == typeof(UnitizedNumberToken) || afterTok.TokenClassType == typeof(WordToken) || afterTok.TokenClassType == typeof(TextStringToken) || afterTok.TokenClassType == typeof(SequenceCallToken) || afterTok.TokenClassType == typeof(NamespaceToken))  //either direct word or text i.e. o!word or o!"Word"
                        PercentagePostfix = false;
                    else
                        PercentagePostfix = true;
                }
            }

            if (tokens[ix].TokenClassType == typeof(SequenceCallToken))
            {
                if (eop != null && eop.Operation == "->")
                {
                    // member call to the object.
                    // execute property its name in tokens[ix][0]
                    quantityExpression =
                        Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("Execute"), Expression.Constant(tokens[ix][0]));

                    var sct =  tokens[ix][1];
                    // because of the behavior of parsing the inner of this token between [ , ]  is ParameterToken
                    // we need to extract the token inside the parameter token

                    var CorrectToken = new Token();
                    CorrectToken.TokenClassType = typeof(SequenceCallToken);
                    foreach (var sct_e in sct)
                    {
                        if (sct_e.TokenClassType == typeof(ParameterToken))
                        {
                            // extract its content
                            CorrectToken.AppendSubToken(sct_e[0]);
                        }
                        else
                            CorrectToken.AppendSubToken(sct_e);
                    }

                    // then apply indexing
                    quantityExpression = ValueIndexExpression(quantityExpression, CorrectToken);

                    // then skip this operation because it has been evaluated

                    eop.Operation = "Skip";

                }
                else if (eop != null && eop.Operation == "!")
                {
                    // execute exlamination first
                    quantityExpression = Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("ExclamationOperator"), Expression.Constant(new QsText(tokens[ix][0].TokenValue)));

                    // then indexer after
                    var sct = tokens[ix][1];
                    var CorrectToken = new Token();
                    CorrectToken.TokenClassType = typeof(SequenceCallToken);
                    foreach (var sct_e in sct)
                    {
                        if (sct_e.TokenClassType == typeof(ParameterToken))
                        {
                            // extract its content
                            CorrectToken.AppendSubToken(sct_e[0]);
                        }
                        else
                            CorrectToken.AppendSubToken(sct_e);
                    }

                    // then apply indexing
                    quantityExpression = ValueIndexExpression(quantityExpression, CorrectToken);

                    // then skip this operation because it has been evaluated

                    eop.Operation = "Skip";
                }
                else
                {
                    quantityExpression = IndexerExpression(
                        tokens[ix][0].TokenValue,
                        tokens[ix][1],
                        tokens[ix].Count > 2 ? tokens[ix][2] : null                   //if arguments exist we must include them   form  S[n](x,y,z)
                    );
                }
            }
            else if (tokens[ix].TokenClassType == typeof(ParenthesisCallToken))
            {
                if (eop != null && eop.Operation == "->")
                {
                    // member call to the object.
                    quantityExpression =
                        Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("Execute"), Expression.Constant(tokens[ix]));

                    eop.Operation = "Skip"; // skip this operation because we already made an evaluation for it
                }
                else
                {
                    quantityExpression = FunctionCallExpression(
                        tokens[ix][0].TokenValue,
                        tokens[ix][1]
                    );
                }
            }
            else if (tokens[ix].TokenClassType == typeof(ParenthesisGroupToken))
            {
                var x = tokens[ix].RemoveSpaceTokens().TrimTokens(1, 1);
                // count the ',' comma token
                // if the existence is more than 0 then we are expressing a tuple
                var commaCount = x.Count(c => c.TokenClassType == typeof(CommaToken));

                if (commaCount > 0)
                {
                    // tuple
                    quantityExpression = ConstructTupleExpression(x);
                }
                else
                {
                    // take the inner tokens and send it to be parsed again.
                    quantityExpression = ParseArithmatic(x);
                }
            }
            else if (tokens[ix].TokenClassType == typeof(UnitizedNumberToken))
            {
                //unitized number
                quantityExpression = Expression.Constant(QsValue.ParseScalar(q), typeof(QsValue)); //you have to explicitly tell expression the type because it searches for the operators and can't find them
            }
            else if (tokens[ix].TokenClassType == typeof(NumberToken))
            {
                //ordinary number
                quantityExpression = Expression.Constant(QsValue.ParseScalar(q), typeof(QsValue));
            }
            else if (tokens[ix].TokenClassType == typeof(CurlyBracketGroupToken))
            {
                // Vector declaration.
                quantityExpression = ParseVector(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(SquareBracketsGroupToken))
            {
                // Matrix declaration.
                quantityExpression = ParseMatrix(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(TensorGroupToken))
            {
                quantityExpression = ParseTensor(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(TextStringToken))
            {
                quantityExpression = ParseText(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(NormGroupToken))
            {
                quantityExpression = ValueNorm(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(AbsoluteGroupToken))
            {
                quantityExpression = ValueAbsolute(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(FunctionValueToken))
            {
                quantityExpression = GetFunctionAsQuantity(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(FunctionQuantityToken))
            {
                quantityExpression = GetFunctionAsQuantity(tokens[ix][0], tokens[ix][1]);
            }
            else if (tokens[ix].TokenClassType == typeof(AtSignToken))
            {
                // convert '@' into function operation.
                var oo = new QsScalar(ScalarTypes.QsOperation)
                {
                    Operation = new QsDifferentialOperation()
                };

                quantityExpression = Expression.Constant(oo, typeof(QsValue));
            }
            else if (tokens[ix].TokenClassType == typeof(Nabla))
            {
                string[] xi = (
                    from x in tokens[ix].TrimTokens(1, 1).TokenValue.Split(' ', ',')
                    where x.Trim() != string.Empty
                    select x.Trim()).ToArray();

                var oo = new QsScalar(ScalarTypes.QsOperation)
                {
                    Operation = new QsNablaOperation(xi)
                };
                quantityExpression = Expression.Constant(oo, typeof(QsValue));
            }
            else if (tokens[ix].TokenClassType == typeof(SymbolicToken) || tokens[ix].TokenClassType == typeof(SymbolicQuantityToken))
            {
                quantityExpression = SymbolicScalar(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(ConstantToken))
            {
                quantityExpression = Expression.Constant(QsScalar.Constant(tokens[ix].TokenValue.TrimStart('%')), typeof(QsValue));
            }
            else if (tokens[ix].TokenClassType == typeof(ComplexNumberToken) || tokens[ix].TokenClassType == typeof(ComplexQuantityToken))
            {
                quantityExpression = ComplexScalar(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(QuaternionNumberToken) || tokens[ix].TokenClassType == typeof(QuaternionQuantityToken))
            {
                quantityExpression = QuaternionScalar(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(RationalNumberToken) || tokens[ix].TokenClassType == typeof(RationalQuantityToken))
            {
                quantityExpression = RationalScalar(tokens[ix]);
            }
            else if (tokens[ix].TokenClassType == typeof(FunctionLambdaToken))
            {
                var g = tokens[ix].TrimTokens(2, 1);
                var funcbody = g.TokenValue;
                if (funcbody.StartsWith("(")) funcbody = "_" + funcbody;

                var sf = new QsScalar(ScalarTypes.FunctionQuantity)
                {
                    FunctionQuantity = QsFunction.ParseFunction(Evaluator, funcbody).ToQuantity()
                };

                quantityExpression = Expression.Constant(sf);
            }
            else if (tokens[ix].TokenClassType == typeof(NamespaceToken))
            {

                // ok this is something like  N:N:L:
                // so we will extract the last
                OperatorTokenText = ":";    // make colon an operation

                var mtk = tokens[ix].TrimEnd(typeof(ColonToken)); // removce the latest colon from the tokens.

                if (eop != null && eop.Operation == "->")
                {
                    // Execute first
                    quantityExpression = Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("Execute"), Expression.Constant(mtk));
                    eop.Operation = "Skip";
                }
                else if (eop != null && eop.Operation == "!")
                {
                    // execute exlamination first
                    quantityExpression = Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("ExclamationOperator"), Expression.Constant(new QsText(mtk.TokenValue)));
                    eop.Operation = "Skip";
                }
                else
                    quantityExpression = GetQsVariable(mtk);

                --ix;   //decrease ix with one because operator was fused in this token.

            }
            else if (tokens[ix].TokenClassType == typeof(TrueToken)) quantityExpression = Expression.Constant(QsBoolean.True);
            else if (tokens[ix].TokenClassType == typeof(FalseToken)) quantityExpression = Expression.Constant(QsBoolean.False);
            else if (tokens[ix].TokenClassType == typeof(LoopBodyToken))
            {
                // get the count of the loop
                var v = Evaluator.GetVariable(tokens[ix][3].TokenValue) as QsValue;

                if (v == null) throw new QsException(tokens[ix][3].TokenValue + " doesn't exist in memoir");
                var count = 0;

                if (v is QsVector) count = ((QsVector)v).Count;
                else if (v is QsFlowingTuple) count = ((QsFlowingTuple)v).Count;
                else if (v is QsText) count = ((QsText)v).Text.Length;
                else throw new QsException(tokens[ix][3].TokenValue + " of type " + v.GetType().Name + " is not supported");

                // expression that hold the counter.
                var counter = Expression.Parameter(typeof(int), "Counter_" + loopIteratorContainer.Count);


                // the statement in tokens[ix][1] will be repeated on the count of vector or tuple on tokens[ix][3]
                var LoopBreakLabel = Expression.Label(typeof(QsValue), "LoopBreak_" + loopIteratorContainer.Count);

                // Creating an expression to hold a local variable.
                var result = Expression.Parameter(typeof(QsFlowingTuple), "result");


                // modify the state of this qsvar to indicate there is a difference in getting the value
                loopIteratorContainer.Push(tokens[ix][3].TokenValue);
                loopIteratorParameter.Add(tokens[ix][3].TokenValue, counter);
                //QsFlowingTuple ss = new QsFlowingTuple();
                //ss.AddOperation(null);

                var fltAdd = typeof(QsFlowingTuple).GetMethod("AddTupleValue");


                quantityExpression = Expression.Block(
                    new[] { counter, result },

                    Expression.Assign(result, Expression.New(typeof(QsFlowingTuple))),
                    Expression.Assign(counter, Expression.Constant(0)),
                    Expression.Loop(
                        Expression.IfThenElse(
                            Expression.LessThan(counter, Expression.Constant(count)),
                            Expression.Block(
                                Expression.Call(result, fltAdd, ParseArithmatic(tokens[ix][1]))
                                , Expression.PostIncrementAssign(counter)
                            ),
                            Expression.Break(LoopBreakLabel, result)
                        ), LoopBreakLabel)
                );

                loopIteratorContainer.Pop();
                loopIteratorParameter.Remove(tokens[ix][3].TokenValue);

            }
            else
            {
                // Word token:  means variable
                if (ParseMode == QsVarParseModes.Function)
                {
                    #region Variable in Function Parsing

                    quantityExpression = GetFunctionOrSequenceParameter(q);
                    #endregion
                }
                else if (ParseMode == QsVarParseModes.Sequence)
                {
                    #region Variable in Sequence Parsing

                    if (q.Equals(Sequence.SequenceRangeStartName, StringComparison.OrdinalIgnoreCase))
                    {
                        quantityExpression = Expression.PropertyOrField(Expression.Constant(Sequence), "StartIndex");
                        quantityExpression = Expression.Call(typeof(Qs).GetMethod("ToScalarValue", [typeof(int)]), quantityExpression);

                        Sequence.CachingEnabled = false;
                    }
                    else if (q.Equals(Sequence.SequenceRangeEndName, StringComparison.OrdinalIgnoreCase))
                    {
                        quantityExpression = Expression.PropertyOrField(Expression.Constant(Sequence), "EndIndex");
                        quantityExpression = Expression.Call(typeof(Qs).GetMethod("ToScalarValue", [typeof(int)]), quantityExpression);

                        Sequence.CachingEnabled = false; // because start and end index may change from call to call
                    }
                    else
                    {


                        //we should check if q is index or parameter of sequence.

                        if (q.Equals(Sequence.SequenceIndexName, StringComparison.OrdinalIgnoreCase))
                        {
                            quantityExpression = lambdaBuilder.Parameters.Single(c => c.Name == q);

                            //it is really an index
                            // convert it into Quantity.
                            //   because it is only integer

                            quantityExpression = Expression.Call(typeof(Qs).GetMethod("ToScalarValue", [typeof(int)]), quantityExpression);

                        }
                        else if (Sequence.Parameters.Count(s => s.Name.Equals(q, StringComparison.OrdinalIgnoreCase)) > 0)
                        {

                            quantityExpression = GetFunctionOrSequenceParameter(q);

                        }
                        else
                        {
                            // global variable
                            //quantity variable  //get it from evaluator  global heap
                            quantityExpression = GetQsVariable(tokens[ix]);
                            Sequence.CachingEnabled = false;  //because when evaluating external variable the external variable may change without knowing
                            throw new QsParameterNotFoundException("Global variable (" + q + ") are not permitted");
                        }


                    }
                    #endregion

                }
                else
                {
                    Action<string, ParameterExpression> GetIteratorElement = (s, vp) =>
                    {
                        // the variable here is iterated over declared tuple or vector
                        // we will get the value from vector or tuple based on counter parameter

                        quantityExpression = GetQsVariable(tokens[ix]);
                        var mix = typeof(QsValue).GetMethod("GetIndexedItem");
                        var mpm = typeof(QsParameter).GetMethod("MakeParameter");

                        var ItoQsv = typeof(Qs).GetMethod("ToScalarValue", [typeof(int)]);

                        var lip = Expression.Call(ItoQsv, vp);

                        var IndexArg = Expression.Call(mpm
                            , Expression.Convert(lip, typeof(object))
                            , Expression.Constant("Counter_" + s));

                        quantityExpression = Expression.Call(quantityExpression, mix, Expression.NewArrayInit(typeof(QsParameter), IndexArg));
                    };

                    #region variable in main context
                    if (eop != null)
                    {
                        if (eop.Operation == "->")
                        {
                            // previous operation were calling for object member
                            quantityExpression =
                                Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("Execute"), Expression.Constant(tokens[ix]));

                            eop.Operation = "Skip"; // skip this operation because we already made an evaluation for it
                        }
                        else if (eop.Operation == "!")
                        {
                            // quantityExpression = Expression.Constant(new QsText(tokens[ix].TokenValue));

                            quantityExpression =
                                Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("ExclamationOperator"), Expression.Constant(new QsText(tokens[ix].TokenValue)));

                            eop.Operation = "Skip";
                        }
                        else if (eop.Operation == ":")
                        {
                            // quantityExpression = Expression.Constant(new QsText(tokens[ix].TokenValue));
                            quantityExpression =
                                Expression.Call(eop.QuantityExpression, typeof(QsValue).GetMethod("ColonOperator"), Expression.Constant(new QsText(tokens[ix].TokenValue)));

                            eop.Operation = "Skip";
                        }
                        else
                        {
                            if (loopIteratorContainer.Count > 0 && loopIteratorContainer.Contains(tokens[ix].TokenValue))
                            {
                                GetIteratorElement(tokens[ix].TokenValue, loopIteratorParameter[tokens[ix].TokenValue]);
                            }
                            else if (loopIteratorContainer.Count > 0
                                     && tokens[ix].TokenValue.Length > 1
                                     && tokens[ix].TokenValue.StartsWith("_")
                                     && loopIteratorContainer.Contains(tokens[ix].TokenValue.Substring(1))
                                    )
                            {
                                // case of loop counter
                                var ItoQsv = typeof(Qs).GetMethod("ToScalarValue", [typeof(int)]);

                                quantityExpression = Expression.Call(ItoQsv, loopIteratorParameter[tokens[ix].TokenValue.Substring(1)]);
                            }
                            else
                            {
                                //quantity variable  //get it from evaluator  global heap
                                quantityExpression = GetQsVariable(tokens[ix]);
                            }
                        }
                    }
                    else
                    {
                        if (loopIteratorContainer.Count > 0 && loopIteratorContainer.Contains(tokens[ix].TokenValue))
                        {
                            // the case of accessing the iterator
                            GetIteratorElement(tokens[ix].TokenValue, loopIteratorParameter[tokens[ix].TokenValue]);
                        }
                        else if (loopIteratorContainer.Count > 0
                                 && tokens[ix].TokenValue.Length > 1
                                 && tokens[ix].TokenValue.StartsWith("_")
                                 && loopIteratorContainer.Contains(tokens[ix].TokenValue.Substring(1))
                                )
                        {
                            // case of loop counter
                            var ItoQsv = typeof(Qs).GetMethod("ToScalarValue", [typeof(int)]);

                            quantityExpression = Expression.Call(ItoQsv, loopIteratorParameter[tokens[ix].TokenValue.Substring(1)]);
                        }
                        else
                        {
                            //quantity variable  //get it from evaluator  global heap
                            quantityExpression = GetQsVariable(tokens[ix]);
                        }
                    }
                    #endregion
                }

            }

            #region Apply the postfix here

            if (FactorialPostfix)
            {
                quantityExpression = Expression.Call(typeof(QsGamma).GetMethod("Factorial"), quantityExpression);

                //get the next operator.
                ix++;
                OperatorTokenText = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

                FactorialPostfix = false;
            }

            if (PercentagePostfix)
            {
                quantityExpression = Expression.Divide(quantityExpression, Expression.Constant(QsValue.ParseScalar("100"), typeof(QsValue)));

                //get the next operator.
                ix++;
                OperatorTokenText = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

                PercentagePostfix = false;
            }

            #endregion
            ConsumeOtherBrackets:
            if (ix + 1 < tokens.Count)
            {
                if (tokens[ix + 1].TokenClassType == typeof(SquareBracketsGroupToken))
                {
                    // call indexer here
                    quantityExpression = ValueIndexExpression(quantityExpression, tokens[ix + 1]);


                    //get the next operator
                    ix++;

                    OperatorTokenText = ix + 1 < tokens.Count ? tokens[ix + 1].TokenValue : string.Empty;

                    goto ConsumeOtherBrackets;    //because their might be other indexers tokens i don't know about.
                }
            }

            ExpressionCompleted:
            if (eop == null)
            {
                //firs time creation
                FirstEop = new ExprOp();

                eop = FirstEop;
            }
            else
            {
                if (eop.Operation != "Skip")
                {
                    //use the next object to be eop.
                    eop.Next = new ExprOp();
                    eop = eop.Next;
                }
            }

            eop.Operation = OperatorTokenText;
            eop.QuantityExpression = quantityExpression;

            ix += 2;

        }

        if (eop.Next == null && string.IsNullOrEmpty(eop.Operation) == false)
        {
            //eop hold the last node to be evaluated
            // if the next of eop is null then it means an operation without right term
            //    to do the operation on it.
            // 
            //  so raise an exception

            throw new QsIncompleteExpression();
        }


        return FirstEop;
    }


    internal Expression ParseArithmatic(Token toks)
    {
        var FirstEop = ParseOperations(toks);

        //then form the calculation expression
        return  ConstructExpression(FirstEop);
    }

    private Expression ConstructTupleExpression(Token x)
    {
        List<Expression> ps = [];

        var tpvcp1 = typeof(QsTupleValue).GetConstructor([typeof(QsValue)]);

        var tpvcp2 = typeof(QsTupleValue).GetConstructor([typeof(string), typeof(QsValue)]);

        var tpvcp3 = typeof(QsTupleValue).GetConstructor([typeof(int), typeof(string), typeof(QsValue)]);

        var tpvcp4 = typeof(QsTupleValue).GetConstructor([typeof(int), typeof(QsValue)]);


        /*
            #normal tuple
            tuple = (3,2,1,5)

            #tuple that is also dictionary
            tuple = ( T!3<K>, R!90<L>, P!{32 2 1}, G![3 2 ; 2 4], L!<|2 3 ; 3 2| 3 4; 1 4|>, H!(3,4,1))

            #tuple that is dictionary with name and integer
            tuple = ( 3:T!400<K>, 500, 200, 20:RT!"Hello there", 40:<|3|>)

         */

        var p = from pp in x.MergeAllBut<ParameterToken>(new CommaToken())
            where pp.TokenClassType != typeof(CommaToken)
            select pp;

        foreach (var t in p)
        {
            var be = new Token();

            var NameExist = false;
            var IdExist = false;

            if( t.Count(el => el.TokenClassType == typeof(ExclamationToken))>0) NameExist=true;
            if( t.Count(el => el.TokenClassType == typeof(ColonToken))>0) IdExist=true;

            if (NameExist && IdExist)
            {
                // Integer-Id:Name!Value
                var i = int.Parse(t[0].TokenValue);
                var nm = t[2].TokenValue;

                be.AppendSubToken(t[4]);
                var r = ParseArithmatic(be);

                ps.Add(Expression.New(tpvcp3, Expression.Constant(i), Expression.Constant(nm), r));
            }
            else if (NameExist)
            {
                // Name!value
                var nm = t[0].TokenValue;

                be.AppendSubToken(t[2]);
                var r = ParseArithmatic(be);

                ps.Add(Expression.New(tpvcp2, Expression.Constant(nm), r));
            }
            else if (IdExist)
            {
                // Integer-Id:value              which is clearly not namespace

                var i = int.Parse(t[0].TokenValue);
                be.AppendSubToken(t[2]);
                var r = ParseArithmatic(be);

                ps.Add(Expression.New(tpvcp4, Expression.Constant(i), r));
            }
            else
            {

                var r = ParseArithmatic(t);
                ps.Add(Expression.New(tpvcp1, r));
            }


        }

        var cp = typeof(QsFlowingTuple).GetConstructor([typeof(QsTupleValue[])]);

        return Expression.New(cp, Expression.NewArrayInit(typeof(QsTupleValue), ps.ToArray()));
    }

    private Expression DeleteInstance(Token token)
    {

        var ex = Expression.Constant(Evaluator);

        return Expression.Call(ex, typeof(QsEvaluator).GetMethod("DeleteQsValue")
            , Expression.Constant(string.Empty)
            , Expression.Constant(token.TokenValue)
        );

    }

    /// <summary>
    /// the token contains the information of the wanted class.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private Expression CreateInstance(Token token, out Type discoveredType)
    {
        var cls = string.Empty;
        string[] args = [];

        if(token.TokenClassType == typeof(ParenthesisCallToken))
        {
            cls = token[0].TokenValue;


            List<string> parms = [];
            //discover parameters
            for (var ai = 1; ai < token[1].Count - 1; ai++)
            {
                if (token[1][ai].TokenValue != ",")
                {
                    parms.Add(token[1][ai].TokenValue);
                }
            }

            args = parms.ToArray();
        }
        else
        {
            cls = token.TokenValue;
        }

        cls = cls.Replace(':','.');


        if (cls.StartsWith(".."))
            cls = cls.Replace("..", ""); // indicates that we used root operator in namespace  ::
        else
            cls = "QsRoot." + cls;



#if WINRT
            discoveredType = Root.GetInternalType(cls);
#else
        discoveredType = Type.GetType(cls, false);
        if (discoveredType == null) discoveredType = Root.GetExternalType(cls);
#endif

        if (discoveredType == null) throw new QsException(string.Format("[{0}] type doesn't exist.", cls));

        if (args.Length == 0)
        {
            return Expression.Call(typeof(QsObject).GetMethod("CreateNativeObject"), Expression.New(discoveredType));
        }

        //constructor included
        //know the parameters types
        List<Expression> Arguments = new(args.Length);
        var argsToken = token[1];

        var d_ctor = discoveredType.GetConstructors().First(c => c.GetParameters().Length == args.Length);
        var d_ctor_params = d_ctor.GetParameters();

        var ctorArgIndex = 0;
        foreach (var tk in argsToken)
        {
            if (tk.TokenClassType == typeof(ParameterToken))
            {
                // evaluate the expresion
                var expr = ParseArithmatic(tk);  // the return value is QsValue

                // we have to check the corresponding parameter type to make convertion if needed.
                var targetType = d_ctor_params[ctorArgIndex].ParameterType;

                var exprArg = Expression.Convert(Root.QsToNativeConvert(targetType, expr)
                    , targetType);
                Arguments.Add(exprArg);
                ctorArgIndex++;
            }
        }

        // args has been prepared
        return Expression.Call(typeof(QsObject).GetMethod("CreateNativeObject"), Expression.New(d_ctor, Arguments.ToArray()));
    }

    /// <summary>
    /// Parse the complex number  C{3 2}  into expression
    /// </summary>
    /// <param name="complexToken"></param>
    /// <returns></returns>
    private Expression RationalScalar(Token complexToken)
    {
        Token cn = null;
        if (complexToken.TokenClassType == typeof(RationalQuantityToken))
            cn = complexToken[0];
        else
            cn = complexToken;

        var token = cn.TrimStart(typeof(WordToken)).TrimStart(typeof(LeftCurlyBracketToken));
        token = token.TrimEnd(typeof(RightCurlyBracketToken));

        token = token.MergeAllBut(typeof(MergedToken), new CommaToken(), new MultipleSpaceToken());
        token = token.RemoveTokens(typeof(CommaToken), typeof(MultipleSpaceToken));

        var nums = new List<float>();
        for (var i = 0; i < token.Count; i++)
        {
            if (token[i].TokenClassType == typeof(MergedToken))
            {
                nums.Add(float.Parse(token[i].TokenValue));
            }
        }

        float num = 0, den = 0;

        if (nums.Count > 0) num = nums[0];
        if (nums.Count > 1) den = nums[1];

        var c = new Rational(num, den);
        QsScalar sc = null;
        if (complexToken.TokenClassType == typeof(RationalQuantityToken)) // there is a unit
            sc = c.ToQuantity(complexToken[1].TokenValue.Trim('<', '>')).ToScalar();

        else
            sc = c.ToQuantity().ToScalar();

        return Expression.Constant(sc, typeof(QsScalar));
    }



    private Expression QuaternionScalar(Token quaternionToken)
    {

        Token cn = null;
        if (quaternionToken.TokenClassType == typeof(QuaternionQuantityToken))
            cn = quaternionToken[0];
        else
            cn = quaternionToken;

        var token = cn.TrimStart(typeof(WordToken)).TrimStart(typeof(LeftCurlyBracketToken));
        token = token.TrimEnd(typeof(RightCurlyBracketToken));

        token = token.MergeAllBut(typeof(MergedToken), new CommaToken(), new MultipleSpaceToken());
        token = token.RemoveTokens(typeof(CommaToken), typeof(MultipleSpaceToken));

        var nums = new List<double>();
        for (var i = 0; i < token.Count; i++)
        {
            if (token[i].TokenClassType == typeof(MergedToken))
            {
                nums.Add(double.Parse(token[i].TokenValue, CultureInfo.InvariantCulture));
            }
        }

        double real = 0, im = 0, j = 0, k = 0;

        if (nums.Count > 0) real = nums[0];
        if (nums.Count > 1) im = nums[1];
        if (nums.Count > 2) j = nums[2];
        if (nums.Count > 3) k = nums[3];

        var c = new Quaternion(real, im, j, k);
        QsScalar sc = null;
        if (quaternionToken.TokenClassType == typeof(QuaternionQuantityToken)) // there is a unit
            sc = c.ToQuantity(quaternionToken[1].TokenValue.Trim('<', '>')).ToScalar();
        else
            sc = c.ToQuantity().ToScalar();

        return Expression.Constant(sc, typeof(QsScalar));

    }

    private Expression ComplexScalar(Token complexToken)
    {
        Token cn = null;
        if (complexToken.TokenClassType == typeof(ComplexQuantityToken))
            cn = complexToken[0];
        else
            cn = complexToken;

        var token = cn.TrimStart(typeof(WordToken)).TrimStart(typeof(LeftCurlyBracketToken));
        token = token.TrimEnd(typeof(RightCurlyBracketToken));

        token = token.MergeAllBut(typeof(MergedToken), new CommaToken(), new MultipleSpaceToken());
        token = token.RemoveTokens(typeof(CommaToken), typeof(MultipleSpaceToken));

        var nums = new List<double>();
        for (var i = 0; i < token.Count; i++)
        {
            if (token[i].TokenClassType == typeof(MergedToken))
            {
                nums.Add(double.Parse(token[i].TokenValue, CultureInfo.InvariantCulture));
            }
        }

        double real = 0, imaginary = 0;

        if(nums.Count>0) real = nums[0];
        if(nums.Count>1) imaginary = nums[1];

        var c = new Complex(real, imaginary);
        QsScalar sc = null;
        if (complexToken.TokenClassType == typeof(ComplexQuantityToken)) // there is a unit
            sc = c.ToQuantity(complexToken[1].TokenValue.Trim('<', '>')).ToScalar();

        else
            sc = c.ToQuantity().ToScalar();

        return Expression.Constant(sc, typeof(QsScalar));
    }


    /// <summary>
    /// Get Absolute of number or determinant of matrix.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private Expression ValueAbsolute(Token token)
    {
        // parsing within absolute will re-execute the parsing
        var rr = ParseArithmatic(token.TrimTokens(1, 1));

        rr = Expression.Call(rr, typeof(QsValue).GetMethod("AbsOperation"));

        return rr;
    }

    /// <summary>
    /// Get the magnitude of the inner text between ||text||
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private Expression ValueNorm(Token token)
    {
        //the token is holding the vector or the expression that will lead to the vector.

        // parsing within absolute will re-execute the parsing
        var rr = ParseArithmatic(token.TrimTokens(1, 1));

        rr = Expression.Call(rr, typeof(QsValue).GetMethod("NormOperation"));

        return rr;
    }


    #region QsValue Parsers

    /// <summary>
    /// Returns the text components of {Vector} {a b c} I mean a,b, and  c  in text format
    /// </summary>
    /// <param name="vectorToken"></param>
    /// <returns></returns>
    public static ICollection<string> VectorComponents(Token tok)
    {
        if (tok.TokenClassType != typeof(CurlyBracketGroupToken)) throw new QsException("The passed token type is not curly bracket group token, it is " + tok.TokenClassType.Name);

        List<string> qtyExpressions = [];
        var token = tok.TrimStart(typeof(LeftCurlyBracketToken));
        token = token.TrimEnd(typeof(RightCurlyBracketToken));


        token = token.MergeAllBut(typeof(MergedToken), new CommaToken(), new MultipleSpaceToken());
        token = token.RemoveTokens(typeof(CommaToken), typeof(MultipleSpaceToken));


        for (var i = 0; i < token.Count; i++)
        {
            if (token[i].TokenClassType == typeof(MergedToken))
            {
                qtyExpressions.Add(token[i].TokenValue);
            }
        }

        return qtyExpressions;
    }

    /// <summary>
    /// Vector of the format { 40, 20, f(2), S[10], 23}
    /// </summary>
    /// <param name="vectorText"></param>
    /// <returns></returns>
    private Expression ParseVector(Token tok)
    {
        List<Expression> qtyExpressions = [];
        var token = tok.TrimStart(typeof(LeftCurlyBracketToken));
        token = token.TrimEnd(typeof(RightCurlyBracketToken));


        token = token.MergeAllBut(typeof(MergedToken), new CommaToken(), new MultipleSpaceToken());
        token = token.RemoveTokens(typeof(CommaToken), typeof(MultipleSpaceToken));


        for (var i = 0; i < token.Count ;i++ )
        {
            if (token[i].TokenClassType == typeof(MergedToken))
            {
                qtyExpressions.Add(ParseArithmatic(token[i]));
            }
        }

        var valuesArray = Expression.NewArrayInit(typeof(QsValue), qtyExpressions);

        var vectorExpression = Expression.Call(
            typeof(QsValue).GetMethod("VectorFromValues"),
            valuesArray);

        return vectorExpression;
    }


    /// <summary>
    /// Parsing is expecting values separated by colon or space
    /// also components should be vectors on the syntax {4 3 2 1}.
    /// </summary>
    /// <param name="tok"></param>
    /// <returns></returns>
    private Expression ParseMatrix(Token tok)
    {
        List<Expression> vctExpressions = [];

        var token = tok.TrimStart(typeof(LeftSquareBracketToken));

        token = token.TrimEnd(typeof(RightSquareBracketToken));

        // first split between semi colon tokens.
        // then use every splitted token to

        token = token.MergeAllBut(typeof(MergedToken), new SemiColonToken());

        // now the tokens are scalars or vectors followed by semi colon;
        // put them beside each other

        for (var i = 0; i < token.Count; i++)
        {

            if (token[i].TokenClassType == typeof(MergedToken))
            {
                // loop through every element
                //  whether it was scalar or vector or matrix.

                List<Expression> componentsExpressions = [];

                var stoks = token[i].MergeAllBut(typeof(MergedToken), new CommaToken(), new MultipleSpaceToken());

                foreach (var vtk in stoks)
                {
                    if (
                        vtk.TokenClassType != typeof(MultipleSpaceToken)
                        &&
                        vtk.TokenClassType != typeof(CommaToken)
                    )
                    {

                        componentsExpressions.Add(ParseArithmatic(vtk));    // get the expression of the current component.

                    }
                }

                Expression ComponentsArray = Expression.NewArrayInit(typeof(QsValue), componentsExpressions);
                Expression MatrixOfComponents = Expression.Call(
                    typeof(QsValue).GetMethod("MatrixRowFromValues"),
                    ComponentsArray);

                vctExpressions.Add(MatrixOfComponents);

            }
        }

        var valuesArray = Expression.NewArrayInit(typeof(QsValue), vctExpressions);

        var matrixExpression = Expression.Call(
            typeof(QsValue).GetMethod("MatrixFromValues"),
            valuesArray);

        return matrixExpression;
    }


    public Expression ParseTensor(Token tok)
    {
        List<Expression> vctExpressions = [];

        var token = tok.TrimStart(typeof(LeftTensorToken));

        token = token.TrimEnd(typeof(RightTensorToken));

        // first split between semi colon tokens.
        // then use every splitted token to

        token = token.MergeAllBut(typeof(MergedToken), new VerticalBarToken());

        for (var i = 0; i < token.Count; i++)
        {

            if (token[i].TokenClassType == typeof(MergedToken))
            {
                if (token[i].Contains<TensorGroupToken>())
                {
                    // get the matrix inside and parse it
                    var subTensors = token[i].RemoveSpaceTokens();
                    vctExpressions.Add(ParseArithmatic(subTensors));
                }
                else
                {
                    // treat as a matrix
                    var vtk = token[i].FuseTokens<SquareBracketsGroupToken>("[", "]");
                    vctExpressions.Add(ParseArithmatic(vtk));
                }
            }
        }

        var valuesArray = Expression.NewArrayInit(typeof(QsValue), vctExpressions);

        var tensorExpression = Expression.Call(
            typeof(QsValue).GetMethod("TensorFromValues"),
            valuesArray);

        return tensorExpression;


    }


    public Expression ParseText(Token tok)
    {
        var tx = Expression.Constant( tok.TrimTokens(1,1).TokenValue);
        var texte = Expression.New(typeof(QsText).GetConstructor([typeof(string)]), tx);
        return Expression.Convert(texte, typeof(QsValue));
    }

    #endregion

    #region Helpers in making values


    /// <summary>
    /// Obtain variable value by expression in the current scope
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Expression GetQsVariable(Token name)
    {
        //string GetterFunction = isObject ? "GetScopeValue" : "GetScopeQsValue";
        var GetterFunction = "GetScopeQsValue";

        var ScopeType = Evaluator.Scope.GetType();

        //store the scope
        var ScopeExp = Expression.Constant(Evaluator.Scope, ScopeType);

        //check if the name contain namespace
        string[] var = name.TokenValue.Split(':');
        if (var.Length >= 2)
        {
            var ns = name.TokenValue.Substring(0, name.TokenValue.LastIndexOf(':'));
            var nsvar = var[var.Length - 1];

            if (nsvar.StartsWith("@"))
            {
                // referring to a function in the namespace.
                return GetFunctionAsQuantity(
                    name
                );
            }

            // normal variable in the namespace
            return Expression.Call(
                typeof(QsEvaluator).GetMethod(GetterFunction),
                ScopeExp,
                Expression.Constant(ns),
                Expression.Constant(nsvar)
            );


        }

        var fe = Expression.Call(
            typeof(QsEvaluator).GetMethod(GetterFunction),
            ScopeExp,
            Expression.Constant(string.Empty),
            Expression.Constant(var[0])
        );


        return fe;
    }


    /// <summary>
    /// gets the function as a scalar of function quantity.
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    public Expression GetFunctionAsQuantity(Token functionValueToken)
    {
        QsFunction f;

        if (functionValueToken.TokenValue.Contains("("))
        {
            // the existence of open parenthesis indicates that we refered to the function name with its parameters list also
            // like this   @f(x,y)  or Y:@F(u,v)

            // get from the opening bracket
            var fpp = functionValueToken.TokenValue.Substring(functionValueToken.TokenValue.IndexOf('('));

            // put the parameter names in array
            string[] pp = (from p in (fpp.Substring(1, fpp.Length - 2).Split(','))
                select p.Trim()).ToArray();


            if (pp.Length == 1 && string.IsNullOrEmpty(pp[0])) pp = null;

            var ns = string.Empty;

            if (functionValueToken.TokenValue.Contains(":"))
                ns = functionValueToken.TokenValue.Substring(0, functionValueToken.TokenValue.LastIndexOf(':'));
            var nsfnname = functionValueToken.TokenValue.Substring(functionValueToken.TokenValue.LastIndexOf(':') + 2);
            nsfnname = nsfnname.Substring(0, nsfnname.IndexOf('('));


            //specify parameters
            f = QsFunction.GetExactFunctionWithParameters(
                Scope,
                ns,
                nsfnname,
                pp
            );
        }
        else
        {
            if (functionValueToken.TokenValue.StartsWith("@"))
            {
                f = QsFunction.GetFirstDeclaredFunction(
                    Scope,
                    string.Empty,
                    functionValueToken.TokenValue.Substring(1));

            }
            else
            {
                //namespace included
                f = QsFunction.GetFirstDeclaredFunction(Scope,
                    functionValueToken.TokenValue.Substring(0, functionValueToken.TokenValue.LastIndexOf(':')),
                    functionValueToken.TokenValue.Substring(functionValueToken.TokenValue.LastIndexOf(':') + 2));
            }
        }


        if (f != null)
        {

            //return Expression.Constant(f, typeof(QsValue));
            return Expression.Constant(f.ToQuantity().ToScalar(), typeof(QsScalar));
        }

        throw new QsVariableNotFoundException("Function (" + functionValueToken.TokenValue + ") not found");
    }

    public Expression GetFunctionAsQuantity(Token functionValueToken, Token unit)
    {
        QsFunction f;
        if (functionValueToken.Count == 2)
        {
            if (functionValueToken[1].TokenClassType == typeof(WordToken))
            {
                f = QsFunction.GetFirstDeclaredFunction(
                    Scope,
                    string.Empty,
                    functionValueToken[1].TokenValue);

            }
            else
            {
                //namespace included
                f = QsFunction.GetFirstDeclaredFunction(Scope,
                    functionValueToken[1][0][0].TokenValue,
                    functionValueToken[1][1].TokenValue);
            }
        }
        else if (functionValueToken.Count > 2)
        {
            var fpp = functionValueToken.SubTokensValue(2);

            string[] pp = (from p in (fpp.Substring(1, fpp.Length - 2).Split(','))
                select p.Trim()).ToArray();

            if (pp.Length == 1 && string.IsNullOrEmpty(pp[0]))
                pp = null;


            if (functionValueToken[1].TokenClassType == typeof(WordToken))
            {
                //specify parameters
                f = QsFunction.GetExactFunctionWithParameters(
                    Scope,
                    string.Empty,
                    functionValueToken[1].TokenValue,
                    pp
                );

            }
            else
            {
                //namespace included
                f = QsFunction.GetExactFunctionWithParameters(
                    Scope,
                    functionValueToken[1][0][0].TokenValue,
                    functionValueToken[1][1].TokenValue,
                    pp);
            }
        }
        else
        {
            throw new QsException("function value syntax error");
        }

        if (f != null)
        {
            return Expression.Constant(f.ToQuantity(unit.TrimTokens(1, 1).TokenValue).ToScalar(), typeof(QsScalar));
        }

        throw new QsVariableNotFoundException("Function (" + functionValueToken.TokenValue + ") not found");
    }

    /// <summary>
    /// Form the SymbolicQuantity expression.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Expression SymbolicScalar(Token token)
    {
        var s = string.Empty;
        var unit = "1";
        if (token.TokenClassType == typeof(SymbolicToken))
        {
            if (token.TokenValue.StartsWith("${"))
                s = token.TokenValue.Substring(2).TrimEnd('}');
            else
                s = token[1].TokenValue;
        }
        else
        {
            if (token[0].TokenValue.StartsWith("${"))
                s = token[0].TokenValue.Substring(2).TrimEnd('}');
            else
                s = token[0][1].TokenValue;


            unit = token[1].TokenValue.Trim('<', '>');
        }
        var sv  = SymbolicVariable.Parse(s);
        var SymbolicScalar = sv.ToQuantity(unit).ToScalar();

        return Expression.Constant(SymbolicScalar, typeof(QsValue));
    }
    #endregion

    #region Execution Global Code
    public QsValue Execute(string line)
    {
        ParseArithmatic(line);
        return Execute() as QsValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal object Execute()
    {

        // Construct Lambda function which return one object.
        Expression<Func<object>> cq = Expression.Lambda<Func<object>>(ResultExpression);

        // compile the function
        Func<object> aqf = cq.Compile();

        // execute the function
        var result = aqf();

        // return the result
        return result;
    }


    public QsScope Scope => Evaluator.Scope;

    #endregion

    public Type ScopeType => Scope.GetType();

    #region Expressions Generators

    /// <summary>
    /// Get Item with the specified indexing
    /// </summary>
    /// <param name="valueName"></param>
    /// <param name="indexes"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public Expression IndexerExpression(string valueName, Token indexes, Token args)
    {



        // this needs to be rewritten

        //  first we need to check our context if we are evaluating inside a function or inside a sequence
        //   then we evaluate the parameters   and we index based on the feeded parameters
        //     if parameters aren't available for that name then we proceed with global scope
        //     




        var ns = string.Empty;
        if (valueName.LastIndexOf(':') > -1) ns = valueName.Substring(0, valueName.LastIndexOf(':'));

        var valName = valueName.Substring(valueName.LastIndexOf(':') + 1);

        if (ParseMode == QsVarParseModes.Sequence && Sequence.Parameters.Count(s => s.Name.Equals(valueName, StringComparison.OrdinalIgnoreCase)) > 0)
        {
            // the parameters is coming from the sequence parameters
            return ValueIndexExpression(GetFunctionOrSequenceParameter(valueName), indexes);
        }

        if (ParseMode == QsVarParseModes.Function && Function.ParametersNames.Count(s => s.Equals(valueName, StringComparison.OrdinalIgnoreCase)) > 0)
        {
            return ValueIndexExpression(GetFunctionOrSequenceParameter(valueName), indexes);
        }
        // global mode
        var value = QsEvaluator.GetScopeValueOrNull(Scope, ns, valName) as QsValue;

        if (value == null)
        {

            // we are in global code context
            //   or we didn't find the variable name in the parameters list of the function
            return SequenceCallExpression(valueName, indexes, args);
        }

        // no there is a value there
        if (value is QsScalar)
        {
            var sc = (QsScalar)value;
            if (sc.ScalarType == ScalarTypes.FunctionQuantity)
            {
                return ValueIndexExpression(sc, indexes);
            }

            // oops Scalar definitely don't have elements inside it.
            return SequenceCallExpression(valueName, indexes, args);
        }

        // vector, matrix, or tensor.
        return ValueIndexExpression(value, indexes);
    }

    /// <summary>
    /// Evaluate vector, matrix, and tensor like v[i], m[i, j], or t[i, j, k]
    /// For contravariant tensor subscripts should be followed by underscore like t[5,8_,2] To be implemented later.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public Expression ValueIndexExpression(QsValue value, Token indexes)
    {
        var Tokens = indexes.TrimStart<LeftSquareBracketToken>().TrimEnd<RightSquareBracketToken>();
        Tokens = Tokens.MergeAllBut<MergedToken>(new CommaToken()).RemoveTokens(typeof(CommaToken));

        var mi =  typeof(QsValue).GetMethod("GetIndexedItem");

        Expression[] ExpressionParameters = new Expression[Tokens.Count];

        var mpm = typeof(QsParameter).GetMethod("MakeParameter");

        for (var ix = 0; ix < Tokens.Count; ix++)
        {
            var t = Tokens[ix].TokenClassType == typeof(MergedToken) ? Tokens[ix][0] : Tokens[ix];

            var vp = ParseArithmatic(t);

            ExpressionParameters[ix] = Expression.Call(
                mpm,
                vp,                                                                       //Evaluated value.
                Expression.Constant(Tokens[ix].TokenValue)                                //Raw Value
            );
        }

        Expression result = Expression.Call(Expression.Constant(value),
            mi,
            Expression.NewArrayInit(typeof(QsParameter), ExpressionParameters));

        return result;

    }

    public Expression ValueIndexExpression(Expression value, Token indexes)
    {

        var Tokens = indexes.TrimStart<LeftSquareBracketToken>().TrimEnd<RightSquareBracketToken>();
        Tokens = Tokens.MergeAllBut<MergedToken>(new CommaToken()).RemoveTokens(typeof(CommaToken));

        var mi = typeof(QsValue).GetMethod("GetIndexedItem");

        Expression[] ExpressionParameters = new Expression[Tokens.Count];

        var mpm = typeof(QsParameter).GetMethod("MakeParameter");

        for (var ix = 0; ix < Tokens.Count; ix++)
        {
            var t = Tokens[ix].TokenClassType == typeof(MergedToken) ? Tokens[ix][0] : Tokens[ix];

            var vp = ParseArithmatic(t);

            ExpressionParameters[ix] = Expression.Call(
                mpm,
                vp,                                                                       //Evaluated value.
                Expression.Constant(Tokens[ix].TokenValue)                                //Raw Value
            );
        }

        Expression result = Expression.Call(value,
            mi,
            Expression.NewArrayInit(typeof(QsParameter), ExpressionParameters));

        return result;

    }





    /// <summary>
    /// evaluate expression on the syntax S[n] or S[9..34](x1, x2, .., x7)  etc.
    /// </summary>
    /// <param name="sequenceName"></param>
    /// <param name="indexes"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public Expression SequenceCallExpression(string sequenceName, Token indexes, Token args)
    {

        //discover the index
        if (indexes.Count > 3) throw new QsException("Calling sequence with more than one index is not supported yet.");

        var indexText = indexes[1].TokenValue; //this is parameter between [.]  to be evaluated later
        if (indexText == "]") // empty square brackets
            indexText = QsSequence.DefaultIndexName;

        // discover sequence parameters.
        List<Expression> parameters = [];
        if (args != null)
        {
            //the sequence called with parameters
            parameters = [];

            //now parameters separated
            for (var ai = 1; ai < args.Count - 1; ai++)
            {
                if (args[ai].TokenValue != ",")
                {

                    Expression rawParameter = Expression.Constant(args[ai].TokenValue.Trim());

                    var nakedParameter = ParseArithmatic(args[ai]);

                    // if this was another function name without like  v(c,g,8)  where g is a function name will be passed to c
                    //  then excpetion will occur in GetQuantity that variable was not found in the scope

                    Expression tryBody = Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"),
                        nakedParameter,
                        rawParameter);


                    // -> Catch(QsVariableNotFoundException e) {QsParameter.MakeParameter(e.ExtraData, args[ip])}

                    var e = Expression.Parameter(typeof(QsVariableNotFoundException), "e");
                    Expression catchBody = Expression.Call(typeof(QsParameter).GetMethod("MakeParameter"),
                        Expression.Property(e, "ExtraData"),
                        rawParameter);

                    // The try catch block when catch exception will execute the call but by passing the parameter as text only
                    /*
                    var tt = Utils.Try(tryBody);

                    tt.Catch(e, catchBody);
                     Expression tryc  = tt.ToExpression()
                     */
                    Expression tryc = Expression.TryCatch(tryBody, Expression.Catch(e, catchBody));


                    parameters.Add(tryc);
                }
            }
        }

        // discover namespace

        var sequenceNamespace = string.Empty;
        if (sequenceName.IndexOf(':') > -1) sequenceNamespace = sequenceName.Substring(0, sequenceName.LastIndexOf(':'));

        var seqCallName = sequenceName.Substring(sequenceName.LastIndexOf(':') + 1);

        var seqo = QsSequence.FormSequenceSymbolicName(seqCallName, 1, parameters.Count);

        //get the sequence dynamically because sequence can be recursive :)

        Expression QsSequExpression = Expression.Call(typeof(QsSequence).GetMethod("GetSequence"),
            Expression.Constant(Evaluator.Scope),
            Expression.Constant(sequenceNamespace),
            Expression.Constant(seqo)
        );

        #region Index manipulation
        Expression IndexExpression = null;
        Expression FromExpression = null;
        Expression ToExpression = null;

        // now the index could be
        // integer number
        int n;

        var methodName = "GetElementValue";

        var itok = Token.ParseText(indexText);
        itok = itok.RemoveSpaceTokens();
        itok = itok.MergeTokens<WordToken>();

        itok = itok.MergeAllBut(typeof(WordToken), new SequenceRangeToken());

        if (itok.Count == 3 && itok[1].TokenClassType == typeof(SequenceRangeToken))
        {
            #region From To and Range Function
            FromExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQsValue"), ParseArithmatic(itok[0].TokenValue));
            ToExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQsValue"), ParseArithmatic(itok[2].TokenValue));

            if (itok[1].TokenValue == "++")
                methodName = "SumElements";
            else if (itok[1].TokenValue == "!!")
                methodName = "Average";
            else if (itok[1].TokenValue == "**")
                methodName = "MulElements";
            else if (itok[1].TokenValue == "..")
                methodName = "QsValueElements";
            else if (itok[1].TokenValue == "!%")
                methodName = "StdDeviation";
            #endregion
        }
        else if (int.TryParse(indexText, out n))
        {
            IndexExpression = Expression.Constant(n, typeof(int));
        }
        else
        {
            // expression to be calculated
            //     variable that should be manipulated from the sequence body if we are declaring sequence.
            //     variable from the sequence parameters

            IndexExpression = ParseArithmatic(indexText);

            //however we need to convert it to integer. because it is an index
            IndexExpression = Expression.Call(typeof(Qs).GetMethod("IntegerFromQsValue"), IndexExpression);

        }
        #endregion

        #region GetElementValue determination
        MethodInfo? mi;
        if (methodName == "GetElementValue")
        {
            #region One Element Value
            switch (parameters.Count)
            {
                case 0:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int)]);
                    break;
                case 1:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter)]);
                    break;
                case 2:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 3:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 4:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 5:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 6:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 7:
                    mi = typeof(QsSequence).GetMethod(methodName, [typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                default:
                    throw new QsException("Number of sequence parameters exceed the built in functionality");
            }
            // insert the index parameter.
            parameters.Insert(0, IndexExpression);
            #endregion
        }
        else
        {
            #region Range Operation
            const string RangeOperation = "RangeOperation";
            switch (parameters.Count)
            {
                case 0:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int)]);
                    break;
                case 1:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter)
                    ]);
                    break;
                case 2:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 3:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 4:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 5:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 6:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                case 7:
                    mi = typeof(QsSequence).GetMethod(RangeOperation, [typeof(string), typeof(int), typeof(int), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter), typeof(QsParameter)
                    ]);
                    break;
                default:
                    throw new QsException("Number of sequence parameters exceed the built in functionality");
            }
            // insert the index parameter.
            parameters.Insert(0, ToExpression);
            parameters.Insert(0, FromExpression);
            parameters.Insert(0, Expression.Constant(methodName));
            #endregion
        }

        #endregion

        var finale = Expression.Call(QsSequExpression
            , mi
            , parameters.ToArray()
        );

        return finale;

    }



    /// <summary>
    /// Used to build the function call expression
    /// either if called directly or called from another function
    /// </summary>
    /// <param name="functionName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public Expression FunctionCallExpression(string functionName, Token args)
    {
        //fn(x, y, ff(y/x, e + fr(d)))     <== sample form :D

        List<Token> ArgumentTokens = [];

        //discover parameters
        for (var ai = 1; ai < args.Count - 1; ai++ )
        {
            if (args[ai].TokenValue != ",")
            {
                ArgumentTokens.Add(args[ai]);
            }
        }

        // discover namespace
        var functionNameSpace = "";

        if (functionName.LastIndexOf(':') > -1) functionNameSpace = functionName.Substring(0, functionName.LastIndexOf(':'));

        // function name part after namespace
        var funcCallName = functionName.Substring(functionName.LastIndexOf(':') + 1);

        //now parameters separated

        // search for suitable function to be called.
        // 1- if the parameters are raw parameters without named argument then get the default function
        // 2- if one of the parameters contain named argument then search for this function.

        var NamedArgumentOccured = false;

        List<string> argNames = [];

        foreach (var argToken in ArgumentTokens)
        {
            var paraToken = argToken.RemoveSpaceTokens();

            if (paraToken.Contains<EqualToken>())
            {
                argNames.Add(paraToken[0].TokenValue);  //trim the argument name ;)
                NamedArgumentOccured = true;
            }
            else
            {
                if (NamedArgumentOccured) throw new QsException("Normal argument after named argument is not permitted");
            }
        }

        var FirstNamedArgumentIndex = ArgumentTokens.Count - argNames.Count; //point to the index of the first named argument in the caller.

        #region Specifing the target function
        QsFunction TargetFunction;

        if (argNames.Count > 0)
        {
            #region Named Arguments discovery
            // get all the functions that contain these named argument parameters.

            var DiscoveredFunctions = QsFunction.FindFunctionByParameters(Scope, functionNameSpace, funcCallName, ArgumentTokens.Count, argNames.ToArray());

            if (DiscoveredFunctions.Length == 0)
            {
                throw new QsFunctionNotFoundException("No functions found at all, please review your parameters");
            }
            if (DiscoveredFunctions.Length > 1)
            {

                //we need to make another filtering criteria to decrease the discovered numbers.

                List<QsFunction> Pass2Functions = [];

                foreach (var func in DiscoveredFunctions)
                {
                    var include = true;
                    for (var ix = 0; ix < FirstNamedArgumentIndex; ix++)
                    {
                        var calleeParam = func.Parameters[ix].Name;

                        //does this parameter lie in the named arguments one.

                        // for example in case of f(4, U:=3, V:=1)
                        //   and current called function is f(u, r, v)
                        //    then u exist in the list of {U:=3, V:=1}
                        //     which exclude this function from the Pass2Functions.

                        if (argNames.Contains(calleeParam, StringComparer.OrdinalIgnoreCase))
                        {
                            include = false;
                            break;
                        }
                    }

                    if (include) Pass2Functions.Add(func);
                }

                if (Pass2Functions.Count == 0)
                    throw new QsFunctionNotFoundException("No function from the (" + DiscoveredFunctions.Length.ToString(CultureInfo.InvariantCulture) + ") discovered is having suitable parameters order");
                if (Pass2Functions.Count == 1)
                    TargetFunction = Pass2Functions[0];
                else
                    throw new QsFunctionNotFoundException("(" + Pass2Functions.Count.ToString(CultureInfo.InvariantCulture) + ") functions, please specify more named arguments. ");
                #endregion

            }
            else
            {
                TargetFunction = DiscoveredFunctions[0];
            }

        }
        else
        {
            // specify the function real name
            var TargetFunctionRealName =
                QsFunction.FormFunctionSymbolicName(funcCallName, ArgumentTokens.Count); //to call the right function

            TargetFunction = QsFunction.GetFunction(Scope, functionNameSpace, TargetFunctionRealName);
        }
        #endregion


        List<Expression> parameters = [];

        #region Helper delegates
        //delegate to check if the function name in the Function object of the current function expression formation or if parsemode = function
        Func<string, Expression> ParameterFunction = delegate(string funcName)
        {
            //this anonymous function is calling the function dynamically from the passed function argument.

            //but may be the function name should be retrieved from a parameter.
            int parametersCount;
            if (ParseMode == QsVarParseModes.Function)
                parametersCount = Function.Parameters.Count(c => c.Name.Equals(funcName, StringComparison.OrdinalIgnoreCase));
            else if (ParseMode == QsVarParseModes.Sequence)
                parametersCount = Sequence.Parameters.Count(c => c.Name.Equals(funcName, StringComparison.OrdinalIgnoreCase));
            else
                throw new QsException("Parse mode is not known to evaluate this function");

            if (parametersCount > 0)
            {
                QsParamInfo pinfo; //the parameter in the parent function that hold the function name.
                if (ParseMode == QsVarParseModes.Function)
                    pinfo = Function.Parameters.Single(c => c.Name.Equals( funcName, StringComparison.OrdinalIgnoreCase));
                else if (ParseMode == QsVarParseModes.Sequence)
                    pinfo = Sequence.Parameters.Single(c => c.Name.Equals(funcName, StringComparison.OrdinalIgnoreCase));
                else
                    throw new QsException("Parse mode is not known to evaluate this function");



                //Yes the function should be retrieved dynamically from the passed parameter name.

                // THIS IS THE RIGHT SIDE OF EVALUATING FUNCTION    i.e. f(a) = 5+a(4,2)+a
                // ------------------------------------------------------------------------

                // Prepare the expression that will execute this function dynamically

                // Get the function name from the parameter
                Expression FunctionParameter = lambdaBuilder.Parameters.Single(c => c.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase));

                Expression FunctionParameterName = Expression.Call(FunctionParameter,
                    typeof(QsParameter).GetMethod("GetTrueFunctionName"),
                    Expression.Constant(ArgumentTokens.Count)
                );

                // Get the Function object.
                Expression Functor = Expression.Call(typeof(QsFunction).GetMethod("GetFunctionAndThrowIfNotFound"),
                    Expression.Constant(Scope),
                    FunctionParameterName
                );

                //prepare arguments.
                //  evaluate the inner arguments of this function  v(x, c , b) = c(b(x), x-2,x/4,40)
                //   b(x)  //will be evaluate to another late discover expression.
                //   x-2
                //   x/4
                //   40
                List<Expression> FunctorParams = [];
                foreach (var tk in ArgumentTokens)
                {
                    var prm = tk.TokenValue;
                    var q = ParseArithmatic(prm); // evaluate the parameter
                    Expression rw;
                    try
                    {
                        rw = lambdaBuilder.Parameters.Single(c => c.Name.Equals( prm, StringComparison.OrdinalIgnoreCase));
                        rw = Expression.Property(rw, "ParameterRawText");  //raw value that was send with this parameter.
                    }
                    catch
                    {
                        rw = Expression.Constant(prm);   // the hard coded parameter because finding the parameter value failed
                        //occurs if the passing parameter contains expression not standalone variable.
                    }

                    /*
                    var tc = Utils.Try(
                     Expression.Call(
                        typeof(QsParameter).GetMethod("MakeParameter"),
                        q,                                //Evaluated value.
                        rw                                //Raw Value
                        ));

                    tc.Catch(typeof(QsVariableNotFoundException),
                        Expression.Call(
                        typeof(QsParameter).GetMethod("MakeParameter"),
                        Expression.Constant(null),                                //no value
                        rw                                //Raw Value
                        ));

                    Expression qp = tc.ToExpression();
                     */

                    Expression qp = Expression.TryCatch(
                        Expression.Call(
                            typeof(QsParameter).GetMethod("MakeParameter"),
                            q,                                //Evaluated value.
                            rw                                //Raw Value
                        )
                        ,
                        Expression.Catch(typeof(QsVariableNotFoundException),
                            Expression.Call(
                                typeof(QsParameter).GetMethod("MakeParameter"),
                                Expression.Constant(null),                                //no value
                                rw                                //Raw Value
                            )
                        )
                    );


                    FunctorParams.Add(qp);

                }

                // expression to call the function QsFunction.GetInvoke  is used here
                Expression CallExpression = Expression.Call(Functor,
                    typeof(QsFunction).GetMethod("GetInvoke"),
                    Expression.NewArrayInit(typeof(QsParameter), FunctorParams)
                );

                //set that the parameter is function.
                pinfo.Type = QsParamType.Function;


                return CallExpression;
            }

            return null;
        };

        #endregion

        if (TargetFunction != null)
        {
            // Means a function is already available in the global heap to be called.
            // so we can call it directly if we wish


            // check if the function name is originally come from the parameters.
            if (ParseMode == QsVarParseModes.Function)
            {
                //but if the function name is from parameter name
                // then we can't call it from global heap
                // we have to get the parameter name that was passed to the function
                //  and call it dynamically from there.

                var FunP = ParameterFunction(functionName); //caling the helper delegate.
                if (FunP != null) return FunP;
            }


            #region Adjusting (ReArranging) the named arguments order to the real function parameter order

            //Rearrange paramsText so it includes named arguments
            //  named argument is on the format  x=20, j=30


            // fParameters is the parameters array that will be sent to the function
            Dictionary<string, string> fParameters = new(StringComparer.OrdinalIgnoreCase);

            // setParameters ensure that the parameter wasn't set before.
            Dictionary<string, bool> setParameters = new(StringComparer.OrdinalIgnoreCase);

            // first add all values of parameters in dictionary of ParameterName: Value
            for (var i = 0; i < ArgumentTokens.Count; i++)
            {
                fParameters.Add(TargetFunction.Parameters[i].Name, ArgumentTokens[i].TokenValue);
                setParameters.Add(TargetFunction.Parameters[i].Name, false);
            }

            #region loop and put named argument in its correct position.
            NamedArgumentOccured = false;

            // second search for named arguments and replace the old values.
            for (var i = 0; i < ArgumentTokens.Count; i++)
            {

                var paraToken = ArgumentTokens[i].RemoveSpaceTokens();

                if (paraToken.Contains<EqualToken>())
                {
                    var paramName = paraToken[0].TokenValue;
                    var paramVal = paraToken.SubTokensValue(2);   //take the rest of parameter after = sign

                    if (fParameters.ContainsKey(paramName))
                    {
                        if (!setParameters[paramName]) //check if the parameter was set before.
                            fParameters[paramName] = paramVal;  //put the value in its corresponding parameter.
                        else
                            throw new QsException("Parameter '" + paramName + "' was set before");

                        setParameters[paramName] = true;
                    }
                    else
                    {
                        throw new QsException("Named parameter '" + paramName + "' not found in the function: " + TargetFunction.FunctionDeclaration);
                    }

                    NamedArgumentOccured = true;
                }
                else
                {
                    if (NamedArgumentOccured) throw new QsException("Normal argument after named argument is not permitted");
                    setParameters[TargetFunction.Parameters[i].Name] = true;
                }


            }
            #endregion

            #endregion

            // Function exist in global heap of the scope
            //  send values of corrected ordinal (positions parameters)
            return TargetFunction.GetInvokeExpression(this, fParameters.Values.ToArray<string>());
        }
        //Why we can't find the function in the global heap.

        if (ParseMode == QsVarParseModes.Function) // are we in function parsing mode?
        {
            var FunP = ParameterFunction(functionName); //try to get the function from the parameter

            if (FunP != null) return FunP;
        }

        throw new QsFunctionNotFoundException(functionName + " Can't be found.");

    }



    /// <summary>
    /// Get the parameter expression when parsing in function mode
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    Expression GetFunctionOrSequenceParameter(string parameterName)
    {
        //get it from the parameters of the lambda
        //  :) if it is found here then it will not be obtained from the global heap :)
        //      now I understand how variable scopes occure :D

        try
        {
            Expression eu = lambdaBuilder.Parameters.Single(c => c.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase));


            // parameter is having quantity in normal cases
            Expression directQuantity = Expression.Property(eu, "QsNativeValue");

            // another expression for getting the quantity from the variable passed
            //   this case only occure if I called a function declared like this   f(a) = a(5,3)+a
            //      because when calling f(u)  where u=50 and u(x,y)=x+y  then I want the evaluation to get the calculations right
            //      because 'a' alone is expressing single global variable.



            Expression indirectQuantity = Expression.Call(
                eu,
                typeof(QsParameter).GetMethod("GetIndirectQuantity"),
                Expression.Constant(Scope)
            );

            //if the quantity in parameter == null then get it as indirect quantity from the scope of the variables

            return Expression.Condition(
                Expression.Equal(directQuantity, Expression.Constant(null)),
                indirectQuantity,
                directQuantity
            );

        }
        catch (Exception e)
        {
            //System.Diagnostics.Debug.Print(e.ToString());

            //quantity variable  //get it from evaluator  global heap

            //return GetQsVariable(parameterName.tok

            throw new QsParameterNotFoundException(parameterName);
        }

    }



    /// <summary>
    /// Just take the left and right expression with the operator and make arithmatic expression.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="op"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private Expression ArithExpression(ExprOp eop, out short skip)
    {
        var left = eop.QuantityExpression;
        var op = eop.Operation;
        var right = eop.Next.QuantityExpression;

        skip = 1;

        var aqType = typeof(QsValue);

        if (op == ":") return Expression.Call(left, aqType.GetMethod("ColonOperator"), right);

        if (op == "!") return Expression.Call(left, aqType.GetMethod("ExclamationOperator"), right);

        if (op == "->") return Expression.Call(left, aqType.GetMethod("Execute"), right);

        if (op == "..") return Expression.Call(left, aqType.GetMethod("RangeOperation"), right);

        if (op == "_h*") return Expression.Multiply(left, right);  // higher multiplication priority in case of 5^-3 to be 5^(-1*3) == 5^-1_h*3

        if (op == "^")
        {
            // This will be right associative operator
            // which means if more than one power appeared like this 3^2^4  then it will be processed like this 3^(2^4)

            if (eop.Next.Next != null)
            {
                if (eop.Next.Operation == "^")
                {
                    short iskip;
                    var powerResult = Expression.Power(left, ArithExpression
                    (
                        eop.Next, out iskip
                    ), aqType.GetMethod("Pow"));

                    skip += iskip;
                    return powerResult;
                }

                return Expression.Power(left, right, aqType.GetMethod("Pow"));
            }

            return Expression.Power(left, right, aqType.GetMethod("Pow"));
        }

        if (op == "^.") return Expression.Power(left, right, aqType.GetMethod("PowDot"));

        if (op.Equals("^x", StringComparison.OrdinalIgnoreCase))
            return Expression.Power(left, right, aqType.GetMethod("PowCross"));

        if (op.Equals("|"))
            return Expression.Call(left, aqType.GetMethod("DifferentiateOperation"), right);

        if (op == "*") return Expression.Multiply(left, right);

        if (op == ".") return Expression.Multiply(left, right, aqType.GetMethod("DotProduct"));

        if (op.Equals("x", StringComparison.OrdinalIgnoreCase))
            return Expression.Multiply(left, right, aqType.GetMethod("CrossProduct"));

        if (op.Equals("(*)", StringComparison.OrdinalIgnoreCase))
            return Expression.Multiply(left, right, aqType.GetMethod("TensorProduct"));

        if (op == "/") return Expression.Divide(left, right);
        if (op == "%") return Expression.Modulo(left, right);
        if (op == "+") return Expression.Add(left, right);
        if (op == "-") return Expression.Subtract(left, right);

        if (op == "<<") return Expression.LeftShift(left, right, aqType.GetMethod("LeftShiftOperator"));
        if (op == ">>") return Expression.RightShift(left, right, aqType.GetMethod("RightShiftOperator"));

        if (op == "<") return Expression.LessThan(left, right);
        if (op == ">") return Expression.GreaterThan(left, right);
        if (op == "<=") return Expression.LessThanOrEqual(left, right);
        if (op == ">=") return Expression.GreaterThanOrEqual(left, right);

        if (op == "==") return Expression.Equal(left, right, true, aqType.GetMethod("op_Equality"));
        if (op == "!=") return Expression.NotEqual(left, right, true, aqType.GetMethod("op_Inequality"));

        if (op.Equals("and", StringComparison.OrdinalIgnoreCase))
            return Expression.And(left, right);

        if (op.Equals("or", StringComparison.OrdinalIgnoreCase))
            return Expression.Or(left, right);

        if (op.Equals("when", StringComparison.OrdinalIgnoreCase))
        {
            // when -> condition -> otherwise -> result
            // here we must consume another expression for the false evaluation.
            //   if there are more expression on the linked list


            if (eop.Next.Next == null)
            {
                throw new QsException("syntax error: When without Otherwise");
            }

            skip = 2;

            //eop:=when .Next=Expression .Next=otherwise .Net=expression

            //There is a node test if it is a when node.
            if (eop.Next.Next.Operation.Equals("when", StringComparison.OrdinalIgnoreCase))
            {
                // yes there is a node when that should its body processed
                short innerSkip;

                var CompoundWhenExpression = ArithExpression(eop.Next.Next, out innerSkip);  //send the next when node

                skip += innerSkip;  // to know how many nodes we skipped.

                var WhenExpression = Expression.Condition(right, left, CompoundWhenExpression, typeof(QsValue));
                return WhenExpression;
            }
            else
            {
                var WhenExpression = Expression.Condition(right, left, eop.Next.Next.QuantityExpression, typeof(QsValue));
                return WhenExpression;
            }
        }

        throw new NotSupportedException("Not Supported Operator '" + op + "'");
    }


    #region Operators and Priority Groups
    //Treat operators as groups
    //  means * and /  are in the same pass
    //  + and - are in the same pass

    // passes depends on priorities of operators.

    // Internal Higher Priority Group
    readonly static string[] HigherGroup =
    [
        "_h*" /* Higher Multiplication priority used internally in
                                              the case of -4  or 5^-3
                                              To be treated like -1_h*4   or 5^-1_h*4
                                           */
    ];

    readonly static string[] Group =
    [
        "^"    /* Power for normal product '*' */,
        "^."   /* Power for dot product */,
        "^x"   /* Power for cross product */,
        ".."   /* Vector Range operator {from..to} */,
        "->"   /* object member calling */,
        "!"    /* Exclamation operation */,
        ":"    /* colon operation*/
    ];

    readonly static string[] SymGroup = ["|" /* Derivation operator */];

    readonly static string[] Group0 = ["x"   /* cross product */];

    readonly static string[] Group1 =
    [
        "*"   /* normal multiplication */,
        "."   /* dot product */,
        "(*)" /* Tensor product */,
        "/"   /* normal division */,
        "%"   /* modulus */
    ];

    readonly static string[] Group2 = ["+", "-"];

    readonly static string[] Shift = ["<<", ">>"];

    readonly static string[] RelationalGroup = ["<", ">", "<=", ">="];
    readonly static string[] EqualityGroup = ["==", "!="];
    readonly static string[] AndGroup = ["and"];
    readonly static string[] OrGroup = ["or"];

    readonly static string[] WhenOtherwiseGroup = ["when", "otherwise"];


    /// Operator Groups Ordered by Priorities.
    readonly static string[][] OperatorGroups = [HigherGroup, Group, SymGroup, Group0, Group1, Group2, Shift, RelationalGroup, EqualityGroup, AndGroup, OrGroup, WhenOtherwiseGroup
    ];

    #endregion


    /// <summary>
    /// Takes the linked list of formed expressions and construct the arithmatic expressions based
    /// on the priority of calculation operators.
    /// Passes:
    /// 1- { "^" }
    /// 2- { "*", "/", "%" /*modulus*/ }
    /// 3- { "+", "-" }
    /// </summary>
    /// <param name="FirstEop"></param>
    /// <returns></returns>
    private Expression ConstructExpression(ExprOp FirstEop)
    {

        foreach (var opg in OperatorGroups)
        {
            var eop = FirstEop;

            //Pass for '[op]' and merge it  but from top to child :)  {forward)
            while (eop.Next != null)
            {
                //if the operator in node found in the opg (current operator group) then execute the logic

                if (opg.Count(c => c.Equals(eop.Operation, StringComparison.OrdinalIgnoreCase)) > 0)
                {
                    short skip;
                    eop.QuantityExpression = ArithExpression(eop, out skip);

                    //drop eop.Next
                    if (eop.Next.Next != null)
                    {
                        while (skip > 0)
                        {
                            eop.Operation = eop.Next.Operation;

                            eop.Next = eop.Next.Next;

                            skip--;
                        }
                    }
                    else
                    {
                        //no more nodes exit the loop

                        eop.Next = null;      //last item were processed.
                        eop.Operation = string.Empty;
                    }
                }
                else
                {
                    eop = eop.Next;
                }
            }
        }

        return FirstEop.QuantityExpression;
    }


    /// <summary>
    /// The final result of the parsing and expression tree construction.
    /// </summary>
    internal Expression ResultExpression
    {
        get;
        set;
    }
    #endregion

}