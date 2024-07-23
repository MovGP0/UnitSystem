namespace ParticleLexer.QsTokens;

/// <summary>
/// unit token form &lt;unit&gt;
/// </summary>
[TokenPattern(
    RegexPattern = "<(°?[\\w\\$]+!?(\\^\\d+)?\\.?)+(/(°?[\\w\\$]+!?(\\^\\d+)?\\.?)+)?>",
    ShouldBeginWith = "<",
    ShouldEndWith = ">")]
public class UnitToken : TokenClass;
