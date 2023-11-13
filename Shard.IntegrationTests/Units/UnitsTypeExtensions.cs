using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Units;

public class UnitsTypeExtensions
{
    public class UnitTypeExtensionsTests
    {
        [Theory]
        [InlineData(UnitType.Scout, "scout")]
        [InlineData(UnitType.Builder, "builder")]
        public void ToLowerString_ReturnsLowerCaseString(UnitType unitType, string expected)
        {
            // Act
            var result = unitType.ToLowerString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Scout", true)]
        [InlineData("Builder", true)]
        [InlineData("InvalidType", false)]
        public void IsValidUnitType_ReturnsExpectedValidity(string type, bool expected)
        {
            // Act
            var result = type.IsValidEnumValue<UnitType>();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Scout", UnitType.Scout)]
        [InlineData("Builder", UnitType.Builder)]
        public void ToUnitType_ReturnsExpectedUnitType(string type, UnitType expected)
        {
            // Act
            var result = type.ToEnum<UnitType>();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("InvalidType")]
        public void ToUnitType_ThrowsArgumentException_WhenInvalidType(string type)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => type.ToEnum<UnitType>());
        }
    }
}