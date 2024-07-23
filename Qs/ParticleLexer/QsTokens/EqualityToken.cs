namespace ParticleLexer.QsTokens;

/// <summary>
/// Equality == operator
/// </summary>
[TokenPattern(RegexPattern = "==", ExactWord = true)]
public class EqualityToken : TokenClass;