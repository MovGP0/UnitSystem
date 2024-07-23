namespace ParticleLexer.QsTokens;

/// <summary>
/// C{3 4}
/// </summary>
[TokenPattern(RegexPattern = @"C\{.+\}", ShouldBeginWith = "C", ShouldEndWith = "}")]
public class ComplexNumberToken : TokenClass;