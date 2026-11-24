using UnitSystem.Orientations;
using UnitSystem.Quantities;
using UnitSystem.Systems;
using static UnitSystem.Orientations.SianoOrientation;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestFixture]
[TestOf(typeof(OrientedQuantity))]
public sealed class OrientedQuantityTests
{
    public static IEnumerable<object[]> OrientationProducts
        => [
        [Orientationless, X, X],
        [X, X, Orientationless],
        [Y, Y, Orientationless],
        [Z, Z, Orientationless],
        [X, Y, Z],
        [Y, Z, X],
        [Z, X, Y],
    ];

    [TestCaseSource(nameof(OrientationProducts))]
    public void ShouldCombineOrientations(
        SianoOrientation first, SianoOrientation second, SianoOrientation expected)
    {
        var left = new OrientedQuantity(new ScalarQuantity(1, SISystem.NoUnit), first);
        var right = new OrientedQuantity(new ScalarQuantity(1, SISystem.NoUnit), second);

        (left * right).Orientation.ShouldBe(expected);
        (right * left).Orientation.ShouldBe(expected);
        (left / right).Orientation.ShouldBe(expected);
    }

    [Test]
    public void ShouldRequireASingleBasisElementForAQuantityComponent()
    {
        var length = new ScalarQuantity(2, m);

        Should.Throw<ArgumentOutOfRangeException>(() => new OrientedQuantity(length, X + Y));
        Should.Throw<ArgumentOutOfRangeException>(() => new OrientedQuantity(length, Zero));
    }

    [Test]
    public void ShouldDistinguishWorkFromMoment()
    {
        var xForce = new OrientedQuantity(new ScalarQuantity(5, N), X);
        var yForce = new OrientedQuantity(new ScalarQuantity(5, N), Y);
        var xDisplacement = new OrientedQuantity(new ScalarQuantity(2, m), X);

        var work = xDisplacement * xForce;
        var moment = xDisplacement * yForce;

        work.Quantity.ShouldBe(new ScalarQuantity(10, J));
        moment.Quantity.ShouldBe(work.Quantity);
        work.Orientation.ShouldBe(Orientationless);
        moment.Orientation.ShouldBe(Z);
        moment.ShouldNotBe(work);
        Should.Throw<InvalidOperationException>(() => work + moment);
    }

    [Test]
    public void ShouldRequireMatchingOrientationsForAdditionAndSubtraction()
    {
        var x = new OrientedQuantity(new ScalarQuantity(2, m), X);
        var moreX = new OrientedQuantity(new ScalarQuantity(3, m), X);
        var y = new OrientedQuantity(x.Quantity, Y);
        var xTime = new OrientedQuantity(new ScalarQuantity(2, s), X);

        (x + moreX).ShouldBe(new OrientedQuantity(new ScalarQuantity(5, m), X));
        (x - moreX).ShouldBe(new OrientedQuantity(new ScalarQuantity(-1, m), X));
        Should.Throw<InvalidOperationException>(() => x + y);
        Should.Throw<InvalidOperationException>(() => x - y);
        Should.Throw<InvalidOperationException>(() => x + xTime);
    }

    [Test]
    public void ShouldRetainOrientationDuringUnitConversion()
    {
        var length = new OrientedQuantity(new ScalarQuantity(2, m), Y);
        var centimetre = SISystem.System.Parse("cm");

        length.Convert(centimetre).ShouldBe(new OrientedQuantity(new ScalarQuantity(200, centimetre), Y));
        length.Convert(centimetre).Quantity.Amount.ShouldBe(200);
        length.Convert(centimetre).Quantity.Unit.ShouldBe(centimetre);
    }

    [Test]
    public void ShouldApplyParityToIntegerPowers()
    {
        var length = new OrientedQuantity(new ScalarQuantity(-2, m), Z);

        length.Pow(-1).ShouldBe(new OrientedQuantity(new ScalarQuantity(-0.5, m.Pow(-1)), Z));
        length.Pow(0).ShouldBe(new OrientedQuantity(new ScalarQuantity(1, SISystem.NoUnit), Orientationless));
        length.Pow(2).ShouldBe(new OrientedQuantity(new ScalarQuantity(4, m.Pow(2)), Orientationless));
    }
}
