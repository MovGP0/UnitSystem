
namespace ParticleLexer.StandardTokens
{
    [TokenPattern(RegexPattern = "\\w+", ContinousToken = true)]
    public class WordToken : TokenClass
    {

    }

    [TokenPattern(RegexPattern = @"\s+", ShouldBeginWith = " ", ContinousToken = true)]
    public class MultipleSpaceToken : TokenClass
    {
    }

}
