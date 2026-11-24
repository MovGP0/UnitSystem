using System.Numerics;
using ShInUeXx.Numerics;
using Shouldly;
using UnitSystem.Gauge;

namespace UnitSystem.UnitTests;

[TestClass]
[TestOf(typeof(GaugeExpression))]
public sealed class GaugeExpressionTests
{
    [TestMethod]
    public void ShouldParseAndSelectTheDownTypeYukawaSinglet()
    {
        var product = GaugeExpression.Parse("  conj (Q_L) * H\n* d_R  ").Analyze();
        var selected = GaugeExpression.Parse("singlet(conj(Q_L) * H * d_R)").Analyze();

        product.Hypercharge.ShouldBe(Rational.Zero);
        product.SingletMultiplicity.ShouldBe(BigInteger.One);
        product.IsGaugeInvariant.ShouldBeFalse();
        selected.IsGaugeInvariant.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldParseAndAnalyzeTextInOneCall()
    {
        GaugeExpression.Analyze("singlet(conj(H) * H)").IsGaugeInvariant.ShouldBeTrue();

        var fields = new Dictionary<string, GaugeRepresentation> { ["S"] = GaugeRepresentation.Singlet };
        GaugeExpression.Analyze("S", fields).IsGaugeInvariant.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldConjugateAGroupedProduct()
    {
        var result = GaugeExpression.Parse("conj(Q_L * conj(H)) * (d_R)").Analyze();
        var downYukawa = GaugeExpression.Parse("conj(Q_L) * H * d_R").Analyze();

        result.Hypercharge.ShouldBe(Rational.Zero);
        result.Channels.Count.ShouldBe(downYukawa.Channels.Count);
        foreach (var (channel, multiplicity) in downYukawa.Channels)
        {
            result.Channels[channel].ShouldBe(multiplicity);
        }
    }

    [TestMethod]
    public void ShouldSelectEachSubexpressionIndependently()
    {
        var product = GaugeExpression.Parse("(conj(H) * H) * (conj(H) * H)").Analyze();
        var selected = GaugeExpression.Parse("singlet(conj(H) * H) * singlet(conj(H) * H)").Analyze();
        var nested = GaugeExpression.Parse("conj(singlet(singlet(conj(H) * H) * conj(H) * H))").Analyze();

        product.SingletMultiplicity.ShouldBe(new BigInteger(2));
        selected.IsGaugeInvariant.ShouldBeTrue();
        selected.SingletMultiplicity.ShouldBe(BigInteger.One);
        nested.IsGaugeInvariant.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldCheckTheSingletOfTheSelectedSubexpression()
    {
        GaugeExpression.Parse("H * conj(H)").Analyze().HasSinglet.ShouldBeTrue();
        Should.Throw<InvalidOperationException>(() => GaugeExpression.Parse("singlet(H) * conj(H)").Analyze());
    }

    [TestMethod]
    public void ShouldBoundParenthesisNesting()
    {
        var withinLimit = new string('(', 64) + "H" + new string(')', 64);
        GaugeExpression.Parse(withinLimit).Analyze().Hypercharge.ShouldBe(new Rational(1, 2));

        Should.Throw<FormatException>(() => GaugeExpression.Parse("(" + withinLimit + ")"));
    }

    [TestMethod]
    public void ShouldReportAnUnknownField()
    {
        Should.Throw<FormatException>(() => GaugeExpression.Parse("H * missing").Analyze()).Message.ShouldContain("missing");
    }

    [TestMethod]
    [DataRow("H *")]
    [DataRow("H H")]
    [DataRow("H + H")]
    [DataRow("")]
    [DataRow("conj(H")]
    [DataRow("conj()")]
    [DataRow("singlet()")]
    [DataRow("(H * )")]
    [DataRow("singlet(conj(H) * H) extra")]
    public void ShouldRejectMalformedExpressions(string expression)
    {
        Should.Throw<FormatException>(() => GaugeExpression.Parse(expression));
    }

    [TestMethod]
    public void ShouldRejectAnAmbiguousOrMissingSingletSelection()
    {
        Should.Throw<InvalidOperationException>(() => GaugeExpression.Parse("singlet(conj(H) * H * conj(H) * H)").Analyze());
        Should.Throw<InvalidOperationException>(() => GaugeExpression.Parse("singlet(H)").Analyze());
    }

    [TestMethod]
    public void ShouldAnalyzeOneParsedExpressionAgainstDifferentFieldMaps()
    {
        var expression = GaugeExpression.Parse("field * field");
        var neutral = new Dictionary<string, GaugeRepresentation> { ["field"] = GaugeRepresentation.Singlet };
        var charged = new Dictionary<string, GaugeRepresentation>
        {
            ["field"] = new GaugeRepresentation(ColorRepresentation.Singlet, 0, new Rational(1, 6)),
        };

        expression.Analyze(neutral).IsGaugeInvariant.ShouldBeTrue();

        var chargedProduct = expression.Analyze(charged);
        chargedProduct.Hypercharge.ShouldBe(new Rational(1, 3));
        chargedProduct.HasSinglet.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldResolveCustomFieldNamesThatStartWithKeywords()
    {
        var fields = new Dictionary<string, GaugeRepresentation>
        {
            ["conjugate"] = GaugeRepresentation.Singlet,
            ["singlet_field"] = GaugeRepresentation.Singlet,
        };

        GaugeExpression.Parse("conjugate * singlet_field").Analyze(fields).IsGaugeInvariant.ShouldBeTrue();
    }
}
