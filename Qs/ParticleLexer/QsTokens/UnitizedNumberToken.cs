namespace ParticleLexer.QsTokens;

/// <summary>
/// Unitized number is
/// 90, 90.9
/// 90.9e2, 90.9e+2, 90.9e-2
/// 90.09&lt;m&gt;, 90.2e+2&lt;m&gt;, etc.
/// </summary>
[TokenPattern(
        RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?" //floating number
                       + "(\\s*<(°?[\\w\\$]+!?(\\^\\d+)?\\.?)+(/(°?[\\w\\$]+!?(\\^\\d+)?\\.?)+)?>)?" // unit itself.
    )
]
public class UnitizedNumberToken : TokenClass;