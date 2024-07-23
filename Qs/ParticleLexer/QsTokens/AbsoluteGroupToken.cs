using ParticleLexer.StandardTokens;

namespace ParticleLexer.QsTokens;

public sealed class AbsoluteGroupToken : GroupTokenClass
{
    public AbsoluteGroupToken()
        : base(new LeftAbsoluteToken(), new RightAbsoluteToken())
    {
    }
}