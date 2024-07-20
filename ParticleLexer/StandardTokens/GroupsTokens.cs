
namespace ParticleLexer.StandardTokens
{


    /// <summary>
    /// Groups that are recursive in each other.
    /// </summary>
    public abstract class GroupTokenClass : TokenClass 
    {
        protected readonly TokenClass _openToken;
        protected readonly TokenClass _closeToken;

        public TokenClass OpenToken
        {
            get { return _openToken; }
        }

        public TokenClass CloseToken
        {
            get { return _closeToken; }
        } 

        public GroupTokenClass(TokenClass openToken, TokenClass closeToken)
        {
            _openToken = openToken;
            _closeToken = closeToken;
        }
    }

    /// <summary>
    /// () groups
    /// </summary>
    public class ParenthesisGroupToken : GroupTokenClass
    {
        public ParenthesisGroupToken()
            : base(new LeftParenthesisToken(), new RightParenthesisToken())
        {
        }

    }


    /// <summary>
    /// [] Groups
    /// </summary>
    public class SquareBracketsGroupToken : GroupTokenClass
    {
        public SquareBracketsGroupToken()
            : base(new LeftSquareBracketToken(), new RightSquareBracketToken())
        {
        }

    }


    /// <summary>
    /// {} Groups
    /// </summary>
    public class CurlyBracketGroupToken : GroupTokenClass
    {
        public CurlyBracketGroupToken()
            : base(new LeftCurlyBracketToken(), new RightCurlyBracketToken())
        {
        }
    }

}
