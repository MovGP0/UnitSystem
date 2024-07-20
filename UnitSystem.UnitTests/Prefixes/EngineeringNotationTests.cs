using Shouldly;
using UnitSystem.Prefixes;
using UnitTests.Shared;

namespace UnitSystem.UnitTests.Prefixes;

[TestClass]
[TestOf(typeof(EngineeringNotation))]
public sealed class EngineeringNotationTests
{
    [TestMethod]
    [DataRow(1e-18, "1 a")]
    [DataRow(1e-17, "10 a")]
    [DataRow(1e-16, "100 a")]
    [DataRow(1e-15, "1 f")]
    [DataRow(1e-14, "10 f")]
    [DataRow(1e-13, "100 f")]
    [DataRow(1e-12, "1 p")]
    [DataRow(1e-11, "10 p")]
    [DataRow(1e-10, "100 p")]
    [DataRow(1e-9, "1 n")]
    [DataRow(1e-6, "1 µ")]
    [DataRow(0.001, "1 m")]
    [DataRow(0.01, "1 c")]
    [DataRow(0.1, "1 d")]
    [DataRow(1, "1")]
    [DataRow(10, "1 da")]
    [DataRow(100, "1 h")]
    [DataRow(1e3, "1 k")]
    [DataRow(1e4, "10 k")]
    [DataRow(1e5, "100 k")]
    [DataRow(1e6, "1 M")]
    [DataRow(1e7, "10 M")]
    [DataRow(1e8, "100 M")]
    [DataRow(1e9, "1 G")]
    [DataRow(1e10, "10 G")]
    [DataRow(1e11, "100 G")]
    [DataRow(1e12, "1 T")]
    [DataRow(1e13, "10 T")]
    [DataRow(1e14, "100 T")]
    [DataRow(1e15, "1 P")]
    [DataRow(1e16, "10 P")]
    [DataRow(1e17, "100 P")]
    [DataRow(1e18, "1 E")]
    [DataRow(1e19, "10 E")]
    public void ShouldConvertValueToString(double value, string expected)
    {
        expected = expected.Replace(' ', (char)0xA0);

        // Act
        var result = EngineeringNotation.FromValue(value);

        // Assert
        result.ShouldBe(expected);
    }
}
