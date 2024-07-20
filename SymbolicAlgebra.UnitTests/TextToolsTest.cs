namespace SymbolicAlgebra.UnitTests;

[TestOf(typeof(TextTools))]
public static class TextToolsTest
{
    [TestClass]
    [TestOf(typeof(TextTools), nameof(TextTools.ComaSplit))]
    public sealed class ComaSplitTests
    {
        /// <summary>
        /// A test for ComaSplit
        ///</summary>
        [TestMethod()]
        public void ComaSplitTest()
        {
            // arrange
            const string expression = "r, t,  sun ( e, f, g(4,5),2),o,p";

            // act
            var result = TextTools.ComaSplit(expression);

            // assert
            result.ShouldSatisfyAllConditions(
                f => f[0].ShouldBe("r"),
                f => f[1].ShouldBe("t"),
                f => f[2].ShouldBe("sun(e,f,g(4,5),2)"),
                f => f[3].ShouldBe("o"),
                f => f[4].ShouldBe("p"));
        }
    }
}