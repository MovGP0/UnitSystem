namespace ParticleLexer.QsTokens;

[TokenPattern(RegexPattern = "<<", ExactWord = true)]
public class LeftShiftToken : TokenClass;