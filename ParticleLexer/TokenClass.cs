using System.Text.RegularExpressions;

namespace ParticleLexer;

public abstract class TokenClass
{
    /// <summary>
    /// The pattern that we will use to tokenizing and merging tokens.
    /// </summary>
    public Regex Regex { get; }

    /// <summary>
    /// Tells if the regex is an exact word without regex classes.
    /// </summary>
    public bool ExactWord { get; }

    /// <summary>
    /// The pattern text.
    /// </summary>
    public string RegexPattern { get; }

    public string OriginalPatternWord => Regex.Unescape(RegexPattern);

    /// <summary>
    /// Condition for the comparing process to make sure that the target should begin with this value. otherwise the code will bypass the current process.
    /// </summary>
    public string ShouldBeginWith { get; }

    /// <summary>
    /// Indicates if parser should continue test after success and consume other tokens or not.
    /// This property by default is false.
    /// </summary>
    public bool ContinueTestAfterSuccess { get; set; } = false;

    public bool ContinousToken { get; set; }

    public string ShouldEndWith { get; }

    //cache token regexes
    static readonly Dictionary<Type, Regex> regexes = new Dictionary<Type, Regex>();

    static readonly Dictionary<Type, bool> exactwords = new Dictionary<Type, bool>();

    static readonly Dictionary<Type, string> words = new Dictionary<Type, string>();

    static readonly Dictionary<Type, string> ShouldBeginingWithList = new Dictionary<Type, string>();

    static readonly Dictionary<Type, bool> ContinueAfterSuccessList = new Dictionary<Type, bool>();

    static readonly Dictionary<Type, bool> ContinousTokenList = new Dictionary<Type, bool>();

    static readonly Dictionary<Type, string> ShouldEndWithList = new Dictionary<Type, string>();

    protected TokenClass()
    {
        Type = GetType();

        // Try Cached versions first
        if (regexes.TryGetValue(Type, out var j))
        {
            Regex = j;

            ExactWord = exactwords[Type];

            RegexPattern = words[Type];

            ShouldBeginWith = ShouldBeginingWithList[Type];

            ContinueTestAfterSuccess = ContinueAfterSuccessList[Type];

            ContinousToken = ContinousTokenList[Type];

            ShouldEndWith = ShouldEndWithList[Type];
        }
        else
        {
            // Cache the regexes due to the multiple creation  of the inherited types
            var rxs = GetType().GetCustomAttributes(false);

            if (rxs.Length > 0)
            {
                var TPA = rxs[0] as TokenPatternAttribute;

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

                regexes.Add(Type, Regex);
                exactwords.Add(Type, ExactWord);
                words.Add(Type, RegexPattern);
                ShouldBeginingWithList.Add(Type, ShouldBeginWith);
                ContinueAfterSuccessList.Add(Type, ContinueTestAfterSuccess);
                ContinousTokenList.Add(Type, ContinousToken);
                ShouldEndWithList.Add(Type, ShouldEndWith);
            }
        }
    }

    /// <summary>
    /// Type of TokenClass
    /// </summary>
    public Type Type { get; }
}
