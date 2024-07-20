using System.Text.RegularExpressions;

namespace ParticleLexer
{
    public abstract class TokenClass
    {

        /// <summary>
        /// The pattern that we will use to tokenizing and merging tokens.
        /// </summary>
        public Regex Regex
        {
            get; 
            private set;
        }


        /// <summary>
        /// Tells if the regex is an exact word without regex classes.
        /// </summary>
        public bool ExactWord
        {
            get;
            private set;
        }

        /// <summary>
        /// The pattern text.
        /// </summary>
        public string RegexPattern
        {
            get;
            private set;
        }

        public string OriginalPatternWord
        {
            get
            {
                return Regex.Unescape(RegexPattern);
            }
        }

        /// <summary>
        /// Condition for the comparing process to make sure that the target should begin with this value. otherwise the code will bypass the current process.
        /// </summary>
        public string ShouldBeginWith { get; private set; }

        private bool _ContinueTestAfterSuccess = false;

        /// <summary>
        /// Indicates if parser should continue test after success and consume other tokens or not.
        /// This property by default is false.
        /// </summary>
        public bool ContinueTestAfterSuccess
        {
            get { return _ContinueTestAfterSuccess; }
            set { _ContinueTestAfterSuccess = value; }
        }

        public bool ContinousToken { get; set; }

        public string ShouldEndWith { get; private set; }


        //cache token regexes
        static Dictionary<Type, Regex> regexes = new Dictionary<Type, Regex>();
        static Dictionary<Type, bool> exactwords = new Dictionary<Type, bool>();
        static Dictionary<Type, string> words = new Dictionary<Type, string>();
        static Dictionary<Type, string> ShouldBeginingWithList = new Dictionary<Type, string>();
        static Dictionary<Type, bool> ContinueAfterSuccessList = new Dictionary<Type, bool>();
        static Dictionary<Type, bool> ContinousTokenList = new Dictionary<Type, bool>();
        static Dictionary<Type, string> ShouldEndWithList = new Dictionary<Type, string>();

        /*
         * worth mentioned note that when I cached the regexes in this part the console calculations went very fast
         * I couldn't imagine that the reflection here make a lot of slowness.
         */

        private Type _ThisTokenType;
        public TokenClass()
        {

            _ThisTokenType = this.GetType();
            
            Regex j;

            // Try Cached versions first
            if (regexes.TryGetValue(_ThisTokenType, out j))
            {
                Regex = j;

                ExactWord = exactwords[_ThisTokenType];

                RegexPattern = words[_ThisTokenType];

                ShouldBeginWith = ShouldBeginingWithList[_ThisTokenType];

                ContinueTestAfterSuccess = ContinueAfterSuccessList[_ThisTokenType];

                ContinousToken = ContinousTokenList[_ThisTokenType];

                ShouldEndWith = ShouldEndWithList[_ThisTokenType];

            }
            else
            {

                // Cache the regexes due to the multiple creation  of the inherited types
                var rxs = this.GetType().GetCustomAttributes(false);

                if (rxs.Length > 0)
                {
                    TokenPatternAttribute TPA = rxs[0] as TokenPatternAttribute;

                    if (TPA != null)
                    {
                        Regex = new Regex("^" + TPA.RegexPattern + "$", RegexOptions.IgnoreCase);
                        ExactWord = TPA.ExactWord;
                        RegexPattern = TPA.RegexPattern;
                        ShouldBeginWith = TPA.ShouldBeginWith;
                        ContinueTestAfterSuccess = TPA.ContinueTestAfterSuccess;
                        ContinousToken = TPA.ContinousToken;
                        ShouldEndWith = TPA.ShouldEndWith;
                    }

                    regexes.Add(_ThisTokenType, Regex);
                    exactwords.Add(_ThisTokenType, ExactWord);
                    words.Add(_ThisTokenType, RegexPattern);
                    ShouldBeginingWithList.Add(_ThisTokenType, ShouldBeginWith);
                    ContinueAfterSuccessList.Add(_ThisTokenType, ContinueTestAfterSuccess);
                    ContinousTokenList.Add(_ThisTokenType, ContinousToken);
                    ShouldEndWithList.Add(_ThisTokenType, ShouldEndWith);
                }
            }
        }



        /// <summary>
        /// Type of TokenClass 
        /// </summary>
        public Type Type
        {
            get
            {
                return _ThisTokenType;
            }
        }
    }
}
