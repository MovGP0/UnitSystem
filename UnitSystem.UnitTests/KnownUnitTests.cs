using Shouldly;
using UnitSystem.Dimensions;
using UnitSystem.Systems;
using UnitTests.Shared;

namespace UnitSystem.UnitTests;

[TestOf(typeof(KnownUnit))]
public static class KnownUnitTests
{
    [TestClass]
    [TestOf(typeof(KnownUnit), nameof(KnownUnit.Equals))]
    public sealed class EqualsTests
    {
        [TestMethod]
        public void ShouldBeEqualWhenSameDimensionAndFactor()
        {
            // Arrange
            var system = SISystem.System;
            var unit1 = new KnownUnit(system, 1, 0, new Dimension(1), "x", "x");
            var unit2 = new KnownUnit(system, 1, 0, new Dimension(1), "x", "x");
            var unit3 = new KnownUnit(system, 3, 0, new Dimension(0, 0, 1), "x", "x");

            // Act & Assert
            unit1.ShouldBe(unit2);
            unit2.ShouldNotBe(unit3);
        }
    }
}
