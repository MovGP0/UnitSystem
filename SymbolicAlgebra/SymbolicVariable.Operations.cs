﻿namespace SymbolicAlgebra;

public partial class SymbolicVariable
{

    internal struct MultipliedTerm(SymbolicVariable term, bool divided)
    {

        public SymbolicVariable Term = term;
        public bool Divided = divided;
        public MultipliedTerm(SymbolicVariable term) : this(term, false)
        {
        }
    }

    /// <summary>
    /// This deconstruct the fused variables inside the symbolic variable into independent symbolic variables objects
    /// </summary>
    /// <param name="sv"></param>
    /// <returns></returns>
    internal static List<MultipliedTerm> DeConstruct(SymbolicVariable sv)
    {
        var MultipliedTermsCount = sv.FusedSymbols.Count + sv.FusedConstants.Count + 1; // last one is the basic symbol and coeffecient in the instant

        // separate all terms into array by flatting them
        var MultipliedTerms = new List<MultipliedTerm>(MultipliedTermsCount);

        Action<SymbolicVariable> SpliBaseTerm = (rr) =>
        {
            var basicterm = rr.Clone();
            basicterm._FusedConstants = null;
            basicterm._FusedSymbols = null;

            // split coeffecient and its associated symbol

            if (basicterm.CoeffecientPowerTerm != null)
            {
                // coefficient
                var CoeffecientOnly = new SymbolicVariable("");
                CoeffecientOnly._CoeffecientPowerTerm = basicterm.CoeffecientPowerTerm;
                CoeffecientOnly.Coeffecient = basicterm.Coeffecient;
                MultipliedTerms.Add(new MultipliedTerm(CoeffecientOnly));

                // multiplied symbol
                if (!string.IsNullOrEmpty(basicterm.SymbolBaseKey))
                    MultipliedTerms.Add(new MultipliedTerm(Parse(basicterm.WholeValueBaseKey)));
            }
            else
            {
                MultipliedTerms.Add(new MultipliedTerm(basicterm));
            }

        };

        Action<SymbolicVariable> SpliFusedConstants = (rr) =>
        {
            var basicterm = rr.Clone();

            var FCConstants = basicterm._FusedConstants;

            // Key  is the coefficient
            //  value contains the power  which always will be symbolic power or null
            foreach (var FC in FCConstants)
            {
                var CoeffecientOnly = new SymbolicVariable("");
                CoeffecientOnly._CoeffecientPowerTerm = FC.Value.SymbolicVariable.Clone();
                CoeffecientOnly.Coeffecient = FC.Key;

                MultipliedTerms.Add(new MultipliedTerm(CoeffecientOnly));
            }
        };

        Action<SymbolicVariable> SplitFusedSymbols = (rr) =>
        {
            var basicterm = rr.Clone();

            var FSymbols = basicterm._FusedSymbols;

            // Key  is the coefficient
            //  value contains the power  which always will be symbolic power or null
            foreach (var FS in FSymbols)
            {
                var ss = new SymbolicVariable(FS.Key);
                ss.SymbolPower = FS.Value.NumericalVariable;
                if (FS.Value.SymbolicVariable != null)
                    ss._SymbolPowerTerm = FS.Value.SymbolicVariable.Clone();

                MultipliedTerms.Add(new MultipliedTerm(ss));
            }
        };

        SpliBaseTerm(sv);
        if (sv.FusedConstants.Count > 0) SpliFusedConstants(sv);
        if (sv.FusedSymbols.Count > 0) SplitFusedSymbols(sv);

        return MultipliedTerms;
    }
}