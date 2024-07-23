namespace ParticleLexer.QsTokens;

/// <summary>
/// Left token of absolute group
/// </summary>
[TokenPattern(RegexPattern = @"_\|", ExactWord = true)]
public class LeftAbsoluteToken : TokenClass;