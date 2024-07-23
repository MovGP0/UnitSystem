namespace ParticleLexer.QsTokens;

/// <summary>
/// AND operator keyword
/// </summary>
[TokenPattern(RegexPattern = "and", ExactWord = true)]
public class AndStatementToken : TokenClass;