namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = "}", ExactWord = true)]
public class RightCurlyBracketToken : TokenClass;