using UnitSystem.Prefixes;

namespace UnitSystem.UnitTests.Prefixes;

[TestFixture]
[TestOf(typeof(BinaryEngineeringNotation))]
public sealed class BinaryEngineeringNotationTests
{
    [Test]
    public void ShouldFormatConcurrently()
    {
        Parallel.For(0, 128, index =>
        {
            var exponent = index % 11;
            var prefix = BinaryPrefix.GetAll().Single(candidate => candidate.Base1024Exponent == exponent);
            var expected = prefix.Symbol.Length == 0 ? "1" : $"1\u00A0{prefix.Symbol}";

            BinaryEngineeringNotation.FromValue(prefix.Factor).ShouldBe(expected);
        });
    }

    [TestCase(5, "5")]
    [TestCase(1024.0, "1 Ki")]
    [TestCase(1048576.0, "1 Mi")]
    [TestCase(1073741824.0, "1 Gi")]
    [TestCase(1099511627776.0, "1 Ti")]
    [TestCase(1125899906842624.0, "1 Pi")]
    [TestCase(1152921504606846976.0, "1 Ei")]
    public void ShouldConvertValueToString(double value, string expected)
    {
        expected = expected.Replace(' ', (char)0xA0);

        // Act
        var result = BinaryEngineeringNotation.FromValue(value);

        // Assert
        result.ShouldBe(expected);
    }
}
