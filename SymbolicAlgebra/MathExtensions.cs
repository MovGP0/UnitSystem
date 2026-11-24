namespace SymbolicAlgebra;

public static class MathExtensions
{
    internal static SymbolicVariable RequireExpression(this SymbolicVariable? value)
        => value ?? throw new SymbolicException("A symbolic operation did not produce an expression.");

    public static SymbolicVariable? Abs(this SymbolicVariable sv)
    {
        if (sv.IsNegative)
        {
            return sv * SymbolicVariable.NegativeOne;
        }
        else
        {
            return sv.Clone();
        }
    }
}
