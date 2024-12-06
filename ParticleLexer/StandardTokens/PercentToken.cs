namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = "%", ExactWord = true)]
public class PercentToken : TokenClass;