using Shouldly;
using UnitSystem.Prefixes;
using UnitSystem.Systems;
using UnitTests.Shared;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(SISystem))]
public static class SISystemTests
{
    [TestOf(typeof(SISystem), nameof(BaseUnits))]
    public sealed class BaseUnitsTests
    {
        [TestMethod]
        public void ShouldDefineAll7BaseUnits()
        {
            // Arrange & Act & Assert
            BaseUnits.Count().ShouldBe(7);
            m.ShouldNotBeNull();
            kg.ShouldNotBeNull();
            s.ShouldNotBeNull();
            A.ShouldNotBeNull();
            K.ShouldNotBeNull();
            mol.ShouldNotBeNull();
            cd.ShouldNotBeNull();
        }

        [TestMethod]
        public void ShouldKnowSomeDerivedUnits()
        {
            // Arrange & Act & Assert
            Hz.ShouldNotBeNull();
            N.ShouldNotBeNull();
            Pa.ShouldNotBeNull();
            J.ShouldNotBeNull();
            W.ShouldNotBeNull();
            C.ShouldNotBeNull();
            V.ShouldNotBeNull();
            F.ShouldNotBeNull();
            Ω.ShouldNotBeNull();
            S.ShouldNotBeNull();
            Wb.ShouldNotBeNull();
            T.ShouldNotBeNull();
            H.ShouldNotBeNull();
            lx.ShouldNotBeNull();
            Sv.ShouldNotBeNull();
            kat.ShouldNotBeNull();
        }
    }

    [TestOf(typeof(SISystem), nameof(Parse))]
    public sealed class ParseTests
    {
        [TestMethod]
        public void ShouldParseUnitsWithoutDenominator()
        {
            // Arrange & Act & Assert
            Parse("kW h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW×h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW × h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW·h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW · h").ShouldBe(MetricPrefix.Kilo * W * h);
        }

        [TestMethod]
        public void ShouldParseUnitsWithDenominator()
        {
            // Arrange & Act & Assert
            Parse("m/s").ShouldBe(m / s);
            Parse("m / s").ShouldBe(m / s);
        }

        [TestMethod]
        public void ShouldParseUnitsWithExponent()
        {
            // Arrange & Act & Assert
            Parse("J/m^3").ShouldBe(J / (m ^ 3));
            Parse("J / m^3").ShouldBe(J / (m ^ 3));
        }

        [TestMethod]
        public void ShouldParseUnitsWithNegativeExponent()
        {
            // Arrange & Act & Assert
            Parse("J m^-3").ShouldBe(J * (m ^ -3));
        }

        [TestMethod]
        public void ShouldParseMilligramsCorrectly()
        {
            // Arrange & Act & Assert
            Parse("mg").ShouldBe(kg / 1000000);
        }

        [TestMethod]
        public void ShouldParseGramsCorrectly()
        {
            // Arrange & Act & Assert
            Parse("g").ShouldBe(kg / 1000);
        }

        [TestMethod]
        public void ShouldThrowFormatExceptionForKiloKilograms()
        {
            // Arrange & Act & Assert
            Should.Throw<FormatException>(() => Parse("kkg"));
        }

        [TestMethod]
        public void ShouldParseInconsistentUnit()
        {
            // Arrange & Act & Assert
            Parse("h").ShouldBe(h);
        }
    }

    [TestOf(typeof(SISystem), nameof(Display))]
    public sealed class DisplayTests
    {
        [TestMethod]
        public void ShouldPrintAndReparseUnit()
        {
            // Arrange
            var expected = kg * m * (s ^ -2);

            // Act
            var display = Display(expected);

            // Assert
            display.ShouldBe("m kg / s^2");
            var result = Parse(display);
            expected.ShouldBe(result);
        }
    }
}
