using UnitSystem.Dimensions;
using NSubstitute;
using Shouldly;

namespace UnitSystem.UnitTests;

[TestOf(typeof(UnitFactory))]
public static class UnitFactoryTests
{
    [TestFixture]
    [TestOf(typeof(UnitFactory), nameof(UnitFactory.CreateUnit))]
    public sealed class CreateUnitTests
    {
        [Test]
        public void ShouldNotCreateTrailingEmptyDimensions()
        {
            // Arrange
            var subject = new UnitFactory();
            var system = Substitute.For<IUnitSystem>();
            system.NumberOfDimensions.Returns(2);

            // Act
            var baseUnit = subject.CreateUnit(system, 1, 0, new Dimension(0, 1, 0), "x", "x", string.Empty);

            // Assert
            baseUnit.Dimension.Count.ShouldBe(2);
        }

        [Test]
        public void ShouldRejectDimensionsAbsentFromTheCatalog()
        {
            var system = UnitSystemFactory.CreateSystem("example");
            system.AddBaseUnit("m", "metre");
            var dimension = new Dimension(0, 1);
            var factory = new UnitFactory();

            Should.Throw<InvalidOperationException>(() => new KnownUnit(system, 1, 0, dimension, "x", "unknown"));
            Should.Throw<InvalidOperationException>(() =>
                factory.CreateUnit(system, 1, 0, dimension, "x", "unknown", string.Empty));
        }
    }
}
