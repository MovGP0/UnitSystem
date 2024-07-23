namespace ParticleLexer.QsTokens;

/// <summary>
/// -> Token
/// </summary>
[TokenPattern(RegexPattern = "->", ExactWord = true)]
public class PointerOperatorToken : TokenClass;