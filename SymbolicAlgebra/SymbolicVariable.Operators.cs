namespace SymbolicAlgebra;

public partial class SymbolicVariable
{


    /// <summary>
    /// Raise to specified power.
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public SymbolicVariable? Power(int power)
    {
        if (power == 0) return _One;

        var total = Clone();
        var pw = Math.Abs(power);
        while (pw > 1)
        {
            if (IsFunction && FunctionName.Equals("Sqrt", StringComparison.OrdinalIgnoreCase))
            {
                //
                var parameterpower = power * 0.5;

                total = _FunctionParameters[0].Power(parameterpower);

                pw = 0; // to end the loop
            }
            else
            {
                total = Multiply(total, this);
                pw--;
            }
        }

        if (power < 0)
        {
            total = Divide(_One, total);
        }

        return total;
    }

    public SymbolicVariable? Power(double power)
    {
        if (Math.Floor(power) == power) return Power((int)power);

        var p = Clone();
        if (p.IsOneTerm)
        {
            // raise the coeffecient and symbol
            if (!string.IsNullOrEmpty(p.Symbol)) p.SymbolPower = power;
            p.Coeffecient = Math.Pow(p.Coeffecient, power);
        }
        else
        {
            if (power == 0.5)
            {
                // return sqrt function of the multi term

                return new SymbolicVariable("Sqrt(" + p.ToString() + ")");
            }
            else if (power > 0 && power < 1)
            {
                // I don't have solution for this now
                throw new SymbolicException("I don't have solution for this type of power " + p.ToString() + "^ (" + power.ToString() + ")");
            }
            else
            {
                // multi term that we can't raise it to the double
                return p.RaiseToSymbolicPower(new SymbolicVariable(power.ToString()));
            }
        }

        return p;
    }


    public static SymbolicVariable? Pow(SymbolicVariable a, int power)
    {
        return a.RaiseToSymbolicPower(new SymbolicVariable(power.ToString()));
    }

    public static SymbolicVariable? Pow(SymbolicVariable a, double power)
    {

        return a.RaiseToSymbolicPower(new SymbolicVariable(power.ToString()));
    }


    public static SymbolicVariable? operator +(SymbolicVariable a, SymbolicVariable b)
    {
        return Add(a, b);
    }

    public static SymbolicVariable? operator -(SymbolicVariable a, SymbolicVariable b)
    {
        return Subtract(a, b);
    }

    public static SymbolicVariable? operator *(SymbolicVariable a, SymbolicVariable b)
    {
        return Multiply(a, b);
    }

    public static SymbolicVariable? operator /(SymbolicVariable a, SymbolicVariable b)
    {
        return Divide(a, b);
    }
}