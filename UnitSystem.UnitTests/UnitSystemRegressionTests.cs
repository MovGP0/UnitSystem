using System.Globalization;
using UnitSystem.Prefixes;
using UnitSystem.Quantities;
using UnitSystem.Serialization;
using UnitSystem.Systems;
using Newtonsoft.Json;
using Shouldly;

namespace UnitSystem.UnitTests;

[TestClass]
public sealed class UnitSystemRegressionTests
{
    [TestMethod]
    public void ShouldConvertTemperaturePointsThroughTheSamePath()
    {
        var freezingCelsius = new ScalarQuantity(0, SISystem.degC);
        var freezingFahrenheit = new ScalarQuantity(32, ImperialSystem.degF);
        Quantity<double> freezingKelvin = new ScalarQuantity(273.15, SISystem.K);

        freezingCelsius.Convert(SISystem.K).Amount.ShouldBe(273.15);
        freezingFahrenheit.Convert(SISystem.K).Amount.ShouldBe(273.15);
        freezingKelvin.Convert(SISystem.degC).Amount.ShouldBe(0);
        freezingKelvin.Convert(ImperialSystem.degF).Amount.ShouldBe(32, 1e-12);
        freezingCelsius.ShouldBe(freezingFahrenheit);
    }

    [TestMethod]
    public void ShouldApplyOnlyScaleWhenConvertingTemperatureDifferences()
    {
        var start = new ScalarQuantity(10, SISystem.degC);
        var end = new ScalarQuantity(20, SISystem.degC);
        var difference = end - start;

        difference.IsPoint.ShouldBeFalse();
        difference.Convert(ImperialSystem.degF).Amount.ShouldBe(18, 1e-12);
        difference.Convert(SISystem.degC).ToString(CultureInfo.InvariantCulture).ShouldBe("10 Δ°C");

        var fahrenheitDifference = new ScalarQuantity(18, ImperialSystem.degF, isPoint: false);
        (start + fahrenheitDifference).ShouldBe(end);
        (fahrenheitDifference + start).ShouldBe(end);
        (end - fahrenheitDifference).ShouldBe(start);
        Should.Throw<InvalidOperationException>(() => start + end);
        Should.Throw<InvalidOperationException>(() => difference - start);
        Should.Throw<InvalidOperationException>(() => start * 2);
    }

    [TestMethod]
    public void ShouldPreserveTemperatureSemanticsInSerializedCanonicalValues()
    {
        var point = new ScalarQuantity(0, SISystem.degC);
        var difference = new ScalarQuantity(10, SISystem.degC, isPoint: false);
        var pointInfo = point.ToInfo();
        var differenceInfo = difference.ToInfo();

        pointInfo.Amount.ShouldBe(273.15);
        differenceInfo.Amount.ShouldBe(10);
        pointInfo.Unit.Keys.ShouldBe(new[] { "K" });

        var json = JsonConvert.SerializeObject(differenceInfo);
        var decoded = JsonConvert.DeserializeObject<QuantityInfo<double>>(json);
        SISystem.System.FromInfo(decoded).ShouldBe(difference);
        SISystem.System.FromInfo(pointInfo).ShouldBe(point);
    }

    [TestMethod]
    public void ShouldUseCanonicalValuesForEqualityHashingAndOrdering()
    {
        var metre = new ScalarQuantity(1, SISystem.m);
        var millimetres = new ScalarQuantity(1000, MetricPrefix.Milli * SISystem.m);
        var second = new ScalarQuantity(1, SISystem.s);

        metre.ShouldBe(millimetres);
        metre.GetHashCode().ShouldBe(millimetres.GetHashCode());
        new HashSet<ScalarQuantity> { metre }.Contains(millimetres).ShouldBeTrue();
        metre.CompareTo(millimetres).ShouldBe(0);
        Should.Throw<InvalidOperationException>(() => metre.CompareTo(second));
        Should.Throw<InvalidOperationException>(() => metre < second);

        var point = new ScalarQuantity(1, SISystem.K);
        var difference = new ScalarQuantity(1, SISystem.K, isPoint: false);
        point.ShouldNotBe(difference);
        Should.Throw<InvalidOperationException>(() => point.CompareTo(difference));
    }

