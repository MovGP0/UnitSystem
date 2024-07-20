﻿namespace SymbolicAlgebra
{
    public static class MathExtensions
    {
        public static SymbolicVariable? Abs(this SymbolicVariable sv)
        {
            if (sv.IsNegative) return sv * SymbolicVariable.NegativeOne;
            else return sv.Clone();
        }
    }
}
