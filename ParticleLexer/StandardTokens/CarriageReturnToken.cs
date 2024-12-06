namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"\r", ExactWord = true)]
public class CarriageReturnToken : TokenClass;