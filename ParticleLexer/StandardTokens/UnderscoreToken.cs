namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"_", ExactWord = true)]
public class UnderscoreToken : TokenClass;