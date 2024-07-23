
namespace ParticleLexer.StandardTokens
{
    /// <summary>
    /// Any charachter
    /// </summary>
    public class CharToken : TokenClass
    {

    }



    [TokenPattern(RegexPattern = @"\(", ExactWord = true)]
    public class LeftParenthesisToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\)", ExactWord = true)]
    public class RightParenthesisToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"\[", ExactWord = true)]
    public class LeftSquareBracketToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\]", ExactWord = true)]
    public class RightSquareBracketToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = "{", ExactWord = true)]
    public class LeftCurlyBracketToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "}", ExactWord = true)]
    public class RightCurlyBracketToken : TokenClass
    {
    }



    [TokenPattern(RegexPattern = ",", ExactWord = true)]
    public class CommaToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = ";", ExactWord = true)]
    public class SemiColonToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = ":", ExactWord = true)]
    public class ColonToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\+", ExactWord = true)]
    public class PlusToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"-", ExactWord = true)]
    public class MinusToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"_", ExactWord = true)]
    public class UnderscoreToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\*", ExactWord = true)]
    public class AsteriskToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\@", ExactWord = true)]
    public class AtSignToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"\^", ExactWord = true)]
    public class CaretToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\/", ExactWord = true)]
    public class SlashToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\\", ExactWord = true)]
    public class BackSlashToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "=", ExactWord = true)]
    public class EqualToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"#", ExactWord = true)]
    public class HashToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\$", ExactWord = true)]
    public class DollarToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"'", ExactWord = true)]
    public class ApostropheToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "\"", ExactWord = true)]
    public class QuotationMarkToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"\~", ExactWord = true)]
    public class TildeToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "%", ExactWord = true)]
    public class PercentToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\!", ExactWord = true)]
    public class ExclamationToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = "<", ExactWord = true)]
    public class LessThanToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = ">", ExactWord = true)]
    public class GreaterThanToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "&", ExactWord = true)]
    public class AmpersandToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\?", ExactWord = true)]
    public class QuestionMarkToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "′", ExactWord = true)]
    public class PrimeToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "`", ExactWord = true)]
    public class GraveAccentToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"\.", ExactWord = true)]
    public class PeriodToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"\|", ExactWord = true)]
    public class VerticalBarToken : TokenClass
    {
    }


    [TokenPattern(RegexPattern = @"\s", ExactWord = true)]
    public class SingleSpaceToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\t", ExactWord = true)]
    public class TabToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\r", ExactWord = true)]
    public class CarriageReturnToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\n", ExactWord = true)]
    public class LineFeedToken : TokenClass
    {
    }



}



namespace ParticleLexer
{

    using System.Collections.Generic;
    using StandardTokens;

    public sealed partial class Token : IEnumerable<Token>
    {




        public static Type GetTokenClassType(char c)
        {
            switch (c)
            {
                case '`': return typeof(GraveAccentToken);
                case '~': return typeof(TildeToken);
                case '!': return typeof(ExclamationToken);
                case '@': return typeof(AtSignToken);
                case '#': return typeof(HashToken);
                case '$': return typeof(DollarToken);
                case '%': return typeof(PercentToken);
                case '^': return typeof(CaretToken);
                case '&': return typeof(AmpersandToken);
                case '*': return typeof(AsteriskToken);
                case '(': return typeof(LeftParenthesisToken);
                case ')': return typeof(RightParenthesisToken);
                case '-': return typeof(MinusToken);
                case '_': return typeof(UnderscoreToken);
                case '+': return typeof(PlusToken);
                case '=': return typeof(EqualToken);

                case '{': return typeof(LeftCurlyBracketToken);
                case '[': return typeof(LeftSquareBracketToken);
                case '}': return typeof(RightCurlyBracketToken);
                case ']': return typeof(RightSquareBracketToken);
                case '\\': return typeof(BackSlashToken);
                case '|': return typeof(VerticalBarToken);
                case ';': return typeof(SemiColonToken);
                case ':': return typeof(ColonToken);
                case '\'': return typeof(ApostropheToken);
                case '"': return typeof(QuotationMarkToken);
                case ',': return typeof(CommaToken);
                case '<': return typeof(LessThanToken);
                case '.': return typeof(PeriodToken);
                case '>': return typeof(GreaterThanToken);
                case '/': return typeof(SlashToken);
                case '?': return typeof(QuestionMarkToken);

                case ' ': return typeof(SingleSpaceToken);

                case '\r': return typeof(CarriageReturnToken);
                case '\n': return typeof(LineFeedToken);


                default:
                    return typeof(CharToken);
            }
        }


    }
}