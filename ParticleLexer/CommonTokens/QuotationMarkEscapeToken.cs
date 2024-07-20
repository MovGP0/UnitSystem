namespace ParticleLexer.CommonTokens
{

    /// <summary>
    /// Mathches \"    
    /// </summary>
    [TokenPattern(RegexPattern = @"\\""", ExactWord = true)]
    public class QuotationMarkEscapeToken : TokenClass
    {

    }


}
