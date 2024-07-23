using ParticleLexer.StandardTokens;

namespace ParticleLexer.QsTokens;

/// <summary>
/// Express groups of '&lt;| a b c |>'
/// </summary>
public class TensorGroupToken : GroupTokenClass
{
    public TensorGroupToken()
        : base(new LeftTensorToken(), new RightTensorToken())
    {
    }
}