﻿namespace SymbolicAlgebra
{
    public partial class SymbolicVariable
    {
        /// <summary>
        /// Multiply two symbolic variables
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SymbolicVariable? Multiply(SymbolicVariable a, SymbolicVariable b)
        {
            if (a == null || b == null) return null;

            if (a.IsZero) return new SymbolicVariable("0");
            if (b.IsZero) return new SymbolicVariable("0");

            if (a.IsOne) return b.Clone();
            if (b.IsOne) return a.Clone();

            SymbolicVariable TargetSubTerm = b.Clone();

            TargetSubTerm._AddedTerms = null;   // remove added variables to prevent its repeated calculations in second passes
            TargetSubTerm._ExtraTerms = null;   // remove the extra terms also
            // or to make sure nothing bad happens {my idiot design :S)

            SymbolicVariable SourceTerm = a.Clone();
            if (a.BaseEquals(TargetSubTerm))
            {
                #region Symbols are Equal (I mean 2*x^3 = 2*X^3)
                

                MultiplyCoeffecients(ref SourceTerm, TargetSubTerm);

                    
                if (a.SymbolPowerTerm != null || TargetSubTerm.SymbolPowerTerm != null)
                {
                    SourceTerm._SymbolPowerTerm = a.SymbolPowerTerm + TargetSubTerm.SymbolPowerTerm;
                }
                else
                {
                    SourceTerm.SymbolPower = SourceTerm.SymbolPower + TargetSubTerm.SymbolPower;
                }

                //fuse the fused variables in b into sv
                foreach (var bfv in TargetSubTerm.FusedSymbols)
                {
                    if (SourceTerm.FusedSymbols.ContainsKey(bfv.Key))
                        SourceTerm.FusedSymbols[bfv.Key] += bfv.Value;
                    else
                        SourceTerm.FusedSymbols.Add(bfv.Key, bfv.Value);
                }
                
                #endregion
            }
            else
            {
                #region Symbols are Different
                
                if (string.IsNullOrEmpty(SourceTerm.Symbol))
                {
                    #region First Case: Source primary symbol doesn't exist
                    
                    //  so take the primary symbol from the target into source
                    //  and copy the symbol power to it  and symbolic power
                    // 

                    // the instance have an empty primary variable so we should add it 
                    if (TargetSubTerm._BaseVariable != null) SourceTerm._BaseVariable = TargetSubTerm._BaseVariable;
                    else 
                        SourceTerm.Symbol = TargetSubTerm.Symbol;

                    SourceTerm.SymbolPower = TargetSubTerm.SymbolPower;
                    if (TargetSubTerm.SymbolPowerTerm != null) 
                        SourceTerm._SymbolPowerTerm = TargetSubTerm.SymbolPowerTerm.Clone();
                    else 
                        SourceTerm._SymbolPowerTerm = null;

                    


                    //fuse the fused variables in target into source
                    foreach (var fv in TargetSubTerm.FusedSymbols)
                    {
                        if (SourceTerm.FusedSymbols.ContainsKey(fv.Key))
                            SourceTerm.FusedSymbols[fv.Key] += fv.Value;
                        else
                            SourceTerm.FusedSymbols.Add(fv.Key, fv.Value);
                    }
                    #endregion
                }
                else
                {
                    #region Testing against symbol of targetsubterm
                    if (SourceTerm.Symbol.Equals(TargetSubTerm.Symbol, StringComparison.OrdinalIgnoreCase))
                    {
                        #region Second Case: Primary symbol in both source and target exist and equal

                        // which means the difference is only in fused variables.

                        // test for complex power (power that contains other symbolic variable) 
                        if (SourceTerm._SymbolPowerTerm != null || TargetSubTerm._SymbolPowerTerm != null)
                        {
                            // make sure the object of symbol power term have values if they don't
                            if (SourceTerm._SymbolPowerTerm == null)
                            {
                                // transfer the numerical power into symbolic variable mode
                                SourceTerm._SymbolPowerTerm = new SymbolicVariable(SourceTerm.SymbolPower.ToString(CultureInfo.InvariantCulture));
                                // also revert the original symbol power into 1  for validation after this
                                SourceTerm.SymbolPower = 1;
                            }
                            if (TargetSubTerm._SymbolPowerTerm == null)
                            {
                                TargetSubTerm._SymbolPowerTerm = new SymbolicVariable(TargetSubTerm.SymbolPower.ToString(CultureInfo.InvariantCulture));
                                TargetSubTerm.SymbolPower = 1;
                            }

                            // we used symbol power term
                            SourceTerm._SymbolPowerTerm += TargetSubTerm._SymbolPowerTerm;
                        }
                        else
                        {
                            SourceTerm.SymbolPower += TargetSubTerm.SymbolPower;
                        }

                        // then add the fused variables
                        foreach (var fv in TargetSubTerm.FusedSymbols)
                        {
                            if (SourceTerm.FusedSymbols.ContainsKey(fv.Key))
                                SourceTerm.FusedSymbols[fv.Key] += fv.Value;
                            else
                                SourceTerm.FusedSymbols.Add(fv.Key, fv.Value);
                        }
                        #endregion

                    }
                    else if (SourceTerm.FusedSymbols.ContainsKey(TargetSubTerm.Symbol))
                    {
                        #region Third Case: Target primary symbol exist in source fused variables

                        // fill the source symbol in fused variables from the primary symbol in Target term.
                        if (TargetSubTerm.SymbolPowerTerm != null)
                            SourceTerm.FusedSymbols[TargetSubTerm.Symbol] +=
                                new HybridVariable
                                {
                                    NumericalVariable = TargetSubTerm.SymbolPower,
                                    SymbolicVariable = TargetSubTerm.SymbolPowerTerm
                                };
                        else
                            SourceTerm.FusedSymbols[TargetSubTerm.Symbol] += TargetSubTerm.SymbolPower;

                        // however primary symbol in source still the same so we need to add it to the value in target
                        //    (if exist in fused variables in target)

                        // also 

                        // there are still some fused variables in source that weren't altered by the target fused symbols
                        // go through every symbol in fused symbols and add it to the source fused symbols
                        foreach (var tfs in TargetSubTerm.FusedSymbols)
                        {
                            if (SourceTerm.FusedSymbols.ContainsKey(tfs.Key))
                            {
                                // symbol exist so accumulate it
                                SourceTerm._FusedSymbols[tfs.Key] += tfs.Value;
                            }
                            else
                            {
                                // two cases here
                                // 1 the fused key equal the primary symbol in source
                                if (SourceTerm.Symbol.Equals(tfs.Key, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (tfs.Value.SymbolicVariable != null)
                                    {
                                        if (SourceTerm._SymbolPowerTerm != null)
                                            SourceTerm._SymbolPowerTerm += tfs.Value.SymbolicVariable;
                                        else
                                        {
                                            // sum the value in the numerical part to the value in symbolic part
                                            SourceTerm._SymbolPowerTerm = new SymbolicVariable(SourceTerm._SymbolPower.ToString(CultureInfo.InvariantCulture)) + tfs.Value.SymbolicVariable;
                                            // reset the value in numerical part
                                            SourceTerm._SymbolPower = 1;
                                        }
                                    }
                                    else
                                        SourceTerm._SymbolPower += tfs.Value.NumericalVariable;
                                }
                                else
                                {
                                    // 2 no matching at all which needs to add the symbol from target into the fused symbols in source.
                                    SourceTerm.FusedSymbols.Add(tfs.Key, tfs.Value);
                                }
                            }
                        }
                        #endregion
                    
                    }
                    else
                    {
                        #region Fourth Case: Target primary symbol doesn't exist in Source Primary Symbol nor Source Fused symbols
                        // Add Target primary symbol to the fused symbols in source
                        SourceTerm.FusedSymbols.Add(
                            TargetSubTerm.Symbol,
                            new HybridVariable
                            {
                                NumericalVariable = TargetSubTerm.SymbolPower,
                                SymbolicVariable = TargetSubTerm.SymbolPowerTerm == null ? null : TargetSubTerm.SymbolPowerTerm.Clone()
                            });                            
                        

                        // But the primary symbol of source may exist in the target fused variables.

                        foreach (var fsv in TargetSubTerm.FusedSymbols)
                        {
                            if (SourceTerm.FusedSymbols.ContainsKey(fsv.Key))
                                SourceTerm.FusedSymbols[fsv.Key] += fsv.Value;
                            else
                            {
                                // 1- if symbol is the same as priamry source 
                                if (SourceTerm.Symbol.Equals(fsv.Key, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (fsv.Value.SymbolicVariable != null)
                                    {
                                        if (SourceTerm._SymbolPowerTerm != null)
                                            SourceTerm._SymbolPowerTerm += fsv.Value.SymbolicVariable;
                                        else
                                        {
                                            // sum the value in the numerical part to the value in symbolic part
                                            SourceTerm._SymbolPowerTerm = new SymbolicVariable(SourceTerm._SymbolPower.ToString(CultureInfo.InvariantCulture)) + fsv.Value.SymbolicVariable;
                                            // reset the value in numerical part
                                            SourceTerm._SymbolPower = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (SourceTerm.SymbolPowerTerm != null)
                                        {
                                            SourceTerm._SymbolPowerTerm += new SymbolicVariable(fsv.Value.ToString());
                                        }
                                        else
                                        {
                                            SourceTerm._SymbolPower += fsv.Value.NumericalVariable;
                                        }
                                    }

                                }
                                else
                                {
                                    SourceTerm.FusedSymbols.Add(fsv.Key, fsv.Value);
                                }
                            }
                        }
                        #endregion

                    
                    }

                    #endregion
                }

                MultiplyCoeffecients(ref SourceTerm, TargetSubTerm);

                #endregion
            }

            if (SourceTerm.DividedTerm.IsOne) SourceTerm.DividedTerm = TargetSubTerm.DividedTerm;
            else
                SourceTerm.DividedTerm = Multiply(SourceTerm.DividedTerm, TargetSubTerm.DividedTerm);

            //here is a code to continue with other parts of a when multiplying them
            if (SourceTerm.AddedTerms.Count > 0)
            {
                Dictionary<string, SymbolicVariable> newAddedVariables = new Dictionary<string, SymbolicVariable>(StringComparer.OrdinalIgnoreCase);
                foreach (var vv in SourceTerm.AddedTerms)
                {
                    // get rid of divided term here because I already include it for the source term above
                    var TSubTerm = TargetSubTerm.Numerator;
                    
                    var newv = Multiply(vv.Value, TSubTerm);

                    newAddedVariables.Add(newv.WholeValueBaseKey, newv);
                }
                SourceTerm._AddedTerms = newAddedVariables;
            }

            // Extra Terms of a
            if (SourceTerm.ExtraTerms.Count > 0)
            {
                List<ExtraTerm> newExtraTerms = new List<ExtraTerm>();
                foreach (var et in SourceTerm.ExtraTerms)
                {
                    var eterm = et.Term;
                    if (et.Negative) eterm = Multiply(NegativeOne, eterm);

                    var newe = Multiply(eterm, TargetSubTerm);
                    newExtraTerms.Add(new ExtraTerm { Term = newe });
                }
                SourceTerm._ExtraTerms = newExtraTerms;
            }

            // now source term which is the first parameter cloned, have the new calculated value.
            int subIndex = 0;
            SymbolicVariable total = SourceTerm;

            while (subIndex < b.AddedTerms.Count)
            {
                // we should multiply other parts also 
                // then add it to the current instance

                // there are still terms to be consumed 
                //   this new term is a sub term in b and will be added to all terms of a.
                TargetSubTerm =  b.AddedTerms.ElementAt(subIndex).Value.Clone();

                
                TargetSubTerm.DividedTerm = b.DividedTerm;   // this line is important because I extracted this added term from a bigger term with the same divided term
                                                             // and when I did this the extracted term lost its divided term 


                var TargetTermSubTotal = Multiply(a, TargetSubTerm);
                total = Add(total, TargetTermSubTotal);

                subIndex = subIndex + 1;  //increase 
            }

            // for extra terms  {terms that has different divided term}
            int extraIndex = 0;
            while (extraIndex < b.ExtraTerms.Count)
            {
                var eTerm = b.ExtraTerms[extraIndex];
                TargetSubTerm = eTerm.Term;
                if (eTerm.Negative) TargetSubTerm = Multiply(NegativeOne, TargetSubTerm);
                var TargetTermSubTotal = Multiply(a, TargetSubTerm);
                total = Add(total, TargetTermSubTotal);
                extraIndex++;
            }

            
            AdjustSpecialFunctions(ref total);
            AdjustZeroPowerTerms(total);

            AdjustZeroCoeffecientTerms(ref total);


            return total;
        }

        public struct CoeffecienttValue
        {
            public double ConstantValue;
            public SymbolicVariable ConstantPower;
        }

        public CoeffecienttValue[] Constants
        {
            get
            {
                var primary = new CoeffecienttValue
                {
                    ConstantValue = Coeffecient,
                    ConstantPower = CoeffecientPowerTerm
                };

                CoeffecienttValue[] cvs = new CoeffecienttValue[FusedConstants.Count + 1];
                cvs[0] = primary;

                for (int i = 0; i < FusedConstants.Count; i++)
                {
                    var hb = FusedConstants.ElementAt(i);
                    cvs[i + 1] = new CoeffecienttValue
                    {
                        
                        ConstantValue = hb.Key,
                        ConstantPower = hb.Value.SymbolicVariable
                    };
                }

                return cvs;
            }
        }

        /// <summary>
        /// Multiply Coeffecients of second argument into first argument.
        /// </summary>
        /// <param name="SourceTerm"></param>
        /// <param name="TargetSubTerm"></param>
        private static void MultiplyCoeffecients(ref SymbolicVariable SourceTerm, SymbolicVariable TargetSubTerm)
        {
            // Note: I will try to avoid the primary coeffecient so it doesn't hold power
            //      and only hold coeffecient itself.
            foreach (var cst in TargetSubTerm.Constants)
            {
                
                if (SourceTerm._CoeffecientPowerTerm == null && cst.ConstantPower == null)
                {
                    SourceTerm.Coeffecient *= cst.ConstantValue;
                }
                // there is a coeffecient power term needs to be injected into the source
                else
                {
                    // no the coeffecient part is not empty so we will test if bases are equal
                    // then make use of the fusedsymbols to add our constant


                    double sbase = Math.Log(SourceTerm.Coeffecient, cst.ConstantValue);

                    if (SourceTerm.Coeffecient == cst.ConstantValue)
                    {
                        // sample: 2^x*2^y = 2^(x+y)
                        var cpower = cst.ConstantPower;
                        if (cpower == null) cpower = One;

                        if (SourceTerm._CoeffecientPowerTerm == null) SourceTerm._CoeffecientPowerTerm = One;
                        SourceTerm._CoeffecientPowerTerm += cpower;
                    }
                    else if ((sbase - Math.Floor(sbase)) == 0.0 && sbase > 0)   // make sure there are no
                    {
                        // then we can use the target base
                        // 27*3^x = 3^3*3^x = 3^(3+x)
                        var cpower = cst.ConstantPower;
                        if (cpower == null) cpower = new SymbolicVariable("1");
                        SourceTerm._CoeffecientPowerTerm = Add(new SymbolicVariable(sbase.ToString(CultureInfo.InvariantCulture)), cpower);
                        SourceTerm.Coeffecient = cst.ConstantValue;

                    }
                    else
                    {
                        // sample: 2^(x+y)*log(2)*3^y * 3^z   can't be summed.
                        HybridVariable SameCoeffecient;
                        if (SourceTerm.FusedConstants.TryGetValue(cst.ConstantValue, out SameCoeffecient))
                        {
                            SameCoeffecient.SymbolicVariable += cst.ConstantPower;
                            SourceTerm.FusedConstants[cst.ConstantValue] = SameCoeffecient;
                        }
                        else
                        {
                            // Add Target coefficient symbol to the fused symbols in source, with key as the coefficient number itself.
                            if (cst.ConstantPower != null)
                            {
                                if (SourceTerm.IsNegativeOne)
                                {
                                    SourceTerm.Coeffecient = -1 * cst.ConstantValue;
                                    SourceTerm._CoeffecientPowerTerm = cst.ConstantPower;
                                }
                                else
                                {
                                    SourceTerm.FusedConstants.Add(
                                        cst.ConstantValue,
                                        new HybridVariable
                                        {
                                            NumericalVariable = 1, // power
                                            SymbolicVariable = cst.ConstantPower.Clone()
                                        });
                                }
                            }
                            else
                            {
                                // coeffecient we working with is number only
                                // First Attempt: to try to get the power of this number with base of available coeffecients
                                // if no base produced an integer power value then we add it into fused constants as it is.

                                if (cst.ConstantValue == 1.0) continue;  // ONE doesn't change anything so we bypass it

                                double? SucceededConstant = null;
                                double ower = 0;
                                foreach (var p in SourceTerm.Constants)
                                {
                                    ower = Math.Log(cst.ConstantValue, p.ConstantValue);
                                    if (ower == Math.Floor(ower))
                                    {
                                        SucceededConstant = p.ConstantValue;
                                        break;
                                    }
                                }

                                if (SucceededConstant.HasValue)
                                {
                                    if (SourceTerm.Coeffecient == SucceededConstant.Value)
                                    {
                                        SourceTerm._CoeffecientPowerTerm += new SymbolicVariable(ower.ToString(CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        var rr = SourceTerm.FusedConstants[SucceededConstant.Value];

                                        rr.SymbolicVariable += new SymbolicVariable(ower.ToString(CultureInfo.InvariantCulture));
                                        SourceTerm.FusedConstants[SucceededConstant.Value] = rr;
                                    }

                                }
                                else
                                {
                                    SourceTerm.FusedConstants.Add(
                                        cst.ConstantValue,
                                        new HybridVariable
                                        {
                                            NumericalVariable = 1, // power
                                        });
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Multiply hybrid variable by symbolic variable, and returns the value into symbolic variable.
        /// because hybrid variable is either number of symbolic.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns><see cref="SymbolicVariable"/></returns>
        internal static SymbolicVariable? Multiply(HybridVariable a, SymbolicVariable b)
        {
            if (a.SymbolicVariable != null) return Multiply(a.SymbolicVariable, b);
            else return Multiply(new SymbolicVariable(a.NumericalVariable.ToString(CultureInfo.InvariantCulture)), b);
        }
    }
}
