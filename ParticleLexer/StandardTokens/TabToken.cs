namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"\t", ExactWord = true)]
public class TabToken : TokenClass;