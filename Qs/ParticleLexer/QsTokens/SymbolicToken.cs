namespace ParticleLexer.QsTokens;

/// <summary>
/// Dollar Sign followed by word token. $x or $y  $ROI
/// also can be used for ${x*x*y}   any text between brackets will be parsed by the symbolicvariable praser
/// </summary>
[TokenPattern(RegexPattern = @"\$\{.+\}", ShouldBeginWith = "$", ShouldEndWith = "}")]
public class SymbolicToken : TokenClass;