namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = "==", ExactWord = true)]
public class C_EqualityToken : TokenClass;