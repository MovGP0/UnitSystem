﻿namespace SymbolicAlgebra;

public partial class SymbolicVariable
{
    /// <summary>
    /// Raise to symbolic variable.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public SymbolicVariable? RaiseToSymbolicPower(SymbolicVariable b)
    {
        var an = Clone();

        //if the value is only having coeffecient then there is no need to instantiate the powerterm

        if (b.IsOneTerm & b.IsCoeffecientOnly & (Math.Floor(b.Coeffecient) == b.Coeffecient) /*& b.Coeffecient >= 0*/)
        {
            an = an.Power(b.Coeffecient);
        }
        else
        {
            #region full symbolic variable
            // the power is symbolic variable

            // check if the base is one term
            if (IsOneTerm)
            {
                if (an._SymbolPowerTerm == null)
                {
                    if (an.SymbolPower != 0)
                    {
                        if (b.IsOneTerm & b.IsCoeffecientOnly)
                        {
                            an.SymbolPower = an.SymbolPower * b.Coeffecient;
                        }
                        else
                        {
                            an._SymbolPowerTerm = an.SymbolPower * b;
                        }
                    }
                }
                else
                {
                    an._SymbolPowerTerm = an._SymbolPowerTerm * b;
                }

                if (an._CoeffecientPowerTerm == null)
                {
                    if (Math.Abs(an.Coeffecient) != 1) an._CoeffecientPowerTerm = b;
                    // because 1^(Any Thing) equals == 1 :)
                }
                else
                {
                    an._CoeffecientPowerTerm = an._CoeffecientPowerTerm * b;
                }

                // raised the fused symbols

                for (var i = 0; i < FusedSymbols.Count; i++)
                {
                    var fusedItem = FusedSymbols[FusedSymbols.ElementAt(i).Key];
                    if (fusedItem.SymbolicVariable == null)
                    {
                        fusedItem.SymbolicVariable = b * fusedItem.NumericalVariable;
                        fusedItem.NumericalVariable = 1;  // set it to one because it has gone to the symbolic part
                    }
                    else
                        fusedItem.SymbolicVariable = fusedItem.SymbolicVariable * b;

                    an.FusedSymbols[FusedSymbols.ElementAt(i).Key] = fusedItem;
                }
            }
            else
            {
                // (x+y)^(3*x)
                // to implement this
                // the whole symbolic variable should have Symbolic Term.
                var thisBase = new SymbolicVariable(this);
                if (_SymbolPowerTerm != null)
                    thisBase._SymbolPowerTerm = _SymbolPowerTerm * b;
                else
                    thisBase._SymbolPowerTerm = b;

                return thisBase;
            }

            #endregion
        }

        return an;
    }

    /// <summary>
    /// Power of symbolic variable.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static SymbolicVariable? SymbolicPower(SymbolicVariable a, SymbolicVariable b) => a.RaiseToSymbolicPower(b);

    public static SymbolicVariable? Pow(SymbolicVariable a, SymbolicVariable b) => SymbolicPower(a, b);
}