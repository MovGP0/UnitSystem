using ParticleLexer.StandardTokens;

namespace ParticleLexer.QsTokens;

public class NormGroupToken : GroupTokenClass
{
    public NormGroupToken()
        : base(new LeftNormToken(), new RightNormToken())
    {
    }
}