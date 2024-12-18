﻿namespace SymbolicAlgebra;

public partial class SymbolicVariable
{

    /*
     * https://socratic.org/calculus/techniques-of-integration/integration-by-parts
     * https://socratic.org/questions/use-the-method-of-integration-by-parts-to-evaluate-int-x-2-sin-pix-dx
     * https://socratic.org/questions/use-the-method-of-integration-by-parts-to-evaluate-intx-2-xdx
     * https://www.dummies.com/article/academics-the-arts/math/calculus/using-the-product-rule-to-integrate-the-product-of-two-functions-179219/
     * https://en.wikipedia.org/wiki/Error_function

     the ILATE method.

        Inverse Trig
        Logarithms
        Algebra
        Trig
        Exponentials

    ILATE stands for:

        I: Inverse trigonometric functions : arctan x, arcsec x, arcsin x etc.

        L: Logarithmic functions : ln x, log5(x), etc.

        A: Algebraic functions.

        T: Trigonometric functions, such as sin x, cos x, tan x etc.

        E: Exponential functions.



        ∫u dv = uv − ∫v du

        we select u and v based on the list of priorities above


     */

    /// <summary>
    /// Integrate one pure term.
    /// </summary>
    /// <param name="sv"></param>
    /// <param name="parameter"></param>
    private static SymbolicVariable? IntegTerm(SymbolicVariable term, string parameter)
    {
        var sv = term.Clone();

        var symbolpowercontainParameter = false;
        if (sv._SymbolPowerTerm != null)
        {
            if (sv._SymbolPowerTerm.InvolvedSymbols.Contains(parameter, StringComparer.OrdinalIgnoreCase))
            {
                symbolpowercontainParameter = true;
            }
            else
            {
                var prcount = sv._SymbolPowerTerm.FusedSymbols.Count(p => p.Key.Equals(parameter, StringComparison.OrdinalIgnoreCase));
                symbolpowercontainParameter = prcount > 0;
            }
        }

        var cc = false;
        if (sv.BaseVariable != null) cc = sv.BaseVariable.InvolvedSymbols.Contains(parameter, StringComparer.OrdinalIgnoreCase); // case of base variable
        else if (sv.IsFunction && symbolpowercontainParameter == true)
        {
            // search if a parameter contains the same parameter
            foreach (var pf in sv._FunctionParameters)
                if (pf.Symbol.Equals(parameter, StringComparison.OrdinalIgnoreCase))
                {
                    cc = true;
                    break;
                }
        }
        else if(sv.IsFunction && sv.InvolvedSymbols.Contains(parameter, StringComparer.OrdinalIgnoreCase) == false)
        {
            cc = true;  // treat the symvar  as an algebraic because there is no variable inside parameters that resemble what we want.
        }
        else
        {
            cc = sv.Symbol.Equals(parameter, StringComparison.OrdinalIgnoreCase);
        }

        // x^3*y^2*z^5    , integrate to x;
        if (cc)
        {
            if (sv._SymbolPowerTerm == null)
            {
                if (sv.IsFunction)
                {
                    sv = Multiply(sv, new SymbolicVariable(parameter));
                }
                else if (sv._SymbolPower + 1 == 0)  // case of 1/x
                {
                    sv._Symbol = $"{FunctionOperation.LnText}({parameter.ToLowerInvariant()})";
                    sv._SymbolPower = 1;
                }
                else
                {
                    sv._SymbolPower += 1;
                    sv.Coeffecient = sv.Coeffecient / sv._SymbolPower;
                    sv._Symbol = parameter;
                }
            }
            else
            {
                if (symbolpowercontainParameter)
                {
                    // here is the case when base and power are the same with the parameter we are differentiating with
                    //  i.e.  x^x . x
                    // Logarithmic integration
                    var lnterm = new SymbolicVariable($"{FunctionOperation.LnText}(" + sv.ToString() + ")");
                    var dlnterm = lnterm.Integrate(parameter);
                    sv = Divide(sv, dlnterm);
                    throw new NotImplementedException();

                }
                else
                {
                    // symbol power term exist
                    sv._SymbolPowerTerm = Add(sv._SymbolPowerTerm, One);
                    sv = Divide(sv, sv._SymbolPowerTerm);
                }
            }
        }
        else if (symbolpowercontainParameter)
        {
            // this case is when the power term is the same
            var log = new SymbolicVariable(FunctionOperation.LnText + "(" + sv.Symbol + ")");
            var dp = sv._SymbolPowerTerm.Integrate(parameter);
            sv = Divide(log, Divide(dp, sv));
            throw new NotImplementedException();
        }
        else
        {
            // try in the fused variables
            HybridVariable hv;
            if (sv.FusedSymbols.TryGetValue(parameter, out hv))
            {
                if (hv.SymbolicVariable == null)
                {
                    if(hv.NumericalVariable + 1 == 0)
                    {
                        hv.NumericalVariable = 1;
                        hv.SymbolicVariable._Symbol = $"{FunctionOperation.LnText}({parameter.ToLowerInvariant()})";
                    }
                    else
                    {
                        hv.NumericalVariable += 1;
                        sv.Coeffecient = sv.Coeffecient / hv.NumericalVariable;

                    }
                    sv._FusedSymbols[parameter] = hv;
                }
                else
                {
                    // symbol power term exist

                    hv.SymbolicVariable = Add(hv.SymbolicVariable, One);
                    sv._FusedSymbols[parameter] = hv;

                    sv = Divide(sv, hv.SymbolicVariable);
                }
            }
            else
            {
                if (sv.IsFunction)
                {
                    var fv = sv.Clone();

                    // remove the power term in this copied function term
                    fv._SymbolPowerTerm = null;
                    fv.SymbolPower = 1.0;
                    fv.Coeffecient = 1.0;


                    if (fv.FunctionName.Equals(FunctionOperation.ExpText, StringComparison.OrdinalIgnoreCase))
                    {
                        if (fv._FunctionParameters.Length != 1) throw new SymbolicException("Exp function must have one parameter for differentiation to be done.");

                        // ∫ e^x = e^x
                        // ∫ exp(x*y) = exp(x*y)/y

                        var spara = new SymbolicVariable(parameter);

                        var pa = fv.FunctionParameters[0];

                        var divo = Divide(pa, spara);

                        if (divo.IsOne) return fv;
                        else return Divide(fv, divo);

                    }
                    else if (fv.FunctionName.Equals(FunctionOperation.LnText, StringComparison.OrdinalIgnoreCase))
                    {
                        if (fv._FunctionParameters.Length != 1) throw new SymbolicException("Log function must have one parameter for differentiation to be done.");

                        // actually  .. in here we need to factorize the paramter if it was a complex equation

                        // ∫u dv = uv − ∫v du

                        // so  ∫ln(x) dx  by parts  is

                        // u  = ln x
                        // dv = dx
                        // v  = x
                        // uv = x ln x
                        // du = 1/x  dx
                        // ∫v du   = ∫ x * 1/x dx
                        //         = ∫ x/x dx
                        //         = ∫ 1 dx
                        //         =  x
                        // ∫ln(x) dx = ∫u dv = uv − ∫v du = x ln x − x


                        // ∫ log(x^2+x) dx
                        // ----------------
                        // u = log(x^2+x)
                        // dv = dx
                        // v = x
                        // uv = x*log(x^2+x)
                        // du = (2*x+1)/(x^2+x) dx
                        // ∫v du   = ∫ x * (2*x+1)/(x^2+x) dx
                        //         = ∫ (2*x^2+x)/(x^2+x) dx
                        //         = 2*x-log(x+1)

                        // uv − ∫v du = x*log(x^2+x) - (2*x + log(x+1) )

                        var u = fv.Clone();
                        var v = new SymbolicVariable(parameter);
                        var uv = Multiply(u, v);

                        var du = u.Differentiate(parameter);
                        var vdu = Multiply(v, du);
                        var ivdu = vdu.Integrate(parameter);

                        fv = Subtract(uv, ivdu);

                        /*
                        var pa = fv._FunctionParameters[0];
                        var hh = SymbolicVariable.Subtract(fv, SymbolicVariable.One);
                        fv = SymbolicVariable.Multiply(pa, hh);
                        */

                    }
                    else if (fv.FunctionName.Equals(FunctionOperation.SqrtText, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new NotImplementedException();
                        // d/dx ( sqrt( g(x) ) ) = g'(x) / 2* sqrt(g(x))
                        if (fv._FunctionParameters.Length != 1) throw new SymbolicException("Sqrt function must have one parameter for differentiation to be done.");

                        var pa = fv._FunctionParameters[0];
                        var dpa = pa.Differentiate(parameter);
                        var den = Multiply(Two, fv);
                        fv = Divide(dpa, den);

                    }
                    else if (FunctionOperation.InversedTrigFunctions.Contains(fv.FunctionName, StringComparer.OrdinalIgnoreCase))
                    {
                        fv = FunctionOperation.IntegInversedTrigFunction(fv, parameter);
                    }
                    else if (FunctionOperation.TrigFunctions.Contains(fv.FunctionName, StringComparer.OrdinalIgnoreCase))
                    {
                        // triogonometric functions

                        fv = FunctionOperation.IntegTrigFunction(fv, parameter);


                    }

                    else
                    {
                        throw new NotImplementedException();


                        // the function is not a special function like sin, cos, and log.
                        // search for the function in the running context.


                        var extendedFunction = Functions.Keys.FirstOrDefault(c => c.StartsWith(fv.FunctionName, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(extendedFunction))
                        {
                            string[] fps = extendedFunction.Substring(extendedFunction.IndexOf("(")).TrimStart('(').TrimEnd(')').Split(',');

                            if (fps.Length != fv._RawFunctionParameters.Length) throw new SymbolicException("Insufficient function parameters");

                            // replace parameters
                            var dsf = Functions[extendedFunction].ToString();

                            for (var ipxf = 0; ipxf < fps.Length; ipxf++)
                            {
                                dsf = dsf.Replace(fps[ipxf], fv._RawFunctionParameters[ipxf]);
                            }

                            fv = Parse(dsf).Differentiate(parameter);

                        }
                        else
                        {
                            throw new SymbolicException("This function is not a special function, and I haven't implemented storing user functions in the running context");
                        }

                    }

                    // second treat the function normally as if it is one big symbol
                    if (sv._SymbolPowerTerm == null)
                    {
                        sv.Coeffecient *= sv._SymbolPower;
                        sv._SymbolPower -= 1;
                        if (sv._SymbolPower == 0) sv._Symbol = "";
                    }
                    else
                    {
                        // symbol power term exist
                        var oldPower = sv._SymbolPowerTerm;
                        sv._SymbolPowerTerm = Subtract(sv._SymbolPowerTerm, One);
                        sv = Multiply(sv, oldPower);
                    }


                    sv = Multiply(fv, sv);
                }
                else if (sv.IsCoeffecientOnly && sv._CoeffecientPowerTerm != null)
                {
                    throw new NotImplementedException();

                    // hint: the coeffecient only term has power of 1 or symbolic power should exist in case of raise to symbolic power

                    // get log(coeffeniect)
                    var log = new SymbolicVariable(FunctionOperation.LnText + "(" + sv.Coeffecient.ToString(CultureInfo.InvariantCulture) + ")");
                    var dp = sv._CoeffecientPowerTerm.Differentiate(parameter);
                    sv = Multiply(log, Multiply(dp, sv));
                }
                else
                {
                    // coefficient only

                    if (string.IsNullOrEmpty(sv._Symbol))
                    {
                        sv._Symbol = parameter;
                        sv._SymbolPower = 1;

                    }
                    else
                    {
                        // we add it to the fused symbols
                        sv.FusedSymbols.Add(parameter, new HybridVariable { NumericalVariable = 1, SymbolicVariable = null });

                        //sv = SymbolicVariable.Multiply(sv, new SymbolicVariable(parameter));
                    }

                    // empty everything :)
                    //sv._SymbolPowerTerm = null;
                    //sv._CoeffecientPowerTerm = null;
                    //sv.Coeffecient = 0;
                    //sv._DividedTerm = null;
                    //sv._BaseVariable = null;
                    //if (sv._SymbolPowerTerm != null)
                    //{
                    //    sv._FusedSymbols.Clear();
                    //    sv._FusedSymbols = null;
                    //}

                }
            }
        }

        return sv;
    }


    public static SymbolicVariable? IntegByParts(string parameter, SymbolicVariable u, Stack<SymbolicVariable> dv)
    {

        // ∫u dv = uv − ∫v du

        if (dv.Count == 0)
        {
            return IntegTerm(u, parameter);
        }
        else
        {

            // ∫u dv = uv − ∫v du
            var popped_dv = dv.Pop();
            var v = IntegByParts(parameter, popped_dv, dv);
            var uv = Multiply(u, v);

            var du = u.Differentiate(parameter);
            var vdu = Multiply(v, du);

            var ivdu = vdu.Integrate(parameter);


            var result = Subtract(uv, ivdu);

            return result;

        }
    }

    private static SymbolicVariable? IntegBigTerm(SymbolicVariable sv, string parameter)
    {
        if (sv.IsZero)
            return Zero;
        if (sv.IsCoeffecientOnly)
            return Multiply(sv, new SymbolicVariable(parameter));

        // now result contains only one term
        // -----------------------------------

        // we need to differentiate multiplied terms   in this  ONE TERM

        // so for integration   we will deconstruct all functions and variables  then regrouping them according to the ilate priorities
        // this is essential for using the integration by parts
        //  integration by parts is to represent the multiplied terms as u dv
        //  where u is the heighest term in the list
        // and dv is the rest of the multiplied terms together

        // the result  will be  integ u dv = uv - integ v du

        // the v du term will be enter to the integration again above
        //  and then we are acting in a recusrsive manner

        // 

        /*
            ILATE stands for:

            I: Inverse trigonometric functions : arctan x, arcsec x, arcsin x etc.

            L: Logarithmic functions : ln x, log5(x), etc.

            A: Algebraic functions.

            T: Trigonometric functions, such as sin x, cos x, tan x etc.

            E: Exponential functions.

        */

        var SvDividedTerm = sv.DividedTerm;  // here we isolate the divided term for later calculations
        sv.DividedTerm = One;

        var MultipliedTerms = DeConstruct(sv);

        //
        var InversedTrig_Term = One;
        var Logarithms_Term = One;
        var Algebra_Term = One;
        var Trig_Term = One;
        var Exponential_Term = One;



        // distribute everything
        foreach (var mt in MultipliedTerms)
        {
            if (mt.Term.IsFunction && mt.Term.InvolvedSymbols.Contains(parameter, StringComparer.OrdinalIgnoreCase ))
            {
                if (mt.Term.FunctionName.Equals(FunctionOperation.LnText, StringComparison.OrdinalIgnoreCase))
                    Logarithms_Term = Multiply(Logarithms_Term, mt.Term);

                if (mt.Term.FunctionName.Equals(FunctionOperation.ExpText, StringComparison.OrdinalIgnoreCase))
                    Exponential_Term = Multiply(Exponential_Term, mt.Term);

                else if (FunctionOperation.InversedTrigFunctions.Contains(mt.Term.FunctionName, StringComparer.OrdinalIgnoreCase))
                    InversedTrig_Term = Multiply(InversedTrig_Term, mt.Term);

                else if (FunctionOperation.TrigFunctions.Contains(mt.Term.FunctionName, StringComparer.OrdinalIgnoreCase))
                    Trig_Term = Multiply(Trig_Term, mt.Term);

            }
            else
            {
                Algebra_Term = Multiply(Algebra_Term, mt.Term);
            }
        }

        Stack<SymbolicVariable> OrderedVariables = new Stack<SymbolicVariable>();
        if (!Exponential_Term.IsOne) OrderedVariables.Push(Exponential_Term);
        if (!Trig_Term.IsOne) OrderedVariables.Push(Trig_Term);
        if (!Algebra_Term.IsOne) OrderedVariables.Push(Algebra_Term);
        if (!Logarithms_Term.IsOne) OrderedVariables.Push(Logarithms_Term);
        if (!InversedTrig_Term.IsOne) OrderedVariables.Push(InversedTrig_Term);


        var result = IntegByParts(parameter, OrderedVariables.Pop(), OrderedVariables);

        return result;

        List<SymbolicVariable> CalculatedIntegs = new List<SymbolicVariable>(1);



        //for (int ix = 0; ix < MultipliedTerms.Count; ix++)
        //{
        //CalculatedIntegs.Add(IntegTerm(MultipliedTerms[ix].Term, parameter));

        /*
        if (integrationDone == false && MultipliedTerms[ix].Term.InvolvedSymbols.Contains(parameter, StringComparer.OrdinalIgnoreCase) == true)
        {
            CalculatedIntegs.Add(IntegTerm(MultipliedTerms[ix].Term, parameter));
            integrationDone = true;
        }
        else
        {
            CalculatedIntegs.Add(MultipliedTerms[ix].Term);
        }*/
        //}



        // what about divided term ??
        if (!SvDividedTerm.IsOne)
        {
            throw new NotImplementedException();
            /*
             * diff(f(x)*g(x)/h(x),x);
             *      -(f(x)*g(x)*('diff(h(x),x,1)))/h(x)^2       <== notice the negative sign here and the squared denominator
             *      +(f(x)*('diff(g(x),x,1)))/h(x)
             *      +(g(x)*('diff(f(x),x,1)))/h(x)
             */
            var dvr = Subtract(Zero, SvDividedTerm.Integrate(parameter));  //differential of divided takes minus sign because it wil
            CalculatedIntegs.Add(dvr);

            // add the divided term but in negative  value because it is divided and in differentiation it will have -ve power
            MultipliedTerms.Add(new MultipliedTerm(SvDividedTerm, true));
        }



        //       dx*y*z     dy*x*z       dz*x*y
        var total = Zero;

        foreach (var cc in CalculatedIntegs)
        {
            if (cc.IsZero) continue;

            total = Add(total, cc);
        }

        if (!SvDividedTerm.IsOne) total.DividedTerm = Multiply(SvDividedTerm, SvDividedTerm);

        return total;
    }


    public SymbolicVariable Integrate(string parameter)
    {
        var result = Clone();

        Dictionary<string, SymbolicVariable> OtherAddedTerms = result._AddedTerms;
        result._AddedTerms = null;

        List<ExtraTerm> OtherExtraTerms = result._ExtraTerms;
        result._ExtraTerms = null;

        result = IntegBigTerm(result, parameter);


        // take the rest terms
        if (OtherAddedTerms != null)
        {
            for (var ix = 0; ix < OtherAddedTerms.Count; ix++)
            {
                var term = OtherAddedTerms.Values.ElementAt(ix);
                term = IntegBigTerm(term, parameter);

                result = Add(result, term);
            }
        }

        if (OtherExtraTerms != null)
        {
            for (var ix = 0; ix < OtherExtraTerms.Count; ix++)
            {
                var term = OtherExtraTerms[ix].Term;
                term = IntegBigTerm(term, parameter);
                result = Add(result, term);
            }
        }


        AdjustSpecialFunctions(ref result);

        AdjustZeroPowerTerms(result);

        AdjustZeroCoeffecientTerms(ref result);

        return result;
    }

}