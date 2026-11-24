using Shouldly;
using UnitTests.Shared;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(Unit))]
public static class UnitsTests
{
    [TestFixture]
    [TestOf(typeof(Unit), ILObjectNames.Multiply)]
    public sealed class MultiplyTests
    {
        [Test]
        public void ShouldMultiplyUnitsCorrectly()
        {
            // Arrange
            var result = m * s;

            // Act & Assert
            result.ShouldNotBeNull();
            result.Dimension.Count.ShouldBe(Math.Max(m.Dimension.Count, s.Dimension.Count));
        }

        [Test]
        public void ShouldRevertMultiplicationWithDivision()
        {
            // Arrange
            var mPerS = m / s;

            // Act
            var result = mPerS * s;

            // Assert
            result.ShouldBe(m);
        }

        [Test]
        public void ShouldReturnKnownUnitsWhenPossible()
        {
            // Arrange
            var result = kg * m * s.Pow(-2);

            // Act & Assert
            ReferenceEquals(result, N).ShouldBeTrue();
        }
    }

    [TestFixture]
    [TestOf(typeof(Unit), ILObjectNames.Division)]
    public sealed class DivisionTests
    {
        [Test]
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
