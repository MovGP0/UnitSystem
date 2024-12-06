namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = "<>", ExactWord = true)]
public class Basic_InequalityToken : TokenClass;