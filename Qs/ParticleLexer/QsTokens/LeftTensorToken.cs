namespace ParticleLexer.QsTokens;

[TokenPattern(RegexPattern = @"<\|", ExactWord = true)]
public class LeftTensorToken : TokenClass;