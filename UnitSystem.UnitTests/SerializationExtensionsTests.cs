using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using UnitSystem.Dimensions;
using UnitSystem.Quantities;
using UnitSystem.Serialization;
using UnitSystem.Systems;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using ShInUeXx.Numerics;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(SerializationExtensions))]
public static class SerializationExtensionsTests
{
    [TestFixture]
    [TestOf(typeof(SerializationExtensions), nameof(SerializationExtensions.ToInfo))]
    public sealed class ToInfoTests
    {
        [Test]
        public void ShouldRejectAQuantityWhenItsCatalogCannotNameAllDimensions()
        {
            var system = Substitute.For<IUnitSystem>();
            system.Name.Returns("invalid catalog");
            system.NumberOfDimensions.Returns(1);
            system.BaseUnits.Returns(Array.Empty<KnownUnit>());
            system.MakeCoherent(Arg.Any<Unit>()).Returns(call => call.Arg<Unit>());
            var unit = new KnownUnit(system, 1, 0, new Dimension(1), "m", "metre");
            var quantity = new ScalarQuantity(12, unit);

            Should.Throw<InvalidOperationException>(() => quantity.ToInfo());
        }
    }

    [TestFixture]
    [TestOf(typeof(SerializationExtensions), nameof(SerializationExtensions.FromInfo))]
    public sealed class FromInfoTests
    {
        [Test]
        public void ShouldSerializeAndDeserializeUsingDataContractSerializer()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J / m.Pow(new Rational(1, 3)));
            var info = quantity.ToInfo();
            var serializer = new DataContractSerializer(typeof(QuantityInfo<double>));

            // Act
            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder))
            {
                serializer.WriteObject(writer, info);
            }

            QuantityInfo<double> deserializedInfo;
            using (var reader = XmlReader.Create(new StringReader(builder.ToString())))
            {
                deserializedInfo = serializer.ReadObject(reader).ShouldBeOfType<QuantityInfo<double>>();
            }

            var result = SISystem.System.FromInfo(deserializedInfo);

            // Assert
            quantity.ShouldBe(result);
        }

        [Test]
        public void ShouldSerializeAndDeserializeUsingJsonNet()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J / m.Pow(new Rational(1, 3)));
            var info = quantity.ToInfo();

            // Act
            var json = JsonConvert.SerializeObject(info);
            json.ShouldContain("\"m\":\"5/3\"");
            var deserializedInfo = JsonConvert.DeserializeObject<QuantityInfo<double>>(json);
            var result = SISystem.System.FromInfo(deserializedInfo);

            // Assert
            quantity.ShouldBe(result);
        }

        [Test]
        public void ShouldReadLegacyJsonExponents()
        {
            const string json = """{"Amount":12,"Unit":{"m":0.1}}""";
            var info = JsonConvert.DeserializeObject<QuantityInfo<double>>(json);

            SISystem.System.FromInfo(info).ShouldBe(new ScalarQuantity(12, m.Pow(new Rational(1, 10))));
        }

        [Test]
        public void ShouldReadLegacyDataContractExponents()
        {
            const string xml = """
                <QuantityInfoOfdouble xmlns="http://schemas.datacontract.org/2004/07/UnitSystem.Serialization">
                  <Amount>12</Amount>
                  <Unit xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
                    <a:KeyValueOfstringfloat><a:Key>m</a:Key><a:Value>0.1</a:Value></a:KeyValueOfstringfloat>
                  </Unit>
                </QuantityInfoOfdouble>
                """;
            var serializer = new DataContractSerializer(typeof(QuantityInfo<double>));
            using var reader = XmlReader.Create(new StringReader(xml));

            var info = (QuantityInfo<double>)serializer.ReadObject(reader)!;

            SISystem.System.FromInfo(info).ShouldBe(new ScalarQuantity(12, m.Pow(new Rational(1, 10))));
        }

        [Test]
        public void ShouldMapToInfoAndBackCorrectly()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J / m.Pow(new Rational(1, 3)));

            // Act
            var info = quantity.ToInfo();
            var result = SISystem.System.FromInfo(info);

            // Assert
            quantity.ShouldBe(result);
        }
    }
}
