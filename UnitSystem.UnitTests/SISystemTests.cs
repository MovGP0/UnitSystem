using UnitSystem.Prefixes;
using UnitSystem.Systems;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(SISystem))]
public static class SISystemTests
{
    [TestFixture]
    [TestOf(typeof(SISystem), nameof(BaseUnits))]
    public sealed class BaseUnitsTests
    {
        [Test]
        public void ShouldDefineBaseUnitsIncludingAngle()
        {
            // Arrange & Act & Assert
            BaseUnits.Count().ShouldBe(8);
            rad.ShouldNotBeNull();
            m.ShouldNotBeNull();
            kg.ShouldNotBeNull();
            s.ShouldNotBeNull();
            A.ShouldNotBeNull();
            K.ShouldNotBeNull();
            mol.ShouldNotBeNull();
            cd.ShouldNotBeNull();
        }

        [Test]
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

    [TestFixture]
    [TestOf(typeof(SISystem), nameof(Parse))]
    public sealed class ParseTests
    {
        [Test]
        public void ShouldParseUnitsWithoutDenominator()
        {
            // Arrange & Act & Assert
            Parse("kW h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW×h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW × h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW·h").ShouldBe(MetricPrefix.Kilo * W * h);
            Parse("kW · h").ShouldBe(MetricPrefix.Kilo * W * h);
        }

        [Test]
        public void ShouldParseUnitsWithDenominator()
        {
            // Arrange & Act & Assert
            Parse("m/s").ShouldBe(m / s);
            Parse("m / s").ShouldBe(m / s);
        }

        [Test]
        public void ShouldParseUnitsWithExponent()
        {
            // Arrange & Act & Assert
            Parse("J/m^3").ShouldBe(J / m.Pow(3));
            Parse("J / m^3").ShouldBe(J / m.Pow(3));
        }

        [Test]
        public void ShouldParseUnitsWithNegativeExponent()
        {
            // Arrange & Act & Assert
            Parse("J m^-3").ShouldBe(J * m.Pow(-3));
        }

        [Test]
        public void ShouldParseMilligramsCorrectly()
        {
            // Arrange & Act & Assert
            Parse("mg").ShouldBe(kg / 1000000);
        }

        [Test]
        public void ShouldParseGramsCorrectly()
        {
            // Arrange & Act & Assert
            Parse("g").ShouldBe(kg / 1000);
        }

        [Test]
        public void ShouldThrowFormatExceptionForKiloKilograms()
        {
            // Arrange & Act & Assert
            Should.Throw<FormatException>(() => Parse("kkg"));
        }

        [Test]
        public void ShouldParseInconsistentUnit()
        {
            // Arrange & Act & Assert
            Parse("h").ShouldBe(h);
        }
    }

    [TestFixture]
    [TestOf(typeof(SISystem), nameof(Display))]
    public sealed class DisplayTests
    {
        [Test]
        public void ShouldPrintAndReparseUnit()
        {
            // Arrange
            var expected = kg * m * s.Pow(-2);

            // Act
            var display = Display(expected);

            // Assert
            display.ShouldBe("m kg / s^2");
            var result = Parse(display);
            expected.ShouldBe(result);
        }
    }
}
