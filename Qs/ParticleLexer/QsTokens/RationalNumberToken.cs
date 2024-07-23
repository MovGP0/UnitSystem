namespace ParticleLexer.QsTokens;

/// <summary>
/// Q{1 2}
/// </summary>
[TokenPattern(RegexPattern = @"Q\{.+\}", ShouldBeginWith = "Q", ShouldEndWith = "}")]
public class RationalNumberToken : TokenClass;