namespace ParticleLexer.QsTokens;

/// <summary>
/// Reference token on the form WordToken, ColonToken  x:  oh:  D80: may contain in its end letters
/// 
/// May begin with ':'
/// Must Begin with alphabet [a-zA-Z]
/// Follwed by any number of letter  \w*
/// may contain ':' in the first and must contain ':' at the end
/// </summary>
[TokenPattern(RegexPattern = @"(:*[a-zA-Z]\w*:)+", ContinueTestAfterSuccess = true)]   // when merging tokens if a success happen then continue merge until a failure happen or consume success as much as you can
public class NamespaceToken : TokenClass;