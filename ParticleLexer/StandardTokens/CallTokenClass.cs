
namespace ParticleLexer.StandardTokens;

/// <summary>
/// Any word followed by group token will be a call token
/// </summary>
public abstract class CallTokenClass(GroupTokenClass groupToken, TokenClass parameterSeparatorToken)
    : TokenClass
{

    /// <summary>
    /// The group that we will look for.
    /// </summary>
    protected readonly GroupTokenClass _GroupToken = groupToken;

    /// <summary>
    /// The token used to separate words in the group.
    /// comma for example
    /// </summary>
    protected readonly TokenClass _ParameterSeparatorToken = parameterSeparatorToken;

    public GroupTokenClass GroupToken => _GroupToken;

    public TokenClass ParameterSeparatorToken => _ParameterSeparatorToken;
}