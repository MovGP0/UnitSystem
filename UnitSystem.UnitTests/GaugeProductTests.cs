using System.Numerics;
using ShInUeXx.Numerics;
using Shouldly;
using UnitSystem.Gauge;
using static UnitSystem.Gauge.StandardModelGaugeRepresentations;

namespace UnitSystem.UnitTests;

[TestFixture]
[TestOf(typeof(GaugeProduct))]
public sealed class GaugeProductTests
{
    [Test]
    public void ShouldProjectEveryStandardModelMultipletToItsGaugeRepresentation()
    {
        (StandardModelGaugeRepresentations Multiplet, GaugeRepresentation Representation)[] cases =
        [
            (new LeftHandedQuarkDoublet(), QuarkDoublet),
            (new RightHandedUpQuark(), UpQuark),
            (new RightHandedDownQuark(), DownQuark),
            (new LeftHandedLeptonDoublet(), LeptonDoublet),
            (new RightHandedChargedLepton(), ChargedLepton),
            (new Higgs(), HiggsDoublet),
        ];

        foreach (var (multiplet, representation) in cases)
        {
            multiplet.Representation.ShouldBeSameAs(representation);
        }

        Should.Throw<InvalidOperationException>(() => default(StandardModelGaugeRepresentations).Representation);
    }

    [Test]
    public void ShouldDistinguishAnAvailableSingletFromAnInvariantProduct()
    {
        var product = GaugeProduct.Analyze(UpQuark, UpQuark.Conjugate());

        product.Channels.Count.ShouldBe(2);
        product.Channels[new GaugeRepresentation(ColorRepresentation.Octet, 0, Rational.Zero)].ShouldBe(BigInteger.One);
        product.SingletMultiplicity.ShouldBe(BigInteger.One);
        product.HasSinglet.ShouldBeTrue();
        product.IsGaugeInvariant.ShouldBeFalse();
        product.SelectUniqueSinglet().ShouldBe(GaugeRepresentation.Singlet);
    }

    [Test]
    public void ShouldDecomposeTheWeakDoubletProduct()
    {
        var product = GaugeProduct.Analyze(HiggsDoublet.Conjugate(), HiggsDoublet);

        product.Channels.Count.ShouldBe(2);
        product.Channels[new GaugeRepresentation(ColorRepresentation.Singlet, 2, Rational.Zero)].ShouldBe(BigInteger.One);
        product.SingletMultiplicity.ShouldBe(BigInteger.One);
    }

    [Test]
    public void ShouldPreserveTheLargestWeakIsospinLabelWhenMultiplyingByASinglet()
    {
        var representation = new GaugeRepresentation(ColorRepresentation.Singlet, uint.MaxValue, Rational.Zero);

        var product = GaugeProduct.Analyze(GaugeRepresentation.Singlet, representation);

        product.Channels.Count.ShouldBe(1);
        product.Channels[representation].ShouldBe(BigInteger.One);
    }

    [Test]
    public void ShouldThrowWhenTheLargestWeakChannelExceedsTheLabelRange()
    {
        var representation = new GaugeRepresentation(ColorRepresentation.Singlet, uint.MaxValue, Rational.Zero);

        Should.Throw<OverflowException>(() => GaugeProduct.Analyze(representation, HiggsDoublet));
    }

    [Test]
    public void ShouldUseExactHyperchargeForYukawaTerms()
    {
        var down = GaugeProduct.Analyze(QuarkDoublet.Conjugate(), HiggsDoublet, DownQuark);
        var up = GaugeProduct.Analyze(QuarkDoublet.Conjugate(), HiggsDoublet.Conjugate(), UpQuark);
        var wrongHiggs = GaugeProduct.Analyze(QuarkDoublet.Conjugate(), HiggsDoublet, UpQuark);

        down.Hypercharge.ShouldBe(Rational.Zero);
        down.SingletMultiplicity.ShouldBe(BigInteger.One);
        up.SingletMultiplicity.ShouldBe(BigInteger.One);
        wrongHiggs.Hypercharge.ShouldBe(Rational.One);
        wrongHiggs.HasSinglet.ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() => wrongHiggs.SelectUniqueSinglet());
    }

    [Test]
    public void ShouldRejectAnUnpairedWeakDoubletAndAColorTripletPair()
    {
        var triplet = new GaugeRepresentation(ColorRepresentation.Triplet, 0, Rational.Zero);
        var weakDoublet = new GaugeRepresentation(ColorRepresentation.Singlet, 1, Rational.Zero);

        GaugeProduct.Analyze(triplet, triplet).HasSinglet.ShouldBeFalse();
        GaugeProduct.Analyze(weakDoublet).HasSinglet.ShouldBeFalse();
    }

    [Test]
    public void ShouldFindTheThreeTripletColorSinglet()
    {
        var product = GaugeProduct.Analyze(UpQuark, DownQuark, DownQuark);

        product.Hypercharge.ShouldBe(Rational.Zero);
        product.Channels[new GaugeRepresentation(ColorRepresentation.Octet, 0, Rational.Zero)].ShouldBe(new BigInteger(2));
        product.SingletMultiplicity.ShouldBe(BigInteger.One);
    }

    [Test]
    public void ShouldReportIndependentContractions()
    {
        var product = GaugeProduct.Analyze(
            QuarkDoublet, QuarkDoublet.Conjugate(), QuarkDoublet, QuarkDoublet.Conjugate());

        product.SingletMultiplicity.ShouldBe(new BigInteger(4));
        Should.Throw<InvalidOperationException>(() => product.SelectUniqueSinglet());
    }

    [Test]
    public void ShouldConjugateColorLabelsAcrossTheUnsignedRange()
    {
        var representation = new ColorRepresentation(uint.MaxValue, 1);

        representation.Conjugate().ShouldBe(new ColorRepresentation(1, uint.MaxValue));
    }

    [Test]
    public void ShouldDecomposeTwoColorOctets()
    {
        var octet = new GaugeRepresentation(ColorRepresentation.Octet, 0, Rational.Zero);
        var product = GaugeProduct.Analyze(octet, octet);

        product.Channels.Count.ShouldBe(5);
        product.Channels[octet].ShouldBe(new BigInteger(2));
        foreach (var color in new[] { ColorRepresentation.Singlet, new(3, 0), new(0, 3), new(2, 2) })
        {
            product.Channels[new GaugeRepresentation(color, 0, Rational.Zero)].ShouldBe(BigInteger.One);
        }
    }

    [Test]
    public void ShouldTreatTheEmptyProductAsInvariant()
    {
        var product = GaugeProduct.Analyze();

        product.IsGaugeInvariant.ShouldBeTrue();
        product.SelectUniqueSinglet().ShouldBe(GaugeRepresentation.Singlet);
    }
}
