namespace ParticleLexer.QsTokens;

/// <summary>
/// H{3 4 2 1}
/// </summary>
[TokenPattern(RegexPattern = @"H\{.+\}", ShouldBeginWith = "H", ShouldEndWith = "}")]
public class QuaternionNumberToken : TokenClass;