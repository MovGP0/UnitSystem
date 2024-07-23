namespace ParticleLexer.QsTokens;

/// <summary>
/// @{(t) = t^2}
/// </summary>
[TokenPattern(RegexPattern = @"@\{.+\}", ShouldBeginWith = "@", ShouldEndWith = "}")]
public class FunctionLambdaToken : TokenClass;