namespace ParticleLexer.QsTokens;

[TokenPattern(RegexPattern = @"False", ExactWord = true)]
public class FalseToken : TokenClass;