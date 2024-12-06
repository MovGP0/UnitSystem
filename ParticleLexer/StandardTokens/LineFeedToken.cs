namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"\n", ExactWord = true)]
public class LineFeedToken : TokenClass;