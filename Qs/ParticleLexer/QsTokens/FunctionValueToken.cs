namespace ParticleLexer.QsTokens;

/// <summary>
/// Adding '@' before function name like @f(x)  return the function body
/// </summary>
[TokenPattern(RegexPattern = @"(@\w+|@\w+\([\sa-zA-Z0-9,]*\)|@\w+:\w+|@\w+:\w+\([\sa-zA-Z0-9,]*\))")]
public class FunctionValueToken : TokenClass;