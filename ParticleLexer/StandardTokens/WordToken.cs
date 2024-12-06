
namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = "\\w+", ContinousToken = true)]
public class WordToken : TokenClass;