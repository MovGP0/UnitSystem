namespace SymbolicAlgebra
{
    public static class CoLogic
    {

        public static double IIF(bool condition, double truePart, double falsePart)
        {
            if (condition) return truePart;
            else return falsePart;
        }
    }
}
