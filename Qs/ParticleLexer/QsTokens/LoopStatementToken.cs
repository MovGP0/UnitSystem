namespace ParticleLexer.QsTokens;

/// <summary>
/// Loop
/// </summary>
[TokenPattern(RegexPattern="Loop", ExactWord = true)]
public class LoopStatementToken : TokenClass;