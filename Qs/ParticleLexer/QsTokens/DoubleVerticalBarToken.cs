namespace ParticleLexer.QsTokens;

/// <summary>
/// Double Vertical Bar
/// </summary>
[TokenPattern(RegexPattern = @"\|\|", ExactWord = true)]
public class DoubleVerticalBarToken : TokenClass;