    [TestMethod]
    public void ShouldPublishTheValidatedStandardCatalogWithCompatibleDimensions()
    {
        CGSSystem.dyn.ShouldBeSameAs(CGSSystem.dyne);
        CGSSystem.Parse("stC").ShouldBeSameAs(CGSSystem.statC);
        CGSSystem.kg.ShouldBeSameAs(SISystem.kg);
        CGSSystem.Gs.ShouldBe(SISystem.T / 10_000);
        SISystem.T.ShouldBe(SISystem.Wb / (SISystem.m ^ 2));
        ImperialSystem.psi.ShouldBe(ImperialSystem.lbf / (ImperialSystem.@in ^ 2));
        ImperialSystem.hp.HasSameDimension(SISystem.W).ShouldBeTrue();
        ImperialSystem.btu.HasSameDimension(SISystem.J).ShouldBeTrue();
        new ScalarQuantity(1, CGSSystem.cm).Convert(SISystem.m).Amount.ShouldBe(0.01);
        new ScalarQuantity(1, ImperialSystem.ft).Convert(SISystem.m).Amount.ShouldBe(0.3048);
        Should.Throw<InvalidOperationException>(() => SISystem.AddDerivedUnit("extra", "extra", 2 * SISystem.m));
    }

    [TestMethod]
    public void ShouldParseRegisteredSymbolsBeforePrefixesAndRoundTripExpressions()
    {
        SISystem.Parse("m").ShouldBeSameAs(SISystem.m);
        SISystem.Parse("°C").ShouldBeSameAs(SISystem.degC);
        ImperialSystem.Parse("long ton").ShouldBeSameAs(ImperialSystem.ton_long);
        SISystem.Parse("kg^2").ShouldBe(SISystem.kg ^ 2);
        SISystem.Parse("km").ToString().ShouldBe("km");
        SISystem.Parse("mm").ShouldBe(MetricPrefix.Milli * SISystem.m);
        Should.Throw<FormatException>(() => SISystem.Parse("m//s"));
        Should.Throw<FormatException>(() => SISystem.Parse("m**s"));
        ImperialSystem.Parse("us gal s").ShouldBe(ImperialSystem.gal_us * SISystem.s);

        var system = SISystem.System;
        var units = new[]
        {
            SISystem.kg / 100,
            3.25 * (SISystem.m ^ 0.5f) / SISystem.s,
            SISystem.s ^ -1,
            0.01 * system.NoUnit,
            system.CreateUnit(0.42, 2, SISystem.K.Dimension)
        };
        foreach (var unit in units)
        {
            system.Parse(system.Display(unit)).ShouldBe(unit);
        }
    }

    [TestMethod]
    public void ShouldRefreshCustomRegistrationsWithoutPartialChangesOnFailure()
    {
        var system = UnitSystemFactory.CreateSystem("test");
        var unit = system.AddBaseUnit("u", "unit", dimensionSymbol: "L");
        system.Parse("ku").ShouldBe(1000 * unit);

        var exact = system.AddDerivedUnit("ku", "custom unit", 2 * unit);
        system.Parse("ku").ShouldBeSameAs(exact);
        system.Parse(system.Display(1000 * unit)).ShouldBe(1000 * unit);

        var foreign = UnitSystemFactory.CreateSystem("foreign").AddBaseUnit("f", "foreign");
        Should.Throw<InvalidOperationException>(() => system.AddDerivedUnit("bad", "foreign unit", foreign));
        Should.Throw<InvalidOperationException>(() => system.AddBaseUnit("u", "duplicate"));

        var second = system.AddBaseUnit("s", "second", dimensionSymbol: "T");
        system.NumberOfDimensions.ShouldBe(2);
        system.Parse("u/s").ShouldBe(unit / second);

        var custom = system.AddBaseUnit("two words", "custom unit");
        var product = custom * second;
        system.Parse(system.Display(product)).ShouldBe(product);
        Should.Throw<ArgumentException>(() => system.AddDerivedUnit("zero", "zero", 0 * unit));
        system["zero"].ShouldBeNull();
    }
}
