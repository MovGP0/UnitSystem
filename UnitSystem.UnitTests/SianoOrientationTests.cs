using System.Numerics;
using UnitSystem.Orientations;
using static UnitSystem.Orientations.SianoOrientation;

namespace UnitSystem.UnitTests;

[TestFixture]
[TestOf(typeof(SianoOrientation))]
public sealed class SianoOrientationTests
{
    [Test]
    public void ShouldUseTheFourCoordinateBasisVectors()
    {
        X.ShouldBe(new SianoOrientation(1, 0, 0, 0));
        Y.ShouldBe(new SianoOrientation(0, 1, 0, 0));
        Z.ShouldBe(new SianoOrientation(0, 0, 1, 0));
        Orientationless.ShouldBe(new SianoOrientation(0, 0, 0, 1));
    }

    [Test]
    public void ShouldRepresentACombinationOfAxes()
    {
        var combination = (3 * X) + (Y * 4);

        combination.ShouldBe(new SianoOrientation(3, 4, 0, 0));
        combination.IsBasisElement.ShouldBeFalse();
        (combination * Orientationless).ShouldBe(combination);
        (combination * Zero).ShouldBe(Zero);
    }

    [Test]
    public void ShouldRotateComplexSpatialCoefficientsAndKeepTheScalar()
    {
        var orientation = new SianoOrientation(new Complex(2, 3), new Complex(5, 7), new Complex(11, 13), new Complex(17, 19));
        var rotation = Rotation3D.FromAxisAngle(0, 0, 1, Math.PI / 2);

        var rotated = orientation.Rotate(rotation);

        (rotated.XCoefficient + orientation.YCoefficient).Magnitude.ShouldBe(0, 1e-12);
        (rotated.YCoefficient - orientation.XCoefficient).Magnitude.ShouldBe(0, 1e-12);
        (rotated.ZCoefficient - orientation.ZCoefficient).Magnitude.ShouldBe(0, 1e-12);
        rotated.ScalarCoefficient.ShouldBe(orientation.ScalarCoefficient);
    }

    [Test]
    public void ShouldMultiplyComplexCoefficientsBilinearly()
    {
        var combination = (Complex.ImaginaryOne * X) + Y;

        (combination * combination).ShouldBe(new SianoOrientation(0, 0, 2 * Complex.ImaginaryOne, 0));
    }
}
