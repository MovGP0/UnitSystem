namespace SymbolicAlgebra;

public class ExtraTerm
{
    public required SymbolicVariable Term;

    public bool Negative;

    public ExtraTerm Clone()
    {
        return new ExtraTerm { Term = Term.Clone(), Negative = Negative };
    }
}
