namespace ParticleLexer.StandardTokens
{

    [TokenPattern(RegexPattern = "and", ExactWord = true)]
    public class AndWordToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "or", ExactWord = true)]
    public class OrWordToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "==", ExactWord = true)]
    public class C_EqualityToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "!=", ExactWord = true)]
    public class C_InequalityToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "<>", ExactWord = true)]
    public class Basic_InequalityToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "<=", ExactWord = true)]
    public class LessThanOrEqualToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = ">=", ExactWord = true)]
    public class GreaterThanOrEqualToken : TokenClass
    {
    }

}