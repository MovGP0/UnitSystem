namespace ParticleLexer.QsTokens;

/// <summary>
/// \/ nabla operator :)
/// </summary>
[TokenPattern(RegexPattern = @"\\.*\/")]
public class Nabla : TokenClass;