namespace ParticleLexer.QsTokens;

[TokenPattern(RegexPattern = "\\.\\.", ExactWord = true, ShouldBeginWith = ".")]
public class VectorRangeToken : TokenClass;