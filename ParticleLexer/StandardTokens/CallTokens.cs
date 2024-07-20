
namespace ParticleLexer.StandardTokens
{

    /// <summary>
    /// Any word followed by group token will be a call token
    /// </summary>
    public abstract class CallTokenClass : TokenClass
    {

        /// <summary>
        /// The group that we will look for.
        /// </summary>
        protected readonly GroupTokenClass _GroupToken;

        /// <summary>
        /// The token used to separate words in the group.
        /// comma for example 
        /// </summary>
        protected readonly TokenClass _ParameterSeparatorToken;

        public CallTokenClass(GroupTokenClass groupToken, TokenClass parameterSeparatorToken)
        {
            _GroupToken = groupToken;
            _ParameterSeparatorToken = parameterSeparatorToken;
        }

        public GroupTokenClass GroupToken
        {
            get
            {
                return _GroupToken;
            }
        }

        public TokenClass ParameterSeparatorToken
        {
            get
            {
                return _ParameterSeparatorToken;
            }
        }

    }

    /// <summary>
    /// parameters of call.
    /// </summary>
    public class ParameterToken : TokenClass
    {
    }


    /// <summary>
    /// fifo(p1, p2, ..., pn)    whole token
    /// </summary>
    public class ParenthesisCallToken : CallTokenClass
    {
        public ParenthesisCallToken()
            : base(new ParenthesisGroupToken(), new CommaToken())
        {
        }
    }

    public class SquareBracketsCallToken : CallTokenClass
    {
        public SquareBracketsCallToken()
            : base(new SquareBracketsGroupToken(), new CommaToken())
        {
        }
    }

}
