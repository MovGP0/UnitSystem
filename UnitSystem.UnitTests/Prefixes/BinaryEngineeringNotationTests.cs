using Shouldly;
using UnitSystem.Prefixes;

namespace UnitSystem.UnitTests.Prefixes;

[TestClass]
[TestOf(typeof(BinaryEngineeringNotation))]
public sealed class BinaryEngineeringNotationTests
{
    [TestMethod]
    [DataRow(5, "5")]
    [DataRow(1024.0, "1 Ki")]
    [DataRow(1048576.0, "1 Mi")]
    [DataRow(1073741824.0, "1 Gi")]
    [DataRow(1099511627776.0, "1 Ti")]
    [DataRow(1125899906842624.0, "1 Pi")]
    [DataRow(1152921504606846976.0, "1 Ei")]
    public void ShouldConvertValueToString(double value, string expected)
    {
        expected = expected.Replace(' ', (char)0xA0);

        // Act
        var result = BinaryEngineeringNotation.FromValue(value);

        // Assert
        result.ShouldBe(expected);
    }
}
