using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Shouldly;
using UnitSystem.Quantities;
using UnitSystem.Serialization;
using UnitSystem.Systems;
using UnitTests.Shared;
using static UnitSystem.Systems.SISystem;

namespace UnitSystem.UnitTests;

[TestOf(typeof(SerializationExtensions))]
public static class SerializationExtensionsTests
{
    [TestOf(typeof(SerializationExtensions), nameof(SerializationExtensions.FromInfo))]
    public sealed class FromInfoTests
    {
        [TestMethod]
        public void ShouldSerializeAndDeserializeUsingDataContractSerializer()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J / (m ^ 3));
            var info = quantity.ToInfo();
            var serializer = new DataContractSerializer(typeof(QuantityInfo<>));

            // Act
            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder))
            {
                serializer.WriteObject(writer, info);
            }

            QuantityInfo<double> deserializedInfo;
            using (var reader = XmlReader.Create(new StringReader(builder.ToString())))
            {
                deserializedInfo = (QuantityInfo<double>)serializer.ReadObject(reader);
            }

            var result = SISystem.System.FromInfo(deserializedInfo);

            // Assert
            quantity.ShouldBe(result);
        }

        [TestMethod]
        public void ShouldSerializeAndDeserializeUsingJsonNet()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J / (m ^ 3));
            var info = quantity.ToInfo();

            // Act
            var json = JsonConvert.SerializeObject(info);
            var deserializedInfo = JsonConvert.DeserializeObject<QuantityInfo<double>>(json);
            var result = SISystem.System.FromInfo(deserializedInfo);

            // Assert
            quantity.ShouldBe(result);
        }

        [TestMethod]
        public void ShouldMapToInfoAndBackCorrectly()
        {
            // Arrange
            var quantity = new ScalarQuantity(100, J / (m ^ 3));

            // Act
            var info = quantity.ToInfo();
            var result = SISystem.System.FromInfo(info);

            // Assert
            quantity.ShouldBe(result);
        }
    }
}
