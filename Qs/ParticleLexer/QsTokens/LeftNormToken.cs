namespace ParticleLexer.QsTokens;

/// <summary>
/// left token of norm group
/// </summary>
[TokenPattern(RegexPattern = @"_\|\|", ExactWord = true)]
public class LeftNormToken : TokenClass;