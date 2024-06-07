using Shouldly;
using UnitSystem.Dimensions;

namespace UnitSystem.UnitTests;

[TestOf(typeof(Dimension))]
public class DimensionTests
{
    [TestClass]
    [TestOf(typeof(Dimension), ILObjectNames.Constructor)]
    public sealed class ConstructorTests
    {
        [TestMethod]
        public void ShouldDetermineEqualityBasedOnExponents()
        {
            // Arrange
            var dim1 = new Dimension(1, 2, 3);
            var dim2 = new Dimension(1, 2, 3);
            var dim3 = new Dimension(3, 2, 1);
            var dim4 = new Dimension(1);
            var dim5 = new Dimension(1);
            var dim6 = new Dimension();
            var dim7 = new Dimension();

            // Act & Assert
            dim1.ShouldBe(dim2);
            dim1.GetHashCode().ShouldBe(dim2.GetHashCode());
            dim1.ShouldNotBe(dim3);
            dim1.GetHashCode().ShouldNotBe(dim3.GetHashCode());
            dim2.ShouldNotBe(dim3);
            dim2.GetHashCode().ShouldNotBe(dim3.GetHashCode());
            dim4.ShouldBe(dim5);
            dim4.GetHashCode().ShouldBe(dim5.GetHashCode());
            dim6.ShouldBe(dim7);
            dim6.GetHashCode().ShouldBe(dim7.GetHashCode());
        }

        [TestMethod]
        public void ShouldIgnoreTrailingZeros()
        {
            // Arrange
            var dim1 = new Dimension(1, 2, 3, 0, 0);
            var dim2 = new Dimension(1, 2, 3);

            // Act & Assert
            dim1.ShouldBe(dim2);
            dim1.GetHashCode().ShouldBe(dim2.GetHashCode());
        }

        [TestMethod]
        public void ShouldNotIgnoreStartingZeros()
        {
            // Arrange
            var dim1 = new Dimension(0, 0, 1);
            var dim2 = new Dimension(1);

            // Act & Assert
            dim1.ShouldNotBe(dim2);
        }

        [TestMethod]
        public void ShouldBeDimensionlessWithEmptyList()
        {
            // Act & Assert
            new Dimension().ShouldBe(Dimension.DimensionLess);
        }

        [TestMethod]
        public void ShouldBeDimensionlessWithOnlyZeroExponents()
        {
            // Act & Assert
            new Dimension(0, 0).ShouldBe(Dimension.DimensionLess);
        }
    }
}
