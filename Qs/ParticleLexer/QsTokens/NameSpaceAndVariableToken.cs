namespace ParticleLexer.QsTokens;

/// <summary>
/// Reference Namespace with its value  x:r   x:u  xd:Abs  x:r:t y:t:@e etc...
/// NamespaceToken
/// </summary>
[TokenPattern(RegexPattern = @"(:*[a-zA-Z]\w*:)+@?[a-zA-Z]\w*", ContinousToken = true)]
public class NameSpaceAndVariableToken : TokenClass;