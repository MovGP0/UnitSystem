namespace ParticleLexer.QsTokens;

/// <summary>
/// right token of norm group
/// </summary>
[TokenPattern(RegexPattern = @"\|\|_", ExactWord = true)]
public class RightNormToken : TokenClass;