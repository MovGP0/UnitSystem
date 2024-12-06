
namespace ParticleLexer.StandardTokens;

/// <summary>
/// Groups that are recursive in each other.
/// </summary>
public abstract class GroupTokenClass(TokenClass openToken, TokenClass closeToken) : TokenClass
{
    protected readonly TokenClass _openToken = openToken;
    protected readonly TokenClass _closeToken = closeToken;

    public TokenClass OpenToken
    {
        get { return _openToken; }
    }

    public TokenClass CloseToken
    {
        get { return _closeToken; }
    }
}