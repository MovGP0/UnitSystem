using ParticleLexer.StandardTokens;

namespace ParticleLexer
{
    public sealed partial class Token : IEnumerable<Token>
    {
        
        /// <summary>
        /// Merge between known start token and different closing tokens with different merged tokens classes
        /// </summary>
        /// <param name="startTokenClass">the start token type</param>
        /// <param name="closeTokenClasses">array of closing tokens</param>
        /// <param name="mergedTokenClasses">array of types of extracted token classes corresponds to closing tokens</param>
        /// <returns></returns>
        public Token MergeTokensBetween(Type startTokenClass, Type[] closeTokenClasses, Type[] mergedTokenClasses) 
        {
            if (closeTokenClasses.Length != mergedTokenClasses.Length) throw new ArgumentException("Number of closing and merged token classes should be equal");

            Token first = new Token();
            Token current = first;

            int ci = 0;

            bool InComplete = false;

            while (ci < childTokens.Count)
            {
                var c = childTokens[ci];
                if (c.TokenClassType == startTokenClass)
                {
                    if (InComplete)
                    {
                        // we didn't encounter ending token   so we put the current tokens into its parents
                        var p = current.ParentToken;
                        foreach (var t in current) p.AppendSubToken(t);
                        current = p;
                        InComplete = false;
                    }

                    current = current.AppendSubToken();

                    // current now is a new container token

                    // put the open token in the container.
                    current.AppendSubToken(c);

                    InComplete = true;
                    goto LoopAgain;
                }

                if (closeTokenClasses.Contains(c.TokenClassType) && InComplete == true)
                {
                    //put the close token inside the current container
                    current.AppendSubToken(c);
                    int ix = 0;
                    for (; ix < closeTokenClasses.Length; ix++)
                    {
                        if (closeTokenClasses[ix] == c.TokenClassType)
                        {
                            // mark the current container with the group class type.
                            current.TokenClassType = mergedTokenClasses[ix];
                            break;
                        }
                    }

                    current = current.ParentToken;

                    InComplete = false;

                    goto LoopAgain;
                }
                //not open nor close.
                current.AppendSubToken(c);

            LoopAgain:
                ci++;
            }

            if (InComplete)
            {
                // we didn't encounter ending token   so we put the current tokens into its parents
                var p = current.ParentToken;
                foreach (var t in current) p.AppendSubToken(t);
                current = null;
            }

            return Zabbat(first);
        }


        /// <summary>
        /// Assemble single tokens into groups like open and close parenthesis 
        ///     (()(())[[[][]])
        ///     <see cref="ParenthesisGroupToken"/>
        ///     <see cref="SquareBracketsGroupToken"/>
        ///     <seealso cref="GroupTokenClass"/>
        /// </summary>
        /// <param name="groupsTokens"></param>
        /// <returns></returns>
        public Token MergeTokensInGroups(params GroupTokenClass[] groupsTokens)
        {
            
            Token first = new Token();
            Token current = first;

            int ci = 0;

            while (ci < childTokens.Count)
            {
                var c = childTokens[ci];

                // discover the open token
                foreach (var GroupToken in groupsTokens)
                {
                    if (c.TokenClassType == GroupToken.OpenToken.Type)
                    {
                        current = current.AppendSubToken();

                        // current now is a new container token
                        
                        // put the open token in the container.
                        current.AppendSubToken(c);

                        goto LoopAgain;
                    }
                }

                // discover the close token.
                foreach (var GroupToken in groupsTokens)
                {
                    if (c.TokenClassType == GroupToken.CloseToken.Type)
                    {
                        //put the close token inside the current container
                        current.AppendSubToken(c);

                        // mark the current container with the group class type.
                        current.TokenClassType = GroupToken.Type;

                        current = current.ParentToken;

                        goto LoopAgain;
                    }
                }

                //not open nor close.
                current.AppendSubToken(c);


            LoopAgain:
                ci++;
            }

            return Zabbat(first);
        }

        
        /// <summary>
        /// After merging words and grouping brackets
        /// this function try to discover the calls to sequences and functions.
        /// </summary>
        /// <returns></returns>
        public Token DiscoverCalls(params CallTokenClass[] callClasses)
        {
            return DiscoverCalls(StringComparer.OrdinalIgnoreCase, callClasses,new Type[] { typeof(WordToken) }, new string[] { });
        }

