namespace ParticleLexer.QsTokens;

/// <summary>
/// a=S[u SequenceRangeToken ]   like  a=S[2++5]
/// The operator range that appears in the sequence calling square brackets n..m
/// where .. is
///         ++ Series:                  Sum elements                    returns Scalar
///         ** Product:                 Multiply elements               returns Scalar
///         !! Average:                 Get the Mean of elements.       returns Scalar.
///         !% Standard Deviation
///         .. Range                                                    returns Vector if components are scalars, Matrix if components are vectors
/// </summary>
[TokenPattern(RegexPattern = "((\\+\\+)?|(!!)?|(\\*\\*)?|(!%)?|(\\.\\.)?)")]
public class SequenceRangeToken : TokenClass;