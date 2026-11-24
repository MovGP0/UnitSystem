using System.Globalization;
using UnitSystem.Prefixes;

namespace UnitSystem.UnitTests.Prefixes;

[TestFixture]
[TestOf(typeof(EngineeringNotation))]
public sealed class EngineeringNotationTests
{
    [TestCase(0.0, "0")]
    [TestCase(double.NaN, "undefined")]
    [TestCase(double.PositiveInfinity, "inf")]
    [TestCase(double.NegativeInfinity, "-inf")]
    public void ShouldFormatSpecialValuesWithoutAPrefix(double value, string expected)
    {
        var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        format.NaNSymbol = "undefined";
        format.PositiveInfinitySymbol = "inf";
        format.NegativeInfinitySymbol = "-inf";

        EngineeringNotation.FromValue(value, format).ShouldBe(expected);
    }

    [TestCase(1e-18, "1 a")]
    [TestCase(1e-17, "10 a")]
    [TestCase(1e-16, "100 a")]
    [TestCase(1e-15, "1 f")]
    [TestCase(1e-14, "10 f")]
    [TestCase(1e-13, "100 f")]
    [TestCase(1e-12, "1 p")]
    [TestCase(1e-11, "10 p")]
    [TestCase(1e-10, "100 p")]
    [TestCase(1e-9, "1 n")]
    [TestCase(1e-6, "1 µ")]
    [TestCase(0.001, "1 m")]
    [TestCase(0.01, "1 c")]
    [TestCase(0.1, "1 d")]
    [TestCase(1, "1")]
    [TestCase(10, "1 da")]
    [TestCase(100, "1 h")]
    [TestCase(1e3, "1 k")]
    [TestCase(1e4, "10 k")]
    [TestCase(1e5, "100 k")]
    [TestCase(1e6, "1 M")]
    [TestCase(1e7, "10 M")]
    [TestCase(1e8, "100 M")]
    [TestCase(1e9, "1 G")]
    [TestCase(1e10, "10 G")]
    [TestCase(1e11, "100 G")]
    [TestCase(1e12, "1 T")]
    [TestCase(1e13, "10 T")]
    [TestCase(1e14, "100 T")]
    [TestCase(1e15, "1 P")]
    [TestCase(1e16, "10 P")]
    [TestCase(1e17, "100 P")]
    [TestCase(1e18, "1 E")]
    [TestCase(1e19, "10 E")]
    public void ShouldConvertValueToString(double value, string expected)
    {
        expected = expected.Replace(' ', (char)0xA0);

        // Act
        var result = EngineeringNotation.FromValue(value);

        // Assert
        result.ShouldBe(expected);
    }
}
