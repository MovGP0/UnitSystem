using System.Text.RegularExpressions;
using System.Diagnostics;
using ParticleLexer.StandardTokens;

#if NET35
#else
using System.Diagnostics.Contracts;
using System.Text;
using ParticleLexer.CommonTokens;
#endif

namespace ParticleLexer
{
    /// <summary>
    /// Token class
    /// A recursive self contained class that contain tokens with token classes.
    /// </summary>
    public sealed partial class Token : IEnumerable<Token>
    {
        public string Value = string.Empty;
        public Type TokenClassType { get; set; }

        private int _IndexInText;

        /// <summary>
        /// Test if one of the childs of this token is from the given token class
        /// </summary>
        /// <typeparam name="TargetTokenClass">Type of Token Class</typeparam>
        /// <returns></returns>
        public bool Contains<TargetTokenClass>() where TargetTokenClass : TokenClass
        {
            if (childTokens.Count(o => o.TokenClassType == typeof(TargetTokenClass)) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenClassType">Type of token class</param>
        /// <returns></returns>
        public bool Contains(Type tokenClassType)
        {
            if (childTokens.Count(o => o.TokenClassType == tokenClassType) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenClassTypes">Types of token classes</param>
        /// <returns></returns>
        public bool Contains(params Type[] tokenClassTypes)
        {
            foreach(var tokenClassType in tokenClassTypes)
            {
                if (childTokens.Count(o => o.TokenClassType == tokenClassType) > 0)
                    return true;
            }

            return false;
        }

        #region structure & methods

        private List<Token> childTokens = [];
        public ICollection<Token> ChildTokens
        {
            get
            {
                return childTokens;
            }
        }



        public Token AppendSubToken()
        {
            return AppendSubToken(string.Empty);
        }

        public Token AppendSubToken(char value)
        {
            return AppendSubToken(value.ToString());
        }

        public Token AppendSubToken(string value)
        {
            var token = new Token() { Value = value, ParentToken = this };

            childTokens.Add(token);

            return token;
        }
        public void AppendSubToken(Token token)
        {
            token.ParentToken = this;
            childTokens.Add(token);
        }


        public void InsertSubToken(int index, Token token)
        {
            token.ParentToken = this;
            childTokens.Insert(index, token);
        }



        /// <summary>
        /// Indexing Child Tokens
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Token this[int index]
        {
            get
            {
                return childTokens[index];
            }
            set
            {
                childTokens[index] = value;
            }
        }


        /// <summary>
        /// Count of child tokens.
        /// </summary>
        public int Count
        {
            get { return childTokens.Count; }
        }

        public Token ParentToken { get; set; }


        private string _TokenValue = null;
        private int _previousChildrenTokenCount;

        /// <summary>
        /// Token Total String Value
        /// </summary>
        public string TokenValue
        {
            get
            {
                if (_TokenValue == null || _previousChildrenTokenCount != childTokens.Count)
                {
                    if (childTokens.Count > 0)
                    {
                        var total = new StringBuilder();

                        foreach (var t in childTokens)
                        {
                            total.Append( t.TokenValue);
                        }

                        _TokenValue = total.ToString();
                        _previousChildrenTokenCount = childTokens.Count;
                    }
                    else
                    {
                        _TokenValue = Value;
                    }
                }
                return _TokenValue;
            }
        }

        public int TokenValueLength
        {
            get
            {
                return TokenValue.Length;
            }
        }


        /// <summary>
        /// Token index in the original text.
        /// </summary>
        public int IndexInText
        {
            get
            {
                if (childTokens.Count > 0)
                    return childTokens[0].IndexInText;
                else
                    return _IndexInText;
            }
        }

        #endregion

        public string DebugView
        {
            get
            {
                return "[" + IndexInText.ToString() + ", "+TokenClassType.Name + "]: " + TokenValue;
            }
        }
        public override string ToString()
        {
            return DebugView;
        }

        #region Tokenization operations.


        public Token TrimStart<TargetTokenClass>()
            where TargetTokenClass : TokenClass
        {
            return TrimStart(typeof(TargetTokenClass));
        }


        /// <summary>
        /// remove the token from the start of tokens.
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token TrimStart(Type tokenType)
        {
            var Trimmed = new Token();

            var ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];
                if (tok.TokenClassType == tokenType)
                {
                    //ignore this token
                }
                else
                {
                    //from here take the rest tokens
                    while (ci < childTokens.Count)
                    {
                        tok = childTokens[ci];
                        Trimmed.AppendSubToken(tok);
                        ci++;
                    }
                    break;
                }
                ci++;
            }

            return Trimmed;
        }

        public Token TrimEnd<TargetTokenClass>()
            where TargetTokenClass : TokenClass
        {
            return TrimEnd(typeof(TargetTokenClass));
        }

        /// <summary>
        /// Remove the token type from the end of tokens
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token TrimEnd(Type tokenType)
        {
            var Trimmed = new Token();

            var ci = childTokens.Count - 1;
            while (ci >= 0)
            {
                var tok = childTokens[ci];
                if (tok.TokenClassType == tokenType)
                {
                    //ignore this token
                }
                else
                {
                    //from here take the rest tokens
                    for (var i = 0; i <= ci; i++)
                    {
                        tok = childTokens[i];
                        Trimmed.AppendSubToken(tok);
                    }
                    break;
                }

                ci--;
            }

            return Trimmed;
        }




        /// <summary>
        /// Merge all tokens into the one and exclude specific token
        /// </summary>
        /// <param name="tokenType">Excluded tokens or Token(s) that act as separators.</param>
        /// <param name="mergedTokensType">The type of the new token merged from sub tokens.</param>
        /// <returns></returns>
        public Token MergeAllBut(Type mergedTokensType, params TokenClass[] tokenTypes)
        {
            return MergeAllBut(0, mergedTokensType, tokenTypes);
        }

        /// <summary>
        /// Merge all tokens into the one and exclude specific token
        /// </summary>
        /// <param name="tokenType">Excluded tokens or Token(s) that act as separators.</param>
        /// <param name="mergedTokensType">The type of the new token merged from sub tokens.</param>
        /// <returns></returns>
        public Token MergeAllBut<MergedTokenClass>(params TokenClass[] tokenClasses)
            where MergedTokenClass: TokenClass
        {
            return MergeAllBut(0, typeof(MergedTokenClass), tokenClasses);
        }


        /// <summary>
        /// Optimized function to merge all tokens but spaces tokens
        /// </summary>
        /// <typeparam name="MergedTokenClass"></typeparam>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public Token MergeAllButSpaces<MergedTokenClass>(int startIndex = 0)
        {
            var first = this;

            var current = new Token();

            // walk on all tokens and accumulate them unitl you encounter separator

            var ci = 0;

            var mergedTokens = new Token();

            while (ci < first.Count)
            {

                var c = first[ci];

                if (ci < startIndex)
                {
                    current.AppendSubToken(c);
                }
                else
                {
                    if (c.TokenClassType == typeof (SingleSpaceToken)|| c.TokenClassType == typeof(MultipleSpaceToken))
                    {
                        //found a separator
                        if (mergedTokens.Count > 0)
                        {
                            mergedTokens.TokenClassType = typeof(MergedTokenClass);

                            current.AppendSubToken(mergedTokens);
                        }
                        current.AppendSubToken(c);

                        mergedTokens = new Token();
                    }
                    else
                    {
                        mergedTokens.AppendSubToken(c);
                    }
                }

                ci++;
            }

            if (mergedTokens.Count > 0)
            {
                //the rest of merged tokens
                mergedTokens.TokenClassType = typeof(MergedTokenClass);

                current.AppendSubToken(mergedTokens);
            }

            current.TokenClassType = first.TokenClassType;

            return Zabbat(current);

        }


        /// <summary>
        /// Merge all tokens into the one Token with specific TokenClass and exclude specific token classes.
        /// </summary>
        /// <param name="tokenClasses">Excluded token or the separator token.</param>
        /// <param name="mergedTokensClassType">The type of the new token class that was merged from sub tokens.</param>
        /// <param name="startIndex">Starting from token index</param>
        /// <returns></returns>
        public Token MergeAllBut(int startIndex, Type mergedTokensClassType, params TokenClass[] tokenClasses)
        {
            var first = MergeTokens(tokenClasses[0]);
            for (var i = 1; i < tokenClasses.Length; i++)
            {
                first = first.MergeTokens(tokenClasses[i]);
            }

            Debug.Assert(first != null);

            var current = new Token();

            // walk on all tokens and accumulate them unitl you encounter separator

            var ci = 0;

            var mergedTokens = new Token();

            while (ci < first.Count)
            {

                var c = first[ci];

                if (ci < startIndex)
                {
                    current.AppendSubToken(c);
                }
                else
                {
                    if (tokenClasses.Count(tok => tok.GetType() == c.TokenClassType) == 0)
                    {
                        mergedTokens.AppendSubToken(c);
                    }
                    else
                    {
                        //found a separator
                        if (mergedTokens.Count > 0)
                        {
                            mergedTokens.TokenClassType = mergedTokensClassType;

                            current.AppendSubToken(mergedTokens);
                        }
                        current.AppendSubToken(c);

                        mergedTokens = new Token();

                    }
                }

                ci++;
            }

            if (mergedTokens.Count > 0)
            {
                //the rest of merged tokens
                mergedTokens.TokenClassType = mergedTokensClassType;

                current.AppendSubToken(mergedTokens);
            }


            current.TokenClassType = first.TokenClassType;

            return Zabbat(current);

        }


        /// <summary>
        /// Merge Single Tokens into one token guided by regular expression.
        /// </summary>
        /// <returns></returns>
        private  Token MergeTokens(TokenClass tokenClassType)
        {
            var rx = tokenClassType.Regex;
            var current = new Token();
            var merged = new Token();
            var tokIndex = 0;

            while (tokIndex < childTokens.Count)
            {
            loopHead:
                var tok = childTokens[tokIndex];

                var Matched = false;

                Matched = rx.Match(merged.TokenValue + tok.TokenValue).Success;


                if (Matched)
                {
                    //continue merge until merged value fail then last merged value is the desired value.
                    merged.AppendSubToken(tok);
                    merged.TokenClassType = tokenClassType.Type;
                }
                else
                {
                    //merge failed on last token value

                    //now there is a chance that if we consume another letters that we back into the success again
                    //   it is like if we want to compare  tamer , begining with t,a,m,e, will fail untill we reach ,r
                    //     I will make dirty solution to try
                    //      consume rest of tokens until found a success or end the discussion (end of tokens) :)
                    //
                    //   The behavior above now is modified when marked the token class as ExactWord

                    #region Failed in match

                    if (!string.IsNullOrEmpty(merged.TokenValue) && tokenClassType.ContinousToken != true)
                    {
                        // don't continue if the target token doesn't start with the required text
                        if (!string.IsNullOrEmpty(tokenClassType.ShouldBeginWith))
                        {
                            if (!merged.TokenValue.StartsWith(tokenClassType.ShouldBeginWith, StringComparison.OrdinalIgnoreCase)) goto NoStartWith;
                        }



                        // inner sneaky loop. :)
                        var rtokIndex = tokIndex;
                        var AccumulatedInnerText = merged.TokenValue;

                        while (rtokIndex < childTokens.Count)
                        {
                            AccumulatedInnerText += childTokens[rtokIndex].TokenValue;

                            //however if the token is marked in tokenclass as ExactWord
                            //  then comparing more characters than actual ones is useless
                            //  and will result un-needed cycles.
                            if (tokenClassType.ExactWord)
                            {
                                // two checks because of exact word flag

                                // first check: is the length of accumlated
                                if (AccumulatedInnerText.Length > tokenClassType.OriginalPatternWord.Length)
                                {
                                    // no need to compare extra charachters ... so BREAAAAAAAAAAK
                                    goto WhileBreak;
                                }

                                // second check: check letter by letter that they are identical
                                for (var iai = 0; iai < AccumulatedInnerText.Length; iai++)
                                    if (AccumulatedInnerText[iai] != tokenClassType.OriginalPatternWord[iai]) goto WhileBreak;
                            }
                            else if (!string.IsNullOrEmpty(tokenClassType.ShouldBeginWith))
                            {
                                // pattern is not word token however we can test if the consumed charachters are in the pattern
                                // test that the accumulated text begins with the token beginwith value.
                                // otherwise no need to make other accumulation.

                                if (!AccumulatedInnerText.StartsWith(tokenClassType.ShouldBeginWith, StringComparison.OrdinalIgnoreCase)) goto WhileBreak;
                            }


                            // terminate on specific conditions

                            // reaching new line:  if you reach new line then most probably we don't need to check more for that token
                            //if (AccumulatedInnerText.EndsWith(Environment.NewLine)) goto WhileBreak;


                            // go with comparing.
                            if (rx.IsMatch(AccumulatedInnerText))
                            {
                                //  after we run over tokens for unknown steps we found a success
                                // merge all tokens that made the success
                                // alter the original loop index and go to the loop tail
                                for (; tokIndex <= rtokIndex; tokIndex++) merged.AppendSubToken(childTokens[tokIndex]);


                                if (tokIndex < childTokens.Count)
                                {
                                    if (tokenClassType.ContinueTestAfterSuccess)
                                    {
                                        goto loopHead;
                                    }
                                    else
                                    {
                                        // store the discovered token and continue after the last token
                                        merged.TokenClassType = tokenClassType.Type;
                                        current.AppendSubToken(merged);
                                        merged = new Token();

                                        goto loopHead;
                                    }
                                }
                                else
                                {
                                    goto loopTail;
                                }
                            }


                            if (!string.IsNullOrEmpty(tokenClassType.ShouldEndWith))
                            {
                                // Why we break here ??
                                //   because the accumulated text is greedy and try to find a completion for the token that we are comparing with
                                //   Accumulated text is grabbing the next token string and compare with the tokenclasstype
                                //   when compare success in the above code regex match then every thing is ok
                                //    however if match didn't occur we will continue consumeing tokens until the end of the tokens
                                //   to prevent this we will chech for the ending of the accumulated text
                                //    if we found the ending resemble the end string of target token class then we will assume that we don't have matching token suitable and
                                //    we will end the loop to prevent unnecessary loops
                                if (AccumulatedInnerText.EndsWith(tokenClassType.ShouldEndWith, StringComparison.OrdinalIgnoreCase)) goto WhileBreak;
                            }

                            rtokIndex++;
                        }

                    WhileBreak: ;

                    }

                    #endregion

                NoStartWith: ;

                    // if merged token is not null put the merged value
                    //  continue to test the last token with next tokens to the same regex
                    if (!string.IsNullOrEmpty(merged.TokenValue))
                    {
                        if (rx.IsMatch(merged.TokenValue))
                        {
                            merged.TokenClassType = tokenClassType.Type;
                        }

                        current.AppendSubToken(merged);
                        merged = new Token();


                        // for begining another test with the new token
                        merged.AppendSubToken(tok);

                        merged.TokenClassType = tok.TokenClassType;
                    }
                    else
                    {
                        //merged token is null
                        merged.AppendSubToken(tok);
                        merged.TokenClassType = tok.TokenClassType;
                    }
                }

            loopTail:
                tokIndex++;

            }

            if (!string.IsNullOrEmpty(merged.TokenValue))
            {
                if (rx.IsMatch(merged.TokenValue)) merged.TokenClassType = tokenClassType.GetType();
                current.AppendSubToken(merged);
            }

            current.TokenClassType = TokenClassType;
            return Zabbat(current);
        }


        /// <summary>
        /// Merge multiple exact words in the same time to enhance performance.
        /// </summary>
        /// <param name="tokenClasses"></param>
        /// <returns></returns>
        public Token MergeMultipleWordTokens(params Type[] tokenClassesTypes)
        {
            #if NET35
            if (tokenClassesTypes == null) throw new ArgumentException("Null parameters");
            if (tokenClassesTypes.Length < 1) throw new ArgumentException("No parameter passed");
            #else
            Contract.Requires(tokenClassesTypes != null);
            Contract.Requires(tokenClassesTypes.Length > 0);
            #endif

            List<TokenClass> tokenClasses = new List<TokenClass>(tokenClassesTypes.Length);
            foreach(var tt in tokenClassesTypes)
            {
                var tc = GetTokenClass(tt);
                if (tc.ExactWord==false) throw new InvalidOperationException("Only exact word tokens are allowed");
                tokenClasses.Add(tc);
            };

            // the required tokens are exact words that have a definite start and a definite end.
            var current = new Token();
            var merged = new Token();
            var tokIndex = 0;

            var TokenClassesIndex = 0;
            var CurrentTokenClass = tokenClasses[TokenClassesIndex]; //the token that will be compared with.

            while (tokIndex < childTokens.Count)
            {
                var tok = childTokens[tokIndex];

                while(TokenClassesIndex < tokenClasses.Count)
                {
                    CurrentTokenClass = tokenClasses[TokenClassesIndex];
                    var subTokIndex = tokIndex;
                    // the token in test should be less than the compare token.
                    if(CurrentTokenClass.OriginalPatternWord.StartsWith(tok.TokenValue, StringComparison.OrdinalIgnoreCase))
                    {

                        while(subTokIndex < ChildTokens.Count)
                        {
                            var stok = childTokens[subTokIndex];
                            merged.AppendSubToken(stok);


                            if (CurrentTokenClass.OriginalPatternWord.StartsWith(merged.TokenValue, StringComparison.OrdinalIgnoreCase))
                            {
                                if (merged.TokenValue.Length == CurrentTokenClass.OriginalPatternWord.Length)
                                {
                                    // comparison is right and done.
                                    merged.TokenClassType = CurrentTokenClass.Type;
                                    current.AppendSubToken(merged);
                                    tokIndex = subTokIndex;
                                    goto loopTail;
                                }
                            }
                            else
                            {
                                break;
                            }
                            subTokIndex++;
                        }
                    }

                    // condition failed either because the token value is greater than the compare or simple is not begining with this token
                    //  or we didn't get this token either.
                    // so we go for another tokenclass to compare

                    if (merged.Count > 0) merged = new Token(); //reset the merged object because it was modified
                    TokenClassesIndex++;

                }

                // we didn't find any match that this token can be merged into it
                // so end up putting it as it is.
                current.AppendSubToken(tok);

            loopTail:
                tokIndex++;
                TokenClassesIndex = 0;
            }

            current.TokenClassType = TokenClassType;
            return Zabbat(current);
        }


        private static Dictionary<Type, TokenClass> CachedTokenClasses = new Dictionary<Type, TokenClass>();

        /// <summary>
        /// Gets <see cref="TokenClass"/> instance from cache if available.
        /// </summary>
        /// <param name="tokenClassType"></param>
        /// <returns></returns>
        private TokenClass GetTokenClass(Type tokenClassType)
        {
            TokenClass instance;
            CachedTokenClasses.TryGetValue(tokenClassType, out instance);
            if (instance == null)
            {
                instance = (TokenClass)Activator.CreateInstance(tokenClassType);
                CachedTokenClasses.Add(tokenClassType, instance);
            }
            return instance;
        }

        public TokenClass GetTokenClass<DesiredTokenClass>() where DesiredTokenClass : TokenClass, new()
        {
            TokenClass instance;
            CachedTokenClasses.TryGetValue(typeof(DesiredTokenClass), out instance);
            if (instance == null)
            {
                instance = new DesiredTokenClass();
                CachedTokenClasses.Add(typeof(DesiredTokenClass), instance);
            }
            return instance;
        }

        /// <summary>
        /// Merge tokens based on token class.
        /// </summary>
        /// <typeparam name="DesiredTokenClass">Token Class Type like <see cref="WordToken"/></typeparam>
        /// <returns></returns>
        public Token MergeTokens<DesiredTokenClass>() where DesiredTokenClass: TokenClass, new()
        {
            return MergeTokens(GetTokenClass<DesiredTokenClass>());
        }

        /// <summary>
        /// Merge the repeated token into Merged Token
        /// </summary>
        /// <typeparam name="MergedToken"></typeparam>
        /// <param name="RepeatedToken"></param>
        /// <returns></returns>
        public Token MergeRepetitiveTokens<MergedToken, RepeatedToken>()
        {
            var current = new Token();

            var merged = new Token() { TokenClassType = typeof(MergedToken) };

            var tokIndex = 0;
            while (tokIndex < childTokens.Count)
            {
                if (childTokens[tokIndex].TokenClassType == typeof(RepeatedToken))
                {
                    merged.AppendSubToken(childTokens[tokIndex]);
                }
                else
                {

                    if (merged.Count > 0)
                    {
                        current.AppendSubToken(merged);
                        merged = new Token() { TokenClassType = typeof(MergedToken) };
                    }

                    // add the current index.
                    current.AppendSubToken(childTokens[tokIndex]);
                }

                tokIndex++;
            }

            return Zabbat(current);
        }

        /// <summary>
        /// Merge a set of known sequence of tokens into a single with specific token class type.
        /// </summary>
        /// <typeparam name="DesiredTokenClass"></typeparam>
        /// <param name="tokenTypes"></param>
        /// <returns></returns>
        public Token MergeSequenceTokens<DesiredTokenClass>(params Type[] tokenTypes) where DesiredTokenClass : TokenClass, new()
        {

            var ComparisonTokensNumber = tokenTypes.Length;

            var CurrentTokenTypeIndex = 0;    // hold the current index of the required tokens
                                              // when the index reaches the ComparisonTokensNumber the compare ends and
                                              //  the merge occure.

            var current = new Token();

            var merged = new Token() { TokenClassType = typeof(DesiredTokenClass) };


            var tokIndex = 0;
            while (tokIndex < childTokens.Count)
            {
                if (childTokens[tokIndex].TokenClassType == tokenTypes[CurrentTokenTypeIndex])
                {
                    merged.AppendSubToken(childTokens[tokIndex]);

                    CurrentTokenTypeIndex++;

                    if (CurrentTokenTypeIndex == ComparisonTokensNumber)
                    {
                        // include the merge token
                        current.AppendSubToken(merged);
                        merged = new Token() { TokenClassType = typeof(DesiredTokenClass) };
                        CurrentTokenTypeIndex = 0;
                    }
                    else
                    {
                        // do nothing :) :) :) :D :D :D
                    }
                }
                else
                {
                    if (CurrentTokenTypeIndex > 0)
                    {
                        // failure after merging some tokens so we have to empty the merged token
                        //  then roll back to the first token we were in to begin after that token again
                        //    illustration
                        //      imagine we  merge Dollar Sign Token + Word Token
                        //      imagine we have this sequence   $$Hello  the second dollar will fail the comparison
                        //      so we have to start from the second dollar token :)

                        tokIndex -= merged.Count;  // return the index to the first index of this token

                        merged = new Token() { TokenClassType = typeof(DesiredTokenClass) };
                        CurrentTokenTypeIndex = 0;
                    }

                    // add the current index.
                    current.AppendSubToken(childTokens[tokIndex]);

                }

                tokIndex++;
            }

            if (merged.Count > 0)
            {
                // then tokens were run out before we know if we can merge this into current or not
                // so take all tokens in merged and add it to current.
                for (var i = 0; i < merged.Count; i++)
                    current.AppendSubToken(merged[i]);

                merged = null;
            }

            return Zabbat(current);
        }


        /// <summary>
        /// This function make sure that inner tokens are not the same as outer tokens by popping out the
        /// buried tokens to the surface.
        /// </summary>
        /// <param name="melakhbat"></param>
        /// <returns></returns>
        public static Token Zabbat(Token melakhbat)
        {
            var Metzabbat = new Token();
            foreach (var h in melakhbat)
            {
                if (h.Count == 1)
                {
                    if (h.TokenClassType == h[0].TokenClassType) Metzabbat.AppendSubToken(h[0]);
                    else Metzabbat.AppendSubToken(h);
                }
                else
                {
                    Metzabbat.AppendSubToken(h);
                }
            }

            Metzabbat.TokenClassType = melakhbat.TokenClassType;

            return Metzabbat;
        }


        /// <summary>
        /// Removes specific tokens from the current tokens.
        /// </summary>
        /// <param name="tokenType">Array of tokens that should be removed.</param>
        /// <returns></returns>
        public Token RemoveTokens(params Type[] tokenTypes)
        {
            var first = new Token();
            var current = first;

            var ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                if (tokenTypes.Count(f => f == tok.TokenClassType) > 0)
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }

        /// <summary>
        /// Returns the first occurance of specific token class type
        /// </summary>
        /// <typeparam name="TargetTokenClass">Desired Class Token Type</typeparam>
        /// <returns></returns>
        public int IndexOf<TargetTokenClass>()
            where TargetTokenClass : TokenClass
        {
            return IndexOf(typeof(TargetTokenClass));
        }

        /// <summary>
        /// Returns the first occurance of specific token class type
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public int IndexOf(Type tokenClassType)
        {
            var idx = -1;
            for (var i = 0; i < Count; i++)
            {
                if (this[i].TokenClassType == tokenClassType)
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }


        /// <summary>
        /// Remove specific token starting from the first token till the terminating token encountered while removing then the process ends.
        /// </summary>
        /// <typeparam name="TargetTokenClass"></typeparam>
        /// <typeparam name="TerminatingTokenClass"></typeparam>
        /// <returns></returns>
        public Token RemoveTokenUntil<TargetTokenClass, TerminatingTokenClass>()
            where TargetTokenClass: TokenClass
            where TerminatingTokenClass: TokenClass
        {
            return RemoveTokenUntil(typeof(TargetTokenClass), typeof(TerminatingTokenClass));
        }

        /// <summary>
        /// Remove specific token until we reach a closing token.
        /// </summary>
        /// <returns></returns>
        public Token RemoveTokenUntil(Type tokenType, Type untilToken)
        {
            var first = new Token();
            var current = first;

            var reached = false;  //specifiy if we reach the close token or not.

            var ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                if (tokenType == tok.TokenClassType && reached == false)
                {
                    //all string are white spaces
                }
                else
                {
                    if (tok.TokenClassType == untilToken) reached = true;

                    //not the required token add it to the return value.
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }


        /// <summary>
        /// Search for <see cref="MultipleSpaceToken"/> tokens and remove them from the children tokens.
        /// Used after Merging tokens with <see cref="MultipleSpaceToken"/>
        /// </summary>
        /// <returns></returns>
        public Token RemoveSpaceTokens()
        {
            var first = new Token();
            var current = first;

            var ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                //if (tok.TokenValue.ToCharArray().Count(w => char.IsWhiteSpace(w)) == tok.TokenValue.Length)
                if(tok.TokenClassType == typeof(MultipleSpaceToken))
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }

        /// <summary>
        /// Search for any space token and remove it
        /// </summary>
        /// <returns></returns>
        public Token RemoveAnySpaceTokens()
        {
            var first = new Token();
            var current = first;

            var ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces


                if (tok.TokenClassType == typeof(MultipleSpaceToken) || tok.TokenClassType == typeof(SingleSpaceToken))
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }

        /// <summary>
        /// Removes New Lines Tokens
        /// </summary>
        /// <returns></returns>
        public Token RemoveNewLineTokens()
        {
            var first = new Token();
            var current = first;

            var ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces


                if (tok.TokenClassType == typeof(CarriageReturnToken) || tok.TokenClassType == typeof(LineFeedToken))
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }



        /// <summary>
        /// Returns the value of tokens starting from specific token.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public string SubTokensValue(int startIndex)
        {
            var idx = startIndex;
            var total = string.Empty;
            while (idx < Count)
            {
                total += this[idx].TokenValue;
                idx++;
            }
            return total;

        }

        /// <summary>
        /// Get inner tokens from leftIndex to the rightIndex
        /// --->   tokens &lt; --
        /// </summary>
        /// <param name="leftIndex"></param>
        /// <param name="rightIndex"></param>
        /// <returns>Return new token with sub tokens trimmed</returns>
        public Token TrimTokens(int leftIndex, int rightIndex)
        {
            var count = Count;


            var rtk = new Token();
            for (var b = leftIndex; b < count - rightIndex; b++)
            {
                rtk.AppendSubToken(this[b]);
            }

            return rtk;

        }


        /// <summary>
        /// Extend Tokens from Left and Right and Fuse them into one Token with specific token class
        /// </summary>
        /// <param name="leftText"></param>
        /// <param name="rightText"></param>
        /// <returns>Return token with sub tokens extended and fused</returns>
        public Token FuseTokens<FusedTokenClass>(string leftText, string rightText)
            where FusedTokenClass : TokenClass
        {
            var count = Count;

            var rtk = new Token();

            foreach (var t in ParseText(leftText))
            {
                rtk.AppendSubToken(t);
            }

            for (var b = 0; b < count; b++)
            {
                rtk.AppendSubToken(this[b]);
            }
            foreach (var t in ParseText(rightText))
            {
                rtk.AppendSubToken(t);
            }

            rtk.TokenClassType = typeof(FusedTokenClass);

            var tk = new Token();
            tk.AppendSubToken(rtk);

            return tk;
        }


        /// <summary>
        /// Parse text between " Double Qoutation marks and escape it \" with back slash \" "
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public Token TokenizeTextStrings()
        {

            var tokens = this;

            // merge \" to be one charachter after this

            tokens = tokens.MergeTokens<QuotationMarkEscapeToken>();

            var root = new Token();

            var runner = root;

            //add every token until you encounter '


            var ix = 0;
            var TextMode = false;
            while (ix < tokens.Count)
            {
                if (tokens[ix].TokenClassType == typeof(QuotationMarkToken))
                {
                    TextMode = !TextMode;

                    if (TextMode)
                    {
                        //true create the token
                        runner = new Token();
                        runner.TokenClassType = typeof(TextStringToken);
                        root.AppendSubToken(runner);

                        runner.AppendSubToken(tokens[ix]);

                    }
                    else
                    {
                        //false: return to root tokens
                        runner.AppendSubToken(tokens[ix]);

                        runner = root;
                    }
                }
                else
                {
                    runner.AppendSubToken(tokens[ix]);
                }


                ix++;

            }


            return root;
        }




        #endregion

        #region Helper Functions

        /// <summary>
        /// Convert all charachters in the string into tokens with their classes types and CharachterToken for unknown ones.
        /// This function represent the entry point of the tokenization process.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Token ParseText(string text)
        {
            var current = new Token();

            var ci = 0;
            while (ci < text.Length)
            {
                var c = text[ci];
                {
                    var tk = current.AppendSubToken(c);
                    tk.TokenClassType = GetTokenClassType(c);
                    tk._IndexInText = ci;
                }

                ci++;
            }
            return current;
        }

        #endregion



        #region IEnumerable<Token> Members

        public IEnumerator<Token> GetEnumerator()
        {
            return childTokens.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return childTokens.GetEnumerator();
        }

        #endregion
    }
}
