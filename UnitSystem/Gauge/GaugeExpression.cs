using System.Collections.Frozen;
using System.Diagnostics;
using FParsec.CSharp;
using static FParsec.CSharp.CharParsersCS;
using static FParsec.CSharp.PrimitivesCS;
using static UnitSystem.Gauge.StandardModelGaugeRepresentations;

namespace UnitSystem.Gauge;

/// <summary>A parsed expression of named fields for the Standard Model gauge checker.</summary>
public sealed class GaugeExpression
{
    private const int MaxNesting = 64;

    private static readonly FrozenDictionary<string, GaugeRepresentation> StandardModelFields
        = new Dictionary<string, StandardModelGaugeRepresentations>
        {
            ["Q_L"] = new LeftHandedQuarkDoublet(),
            ["u_R"] = new RightHandedUpQuark(),
            ["d_R"] = new RightHandedDownQuark(),
            ["L_L"] = new LeftHandedLeptonDoublet(),
            ["e_R"] = new RightHandedChargedLepton(),
            ["H"] = new Higgs(),
        }
            .ToFrozenDictionary(field => field.Key, field => field.Value.Representation, StringComparer.Ordinal);

    private static readonly Func<string, Node> ParseExpression = CreateParser();

    private readonly Node _root;

    private GaugeExpression(Node root) => _root = root;

    /// <summary>
    /// Parses a product of named fields. Parentheses group products and
    /// <c>conj(expression)</c> or <c>singlet(expression)</c> apply to their operands.
    /// The parser accepts up to 64 nested pairs of parentheses.
    /// </summary>
    /// <exception cref="FormatException">The expression is malformed.</exception>
    public static GaugeExpression Parse(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return new GaugeExpression(ParseExpression(expression));
    }

    /// <summary>
    /// Analyzes the parsed expression. A supplied field map replaces the Standard Model defaults.
    /// </summary>
    /// <exception cref="FormatException">The expression refers to an unknown field.</exception>
    /// <exception cref="InvalidOperationException">A singlet selection has zero or multiple channels.</exception>
    public GaugeProduct Analyze(IReadOnlyDictionary<string, GaugeRepresentation>? fields = null)
        => AnalyzeNode(_root, fields ?? StandardModelFields, conjugated: false);

    private static GaugeProduct AnalyzeNode(Node expression, IReadOnlyDictionary<string, GaugeRepresentation> fields, bool conjugated)
    {
        var factors = new List<GaugeRepresentation>();
        CollectFactors(expression, fields, factors, conjugated);
        return GaugeProduct.Analyze([.. factors]);
    }

    private static void CollectFactors(
        Node expression,
        IReadOnlyDictionary<string, GaugeRepresentation> fields,
        List<GaugeRepresentation> factors,
        bool conjugated)
    {
        switch (expression)
        {
            case Field(var name):
                if (!fields.TryGetValue(name, out var representation) || representation is null)
                {
                    throw new FormatException($"Unknown gauge field '{name}'.");
                }

                factors.Add(conjugated ? representation.Conjugate() : representation);
                break;

            case Product(var terms):
                foreach (var term in terms)
                {
                    CollectFactors(term, fields, factors, conjugated);
                }

                break;

            case Conjugate(var operand):
                CollectFactors(operand, fields, factors, !conjugated);
                break;

            case SingletSelection(var operand):
                factors.Add(AnalyzeNode(operand, fields, conjugated).SelectUniqueSinglet());
                break;

            default:
                throw new UnreachableException();
        }
    }

    private static Func<string, Node> CreateParser()
    {
        var identifierStart = AsciiLetter.Or(CharP('_'));
        var field = Many1Chars(identifierStart, identifierStart.Or(Digit)).AndL(Spaces)
            .Map(name => (Node)new Field(name));
        var open = Skip('(').AndL(Spaces);
        var close = Skip(')').AndL(Spaces);
        var multiply = Skip('*').AndL(Spaces);
        var conjugate = Try(Skip("conj").AndL(Spaces).AndL(open));
        var singlet = Try(Skip("singlet").AndL(Spaces).AndL(open));

        // Build the parser from the innermost level out to bound recursion during parsing and evaluation.
        var product = Many1(field, multiply).Map(terms => (Node)new Product(terms));
        for (var depth = 0; depth < MaxNesting; depth++)
        {
            var term = Choice(
                conjugate.AndR(product).AndL(close).Map(operand => (Node)new Conjugate(operand)),
                singlet.AndR(product).AndL(close).Map(operand => (Node)new SingletSelection(operand)),
                open.AndR(product).AndL(close),
                field);
            product = Many1(term, multiply).Map(terms => (Node)new Product(terms));
        }

        var expression = Spaces.AndR(product).AndL(EOF);
        return input => expression.Run(input).GetResult(error => throw new FormatException(error));
    }

    private abstract record Node;

    private sealed record Field(string Name) : Node;

    private sealed record Product(IEnumerable<Node> Terms) : Node;

    private sealed record Conjugate(Node Operand) : Node;

    private sealed record SingletSelection(Node Operand) : Node;
}
