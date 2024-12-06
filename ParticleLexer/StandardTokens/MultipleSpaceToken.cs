namespace ParticleLexer.StandardTokens;

[TokenPattern(RegexPattern = @"\s+", ShouldBeginWith = " ", ContinousToken = true)]
public class MultipleSpaceToken : TokenClass;