        /// <summary>
        /// Should be used after merging by word
        /// and grouping brackets
        /// The function tries to find Word followed by complete parenthesis group or square brackets group.
        ///     or desired group
        /// And can parse this example:
        ///            fn(434,fn((434+434)+8/4,50,fifo(5)))
        ///            
        ///            S[5,F[4,2,1],G](4, 3, 2,R[2], p(30*o(9)))
        ///            -              - Parameters              -
        ///            -    Indexes   -
        ///            -              Sequence Call             -
        /// </summary>
        /// <param name="ignoreWords">list of words that should be ignored when discovering calls </param>
        /// <param name="wordTokensClasses">list of word tokens to be included in search</param>
        /// <param name="callTokensClasses"> list of call tokens classes that will be discovered like f[3,2] and g(3,f[3]) in the same time</param>
        /// <returns></returns>
        public Token DiscoverCalls(StringComparer stringComparer, CallTokenClass[] callTokensClasses,Type[] wordTokensClasses, string[] ignoreWords) 
        {
            

            var GroupsClassesTypes = (from gct in callTokensClasses
                                     select gct.GroupToken.Type).ToArray();
            

            Token first = new Token();
            Token current = first;

            int ci = 0;

            while (ci < childTokens.Count)
            {
                var c = childTokens[ci];

                if (c.Contains(GroupsClassesTypes))
                {
                    //recursive call if the token have inside groups
                    current.AppendSubToken(c.DiscoverCalls(stringComparer, callTokensClasses, wordTokensClasses, ignoreWords));
                }
                else
                {
                    //sub groups then test this token 
                    if (
                        (
                              wordTokensClasses.Contains(c.TokenClassType) 
                        )
                        && ignoreWords.Contains(c.TokenValue, stringComparer) == false         // and the whole value is not in  the ignore words
                        )
                    {
                        //check if the next token is group
                        if (ci < this.Count - 1)
                        {
                            Token cnext = childTokens[ci + 1];

                            #region group discovery
                            // see if the next token class type is among the types we are checking for.
                            if (GroupsClassesTypes.Contains(cnext.TokenClassType))
                            {
                                // so this is a call pattern
                                // find the CALL class type of this group from the original CallTokenClasses
                                CallTokenClass TargetCallClass = callTokensClasses.First(o => o.GroupToken.Type == cnext.TokenClassType);
                                //take the current token with the next token and make it as functionToken

                                Token targetCallToken = new Token();
                                targetCallToken.TokenClassType = TargetCallClass.Type;
                                targetCallToken.AppendSubToken(c);

                                // look a further look inside the group see if it enclose 
                                //   other groups that needs to be looked out before we finish this call
                                
                                if (cnext.Contains(GroupsClassesTypes))
                                {
                                    cnext = cnext.DiscoverCalls(stringComparer, callTokensClasses,wordTokensClasses, ignoreWords);
                                }

                                cnext = SplitParamerers(cnext, TargetCallClass.ParameterSeparatorToken);

                                targetCallToken.AppendSubToken(cnext);

                                current.AppendSubToken(targetCallToken);

                                ci += 2;
                                continue;
                            }
                            #endregion
                        }
                    }

                    // if all conditions failed we put the token and resume to another one
                    current.AppendSubToken(childTokens[ci]);
                }

                ci++;
            }

            first.TokenClassType = this.TokenClassType;

            return Zabbat(first);
        }


        /// <summary>
        /// Merge tokens inside group token ignore the first and last token and merge all tokens inside them leaving the separator as it is
        /// then mark the merged tokens as ParameterToken
        /// </summary>
        /// <param name="cnext"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public Token SplitParamerers(Token cnext, TokenClass separator)
        {
            
            //here I must merge all but the parenthesis
            Token temp = new Token();

            for (int iy = 1; iy < cnext.Count - 1; iy++)
                temp.AppendSubToken(cnext[iy]);


            temp = temp.MergeAllBut<ParameterToken>(separator);

            Token tmp2 = new Token();
            tmp2.TokenClassType = cnext.TokenClassType;
            tmp2.AppendSubToken(cnext[0]);

            foreach (Token itmp in temp)
                tmp2.AppendSubToken(itmp);

            tmp2.AppendSubToken(cnext[cnext.Count - 1]);

            return Zabbat(tmp2);
        }


        




    }
}