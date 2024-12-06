namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"\s", ExactWord = true)]
public class SingleSpaceToken : TokenClass;