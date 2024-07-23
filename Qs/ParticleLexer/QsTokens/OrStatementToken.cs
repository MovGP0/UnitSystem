namespace ParticleLexer.QsTokens;

/// <summary>
/// OR operator keyword
/// </summary>
[TokenPattern(RegexPattern = "or", ExactWord = true)]
public class OrStatementToken : TokenClass;