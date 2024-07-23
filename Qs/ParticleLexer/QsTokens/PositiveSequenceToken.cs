namespace ParticleLexer.QsTokens;

/// <summary>
/// ..>  token
/// </summary>
[TokenPattern(RegexPattern = "\\.\\.>", ExactWord = true, ShouldBeginWith = ".")]
public class PositiveSequenceToken : TokenClass;