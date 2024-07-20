using System.Globalization;
using Shouldly;
using UnitSystem.Prefixes;
using UnitSystem.Quantities;
using UnitSystem.Systems;
using UnitTests.Shared;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(ScalarQuantity))]
public static class ScalarQuantityTests
{
    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.Division)]
    public sealed class DivisionTests
    {
        [TestMethod]
        public void ShouldDivideCorrectly()
        {
            // Arrange
            var kWh = MetricPrefix.Kilo * W * h;
            var m3 = m ^ 3;
            var energy = new ScalarQuantity(100, kWh);
            var volume = new ScalarQuantity(5, m3);
            var expected = new ScalarQuantity(20, kWh / m3);

            // Act
            var result = energy / volume;

            // Assert
            result.ShouldBe(expected);
        }

        [TestMethod]
        public void ShouldBehaveCorrectly()
        {
            // Arrange
            var mass100 = new ScalarQuantity(100, kg);
            var mass25 = new ScalarQuantity(25, kg);

            // Act
            var result = mass100 / 4;

            // Assert
            result.ShouldBe(mass25);
        }

        [TestMethod]
        public void ShouldYieldARatioWhenDividingSameUnits()
        {
            // Arrange
            var a = new ScalarQuantity(10, m);
            var b = new ScalarQuantity(2, m);

            // Act
            var result = a / b;
            var expected = new ScalarQuantity(5, NoUnit);

            // Assert
            result.ShouldBe(expected);
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.Addition)]
    public sealed class AdditionTests
    {
        [TestMethod]
        public void ShouldAddQuantitiesCorrectly()
        {
            // Arrange
            var kWh = MetricPrefix.Kilo * W * h;
            var energy1 = new ScalarQuantity(100, kWh);
            var energy2 = new ScalarQuantity(50, kWh).Convert(J);
            var expected = new ScalarQuantity(150, kWh);

            // Act
            var result = energy1 + energy2;

            // Assert
            result.ShouldBe(expected);
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.Subtraction)]
    public sealed class SubtractionTests
    {
        [TestMethod]
        public void ShouldSubtractQuantitiesCorrectly()
        {
            // Arrange
            var kWh = MetricPrefix.Kilo * W * h;
            var energy1 = new ScalarQuantity(100, kWh);
            var energy2 = new ScalarQuantity(20, kWh).Convert(J);
            var expected = new ScalarQuantity(80, kWh);

            // Act
            var result = energy1 - energy2;

            // Assert
            result.ShouldBe(expected);
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.Power)]
    public sealed class ExponentTests
    {
        [TestMethod]
        public void ShouldExponentizeQuantitiesCorrectly()
        {
            // Arrange
            var length = new ScalarQuantity(4, m);
            var square = m * m;
            var expected = new ScalarQuantity(16, square);

            // Act
            var result = length ^ 2;

            // Assert
            result.ShouldBe(expected);
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.Inequality)]
    public sealed class InEqualityTests
    {
        [TestMethod]
        public void ShouldWorkAsExpected()
        {
            // Arrange
            var amount1 = new ScalarQuantity(0.1, kg);
            var amount2 = new ScalarQuantity(1.0 / 10, kg);

            // Act & Assert
            (amount1 != amount2).ShouldBeFalse();

            amount1 = null;
            (amount1 != amount2).ShouldBeTrue();

            amount2 = null;
            (amount1 != amount2).ShouldBeFalse();

            amount1 = new ScalarQuantity(0.1, kg);
            amount2 = amount1;
            (amount1 != amount2).ShouldBeFalse();
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.Equality)]
    public sealed class EqualityTests
    {
        [TestMethod]
        public void ShouldCheckEqualityForEquivalentQuantities()
        {
            // Arrange
            var hour1 = new ScalarQuantity(1, h);
            var hour2 = new ScalarQuantity(3600, s);

            // Act & Assert
            (hour1 == hour2).ShouldBeTrue();
            hour1.ShouldBe(hour2);
        }

        [TestMethod]
        public void ShouldWorkAsExpected()
        {
            // Arrange
            var amount1 = new ScalarQuantity(0.1, kg);
            var amount2 = new ScalarQuantity(1.0 / 10, kg);

            // Act & Assert
            (amount1 == amount2).ShouldBeTrue();

            amount1 = null;
            (amount1 == amount2).ShouldBeFalse();

            amount2 = null;
            (amount1 == amount2).ShouldBeTrue();

            amount1 = new ScalarQuantity(0.1, kg);
            amount2 = amount1;
            (amount1 == amount2).ShouldBeTrue();
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), ILObjectNames.LessThan)]
    public sealed class LessThanTests
    {
        [TestMethod]
        public void ShouldWorkAsExpected1()
        {
            // Arrange
            var kWh = SISystem.System.AddDerivedUnit("kWh", "kilowatt hour", MetricPrefix.Kilo * W * h);
            var energy1 = new ScalarQuantity(100, J).Convert(kWh);
            var energy2 = new ScalarQuantity(101, J);

            // Act & Assert
            (energy1 < energy2).ShouldBeTrue();
            (energy1 <= energy2).ShouldBeTrue();
            (energy1 > energy2).ShouldBeFalse();
            (energy1 >= energy2).ShouldBeFalse();
            (energy1 == energy2).ShouldBeFalse();
        }

        [TestMethod]
        public void ShouldWorkAsExpected2()
        {
            // Arrange
            var amount1 = new ScalarQuantity(1, kg);
            var amount2 = new ScalarQuantity(2, kg);

            // Act & Assert
            (amount1 < amount2).ShouldBeTrue();
            (amount1 > amount2).ShouldBeFalse();
            (amount1 <= amount2).ShouldBeTrue();
            (amount1 >= amount2).ShouldBeFalse();

            amount1 = new ScalarQuantity(1, kg);
            amount2 = new ScalarQuantity(1, kg);
            (amount1 < amount2).ShouldBeFalse();
            (amount1 > amount2).ShouldBeFalse();
            (amount1 <= amount2).ShouldBeTrue();
            (amount1 >= amount2).ShouldBeTrue();

            amount2 = null;
            (amount1 < amount2).ShouldBeFalse();
            (amount1 > amount2).ShouldBeTrue();
            (amount1 <= amount2).ShouldBeFalse();
            (amount1 >= amount2).ShouldBeTrue();

            amount1 = null;
            (amount1 < amount2).ShouldBeFalse();
            (amount1 > amount2).ShouldBeFalse();
            (amount1 <= amount2).ShouldBeTrue();
            (amount1 >= amount2).ShouldBeTrue();
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), nameof(ScalarQuantity.CompareTo))]
    public sealed class CompareToTests
    {
        [TestMethod]
        public void ShouldCompareToWorkAsExpected()
        {
            // Arrange
            var amount1 = new ScalarQuantity(1, kg);
            var amount2 = new ScalarQuantity(2, kg);

            // Act & Assert
            amount1.CompareTo(amount2).ShouldBe(-1);
            amount2.CompareTo(amount1).ShouldBe(1);

            amount2 = amount1;
            amount1.CompareTo(amount2).ShouldBe(0);

            amount2 = null;
            amount1.CompareTo(amount2).ShouldBe(1);
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), nameof(ScalarQuantity.Convert))]
    public sealed class ConvertTests
    {
        [TestMethod]
        public void ShouldBeEquivalentWhenConverted()
        {
            // Arrange
            var GWh = AddDerivedUnit("GWh", "gigawatt hour", MetricPrefix.Giga * W * h);
            var quantity = new ScalarQuantity(100*1000*1000, J);

            // Act
            var result = quantity.Convert(GWh);

            // Assert
            quantity.ShouldBe(result);
        }

        [TestMethod]
        public void ShouldThrowExceptionForInequivalentUnits()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J);

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => quantity.Convert(W));
        }
    }

    [TestClass]
    [TestOf(typeof(ScalarQuantity), nameof(ScalarQuantity.ToString))]
    public sealed class ToStringTests
    {
        public ToStringTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        [TestMethod]
        public void ShouldPrintQuantityInDesiredUnitAndFormat()
        {
            // Arrange
            var quantity = new ScalarQuantity(10 * 1000 * 1000, J);
            var MWh = AddDerivedUnit("MWh", "megawatt hour", MetricPrefix.Mega * W * h);

            // Act & Assert
            quantity.ToString().ShouldBe("10000000 J");
            quantity.ToString(MWh).ShouldBe("0.00277777777777778 MWh");
            quantity.ToString("N3", MWh).ShouldBe("0.003 MWh");
        }

        [TestMethod]
        public void ShouldBehaveCorrectly()
        {
            // Arrange
            var mg = AddDerivedUnit("mg", "milligram", kg / (1000 * 1000));
            var t = AddDerivedUnit("t", "tonne", 1000 * kg);
            var mass = new ScalarQuantity(10 * 1000, kg);

            // Act & Assert
            mass.ShouldBe(mass.Convert(mg));
            mass.ShouldBe(mass.Convert(t));
            mass.ToString(mg).ShouldBe("10000000000 mg");
            mass.ToString(t).ShouldBe("10 t");
        }
    }
}
