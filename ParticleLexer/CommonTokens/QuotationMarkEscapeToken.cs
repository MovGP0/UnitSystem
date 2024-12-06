namespace ParticleLexer.CommonTokens;

/// <summary>
/// Matches <c>"\</c>
/// </summary>
[TokenPattern(RegexPattern = @"\\""", ExactWord = true)]
public class QuotationMarkEscapeToken : TokenClass;