using NSubstitute;
using Shouldly;
using UnitSystem.Dimensions;
using UnitTests.Shared;

namespace UnitSystem.UnitTests;

[TestOf(typeof(UnitFactory))]
public static class UnitFactoryTests
{
    [TestClass]
    [TestOf(typeof(UnitFactory), nameof(UnitFactory.CreateUnit))]
    public sealed class CreateUnitTests
    {
        [TestMethod]
        public void ShouldNotCreateTrailingEmptyDimensions()
        {
            // Arrange
            var subject = new UnitFactory();

            // Act
            var baseUnit = subject.CreateUnit(Substitute.For<IUnitSystem>(), 1, 0, new Dimension(0, 1), "x", "x", string.Empty);

            // Assert
            baseUnit.Dimension.Count.ShouldBe(2);
        }
    }
}
