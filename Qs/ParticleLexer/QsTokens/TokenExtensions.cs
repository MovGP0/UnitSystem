using ParticleLexer.StandardTokens;

namespace ParticleLexer.QsTokens;

public static class TokenExtensions
{
    /// <summary>
    /// Should be used after merging by word
    /// and grouping brackets
    /// The function tries to find Word followed by complete parenthesis group or square brackets group.
    /// And can parse this example:
    ///            fn(434,fn((434+434)+8/4,50,fifo(5)))
    ///            
    ///            S[5,F[4,2,1],G](4, 3, 2,R[2], p(30*o(9)))
    ///            -              - Parameters              -
    ///            -    Indexes   -
    ///            -              Sequence Call             -
    /// </summary>
    /// <param name="ignoreWords">list of words that should be ignored when discovering calls </param>
    /// <returns></returns>
    public static Token DiscoverQsCalls(this Token token, StringComparer stringComparer, params string[] ignoreWords)
    {
        var first = new Token();
        var current = first;

        var ci = 0;

        while (ci < token.Count)
        {
            var c = token[ci];

            if (c.Contains(typeof(ParenthesisGroupToken)) | c.Contains(typeof(SquareBracketsGroupToken)))
            {
                //recursive call if the token have inside groups
                current.AppendSubToken(c.DiscoverQsCalls(stringComparer, ignoreWords));
            }
            else
            {
                //sub groups then test this token
                if (
                    (
                        c.TokenClassType == typeof(WordToken)               // word token
                        || c.TokenClassType == typeof(NameSpaceAndVariableToken)  // or namespace:value token
                    )
                    && ignoreWords.Contains(c.TokenValue, stringComparer) == false         // and the whole value is not in  the ignore words
                )
                {
                    //check if the next token is group
                    if (ci < token.Count - 1)
                    {
                        var cnext = token[ci + 1];

                        #region Parenthesis group discovery
                        if (cnext.TokenClassType == typeof(ParenthesisGroupToken))
                        {
                            // so this is a function
                            //take the current token with the next token and make it as functionToken

                            var functionCallToken = new Token();
                            functionCallToken.TokenClassType = typeof(ParenthesisCallToken);
                            functionCallToken.AppendSubToken(c);



                            if (cnext.Contains((typeof(ParenthesisGroupToken))) | cnext.Contains(typeof(SquareBracketsGroupToken)))
                            {
                                cnext = cnext.DiscoverQsCalls(stringComparer, ignoreWords);
                            }


                            cnext = token.SplitParamerers(cnext, new CommaToken());


                            functionCallToken.AppendSubToken(cnext);

                            current.AppendSubToken(functionCallToken);

                            ci += 2;
                            continue;
                        }
                        #endregion

                        #region Square Brackets discovery
                        if (cnext.TokenClassType == typeof(SquareBracketsGroupToken))
                        {
                            // so this is a sequence
                            //take the current token with the next token and make it as sequenceToken

                            var sequenceCallToken = new Token();
                            sequenceCallToken.TokenClassType = typeof(SequenceCallToken);
                            sequenceCallToken.AppendSubToken(c);

                            if (cnext.Contains((typeof(SquareBracketsGroupToken))) | cnext.Contains((typeof(ParenthesisGroupToken))))
                            {
                                cnext = cnext.DiscoverQsCalls(stringComparer, ignoreWords);
                            }

                            cnext = token.SplitParamerers(cnext, new CommaToken());

                            sequenceCallToken.AppendSubToken(cnext);

                            if (token.Count > ci + 2)
                            {
                                //check if we have a Parenthesis parameters after Square Brackets
                                var cnextnext = token[ci + 2];
                                if (cnextnext.TokenClassType == typeof(ParenthesisGroupToken))
                                {
                                    //then this is a sequence call with parameters.
                                    if ((cnextnext.Contains((typeof(SquareBracketsGroupToken))) | cnextnext.Contains((typeof(ParenthesisGroupToken)))))
                                    {
                                        cnextnext = cnextnext.DiscoverQsCalls(stringComparer, ignoreWords);
                                    }

                                    cnextnext = token.SplitParamerers(cnextnext, new CommaToken());

                                    sequenceCallToken.AppendSubToken(cnextnext);

                                    ci += 3;
                                }
                                else
                                {
                                    ci += 2;
                                }
                            }
                            else
                            {
                                ci += 2;
                            }
                            current.AppendSubToken(sequenceCallToken);
                            continue;
                        }

                        #endregion

                    }
                }

                // if all conditions failed we put the token and resume to another one
                current.AppendSubToken(token[ci]);
            }

            ci++;
        }
        first.TokenClassType = token.TokenClassType;

        return Token.Zabbat(first);
    }



    /// <summary>
    /// Parsing the loop body
    /// Loop Music:Play(c) On c   # where c = ("C", "D", "E", "F", "G", "A", "B")
    /// Loop Loop IO:Poke(i, j, u(i)+v(j)) on i on j
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    public static Token DiscoverQsLoopsTokens(this Token tokens)
    {
        // here I will discover the looping statement  loop expression on vector name

        var root = new Token();
        var runner = root;

        //add every token until you encounter "loop"  then "on"
        var ix = 0;

        Stack<Token> s = new();
        while (ix < tokens.Count)
        {
            if (tokens[ix].TokenClassType == typeof(LoopStatementToken))
            {
                var loopBody = new Token();
                loopBody.TokenClassType = typeof(LoopBodyToken);
                runner.AppendSubToken(loopBody);

                loopBody.AppendSubToken(tokens[ix]); // a// add the "loop" token
                var bodyexpr = new Token() { TokenClassType = typeof(LoopBodyExpressionToken) };
                loopBody.AppendSubToken(bodyexpr);
                runner = bodyexpr;
            }
            else if (tokens[ix].TokenClassType == typeof(OnStatementToken))
            {
                runner = runner.ParentToken;

                runner.AppendSubToken(tokens[ix]);      // the name of on
                runner.AppendSubToken(tokens[ix + 1]);  // the name of the container we are counting on
                ix++;
                runner = runner.ParentToken;
            }
            else
            {
                // add the expressions normally
                runner.AppendSubToken(tokens[ix]);
            }

            ix++;
        }

        return root;
    }
}