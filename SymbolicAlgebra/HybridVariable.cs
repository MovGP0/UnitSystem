namespace SymbolicAlgebra;

/// <summary>
/// Symbolic + Numerical  value
/// currently used in Fused Variables as the power term for the symbol in dictionary.
/// </summary>
internal struct HybridVariable
{

    public SymbolicVariable? SymbolicVariable;
    public double NumericalVariable;


    public static HybridVariable operator -(HybridVariable hv, HybridVariable num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable - num.NumericalVariable;
        nhv.SymbolicVariable = hv.SymbolicVariable - num.SymbolicVariable;

        if (nhv.SymbolicVariable != null)
        {
            if (nhv.SymbolicVariable.IsCoeffecientOnly)
            {
                nhv.NumericalVariable = nhv.SymbolicVariable.Coeffecient;
                nhv.SymbolicVariable = null;
            }
        }
        return nhv;
    }

    public static HybridVariable operator -(HybridVariable hv, double num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable - num;
        nhv.SymbolicVariable = hv.SymbolicVariable;
        return nhv;
    }

    public static HybridVariable operator -(HybridVariable hv, SymbolicVariable num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable;
        nhv.SymbolicVariable = hv.SymbolicVariable - num;
        if (nhv.SymbolicVariable.IsCoeffecientOnly)
        {
            nhv.NumericalVariable = nhv.SymbolicVariable.Coeffecient;
            nhv.SymbolicVariable = null;
        }
        return nhv;
    }

    public static HybridVariable operator +(HybridVariable hv, HybridVariable num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable + num.NumericalVariable;
        nhv.SymbolicVariable = hv.SymbolicVariable + num.SymbolicVariable;
        return nhv;
    }


    public static HybridVariable operator +(HybridVariable hv, double num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable + num;
        nhv.SymbolicVariable = hv.SymbolicVariable;
        return nhv;
    }

    public static HybridVariable operator *(HybridVariable hv, HybridVariable num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable * num.NumericalVariable;
        nhv.SymbolicVariable = hv.SymbolicVariable * num.SymbolicVariable;
        return nhv;
    }

    public static HybridVariable operator *(HybridVariable hv, double num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable * num;
        nhv.SymbolicVariable = hv.SymbolicVariable * num;
        return nhv;
    }



    public static HybridVariable operator /(HybridVariable hv, HybridVariable num)
    {
        HybridVariable nhv;
        nhv.NumericalVariable = hv.NumericalVariable - num.NumericalVariable;
        nhv.SymbolicVariable = hv.SymbolicVariable - num.SymbolicVariable;
        return nhv;
    }

    public override string ToString()
    {
        if (SymbolicVariable != null)
        {
            return SymbolicVariable.ToString();
        }
        else
        {
            return NumericalVariable.ToString(CultureInfo.InvariantCulture);
        }
    }


    public bool IsZero
    {
        get
        {
            if (SymbolicVariable != null)
            {
                return SymbolicVariable.IsZero;
            }
            else if (NumericalVariable == 0)
            {
                return true;
            }
            else
                return false;
        }

    }

    public bool IsNegative
    {
        get
        {
            if (SymbolicVariable != null)
            {
                return SymbolicVariable.IsNegative;
            }
            else if (NumericalVariable < 0)
            {
                return true;
            }
            else
                return false;

        }
    }

    #region ICloneable Members

    public object Clone()
    {
        var hv = new HybridVariable();
        hv.NumericalVariable = NumericalVariable;
        if (SymbolicVariable != null)
            hv.SymbolicVariable = SymbolicVariable.Clone();
        return hv;
    }

    #endregion
}