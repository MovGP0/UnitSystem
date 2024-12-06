
namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?")]
public class NumberToken : TokenClass;