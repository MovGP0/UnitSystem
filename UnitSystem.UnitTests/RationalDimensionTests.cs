using ShInUeXx.Numerics;
using Shouldly;
using UnitSystem.Dimensions;
using UnitSystem.Quantities;
using UnitSystem.Systems;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestFixture]
[TestOf(typeof(Dimension))]
public sealed class RationalDimensionTests
{
    [Test]
    public void ShouldRecoverTheOriginalUnitWhenFractionalPowersCombine()
    {
        var third = m.Pow(new Rational(1, 3));
        var twoThirds = m.Pow(new Rational(2, 3));

        (third * twoThirds).ShouldBeSameAs(m);
        third.Pow(3).ShouldBeSameAs(m);
        var equivalent = m.Pow(new Rational(2, 6));
        third.ShouldBe(equivalent);
        third.Dimension.GetHashCode().ShouldBe(equivalent.Dimension.GetHashCode());
        third.ShouldNotBe(m.Pow(0.33333333f));
    }

    [Test]
    public void ShouldPreserveExponentsAcrossDifferentBaseDimensions()
    {
        var dimension = m.Pow(new Rational(1, 3)).Dimension / s.Pow(new Rational(2, 3)).Dimension;

        dimension.ShouldBe(new Dimension(new Rational(1, 3), Rational.Zero, new Rational(-2, 3)));
        dimension.ToString(SISystem.System, null, null).ShouldBe("L^1/3\u00A0T^-2/3");
    }

    [Test]
    public void ShouldDisplayAndParseFractionalExponents()
    {
        var unit = m.Pow(new Rational(1, 3)) / s.Pow(new Rational(2, 3));
        var expression = SISystem.System.Display(unit);

        expression.ShouldBe("m^(1/3) / s^(2/3)");
        SISystem.System.Parse(expression).ShouldBe(unit);
        SISystem.System.Parse("@unit 1 0 1/3").ShouldBe(m.Pow(new Rational(1, 3)));
    }

    [Test]
    public void ShouldInterpretDecimalAndScientificExponentsExactly()
    {
        SISystem.System.Parse("m^0.1").ShouldBe(m.Pow(0.1f));
        SISystem.System.Parse("m^1e-2").ShouldBe(m.Pow(new Rational(1, 100)));
        SISystem.System.Parse("m^-.5e-1").ShouldBe(m.Pow(new Rational(-1, 20)));
        SISystem.System.Parse("@unit 1 0 0.1").ShouldBe(m.Pow(new Rational(1, 10)));
    }

    [Test]
    public void ShouldRaiseQuantitiesToRationalPowers()
    {
        var quantity = new ScalarQuantity(8, m).Pow(new Rational(1, 3));

        quantity.Amount.ShouldBe(2.0, 1e-12);
        quantity.Unit.ShouldBe(m.Pow(new Rational(1, 3)));
        new ScalarQuantity(8, m).Pow(0.1f).Unit.ShouldBe(m.Pow(new Rational(1, 10)));
    }

    [Test]
    public void ShouldRejectNonfiniteExponents()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => new Dimension(default(Rational)));
        Should.Throw<ArgumentOutOfRangeException>(() => m.Pow(default(Rational)));
        Should.Throw<FormatException>(() => SISystem.System.Parse("m^(1/0)"));
        Should.Throw<FormatException>(() => SISystem.System.Parse("m^(1/3"));
    }

    [Test]
    public void ShouldRejectOversizedScientificExponents()
    {
        Should.Throw<FormatException>(() => SISystem.System.Parse("m^1e100000000"));
        Should.Throw<FormatException>(() => SISystem.System.Parse("m^1e-100000000"));
    }
}
