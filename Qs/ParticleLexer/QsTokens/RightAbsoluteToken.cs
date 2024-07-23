namespace ParticleLexer.QsTokens;

/// <summary>
/// right token of absolute group
/// </summary>
[TokenPattern(RegexPattern = @"\|_", ExactWord = true)]
public class RightAbsoluteToken : TokenClass;