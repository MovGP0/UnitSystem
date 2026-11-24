using System.Numerics;
using UnitSystem.Orientations;
using UnitSystem.Quantities;
using UnitSystem.Systems;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestFixture]
[TestOf(typeof(VectorQuantity))]
public sealed class VectorQuantityTests
{
    [Test]
    public void ShouldConvertAndSubtractCompatibleComponents()
    {
        var centimetres = SISystem.System.Parse("cm");
        var first = new VectorQuantity(3, 1, 0, m);
        var second = new VectorQuantity(100, 300, 50, centimetres);

        var result = (first - second).Convert(centimetres);

        (result.X, result.Y, result.Z).ShouldBe((200d, -200d, -50d));
        result.Unit.ShouldBe(centimetres);
        Should.Throw<InvalidOperationException>(() => first.Convert(s));
    }

    [Test]
    public void ShouldConvertAffineUnitsAsDifferences()
    {
        var vector = new VectorQuantity(2, -4, 0, degC);

        var converted = vector.Convert(K);

        (converted.X, converted.Y, converted.Z).ShouldBe((2d, -4d, 0d));
        converted.Unit.ShouldBe(K);
        vector.Dot(vector).ShouldBe(new ScalarQuantity(20, K * K, isPoint: false));
    }

    [Test]
    public void ShouldDistinguishWorkFromSignedTorque()
    {
        var displacement = new VectorQuantity(200, 0, 0, SISystem.System.Parse("cm"));
        var force = new VectorQuantity(4, 5, 0, N);

        displacement.Dot(force).ShouldBe(new ScalarQuantity(8, J));

        var torque = displacement.Cross(force);
        (torque.X, torque.Y, torque.Z).ShouldBe((0d, 0d, 10d));
        torque.Unit.ShouldBe(m * N);
        force.Cross(displacement).Z.ShouldBe(-10);
    }

    [Test]
    public void ShouldPreserveDotProductAndRotateCrossProduct()
    {
        var first = new VectorQuantity(3, 5, 7, m);
        var second = new VectorQuantity(2, -3, 11, N);
        var rotation = Rotation3D.FromAxisAngle(1, 2, -3, 0.85);
        var rotatedFirst = first.Rotate(rotation);
        var rotatedSecond = second.Rotate(rotation);

        var dot = rotatedFirst.Dot(rotatedSecond);
        dot.Amount.ShouldBe(first.Dot(second).Amount, 1e-12);
        dot.Unit.ShouldBe(J);

        var cross = rotatedFirst.Cross(rotatedSecond);
        var expected = first.Cross(second).Rotate(rotation);
        cross.X.ShouldBe(expected.X, 1e-12);
        cross.Y.ShouldBe(expected.Y, 1e-12);
        cross.Z.ShouldBe(expected.Z, 1e-12);
        cross.Unit.ShouldBe(expected.Unit);
    }

    [Test]
    public void ShouldNormalizeQuaternionAndKeepVectorUnit()
    {
        var vector = new VectorQuantity(5, 0, 0, m);
        var rotation = Rotation3D.FromQuaternion(0, 0, 1e250, 1e250);

        var rotated = vector.Rotate(rotation);
        var interop = vector.Rotate(Rotation3D.FromQuaternion(new Quaternion(0, 0, 1, 1)));

        rotated.X.ShouldBe(0, 1e-12);
        rotated.Y.ShouldBe(5, 1e-12);
        rotated.Z.ShouldBe(0, 1e-12);
        rotated.Unit.ShouldBe(m);
        (rotated.X, rotated.Y, rotated.Z).ShouldBe((interop.X, interop.Y, interop.Z));
        vector.Rotate(Rotation3D.Identity).X.ShouldBe(vector.X);
    }

    [Test]
    public void ShouldRotateLargeComponentsWithoutIntermediateOverflow()
    {
        var vector = new VectorQuantity(double.MaxValue, 0, 0, m);

        var rotated = vector.Rotate(Rotation3D.FromQuaternion(0, 0, 1, 0));

        rotated.X.ShouldBe(-double.MaxValue);
        rotated.Y.ShouldBe(0);
        rotated.Z.ShouldBe(0);
    }

    [Test]
    public void ShouldRejectInvalidRotationInputs()
    {
        Should.Throw<ArgumentException>(() => Rotation3D.FromQuaternion(0, 0, 0, 0));
        Should.Throw<ArgumentException>(() => Rotation3D.FromQuaternion(double.NaN, 0, 0, 1));
        Should.Throw<ArgumentException>(() => Rotation3D.FromAxisAngle(0, 0, 0, 1));
        Should.Throw<ArgumentOutOfRangeException>(() => Rotation3D.FromAxisAngle(0, 0, 1, double.PositiveInfinity));
    }
}
