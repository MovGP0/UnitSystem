using Shouldly;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(Unit))]
public static class UnitsTests
{
    [TestClass]
    [TestOf(typeof(Unit), ILObjectNames.op_Multiply)]
    public sealed class MultiplyTests
    {
        [TestMethod]
        public void ShouldMultiplyUnitsCorrectly()
        {
            // Arrange
            var result = m * s;

            // Act & Assert
            result.ShouldNotBeNull();
            result.Dimension.Count.ShouldBe(Math.Max(m.Dimension.Count, s.Dimension.Count));
        }

        [TestMethod]
        public void ShouldRevertMultiplicationWithDivision()
        {
            // Arrange
            var mPerS = m / s;

            // Act
            var result = mPerS * s;

            // Assert
            result.ShouldBe(m);
        }

        [TestMethod]
        public void ShouldReturnKnownUnitsWhenPossible()
        {
            // Arrange
            var result = kg * m * (s ^ -2);

            // Act & Assert
            ReferenceEquals(result, N).ShouldBeTrue();
        }
    }

    [TestClass]
    [TestOf(typeof(Unit), ILObjectNames.op_Division)]
    public sealed class DivisionTests
    {
        [TestMethod]
        public void ShouldDivideUnitsCorrectly()
        {
            // Arrange
            var result = m / s;

            // Act & Assert
            result.ShouldNotBeNull();
            result.Dimension.Count.ShouldBe(Math.Max(m.Dimension.Count, s.Dimension.Count));
        }
    }
}
