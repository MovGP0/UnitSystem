namespace ParticleLexer.QsTokens;

[TokenPattern(RegexPattern = @"True", ExactWord=true)]
public class TrueToken : TokenClass;