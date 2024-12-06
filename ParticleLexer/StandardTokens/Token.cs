namespace ParticleLexer;

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