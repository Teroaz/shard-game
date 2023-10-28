using Shard.Web.ImplementationAPI.Buildings;

namespace Shard.IntegrationTests.Buildings;

public class BuildingTypeExtensionsTests
{
    [Theory]
    [InlineData(BuildingType.Mine, "mine")]
    // ... other BuildingType enum values ...
    public void ToLowerString_ShouldReturnLowerCaseString(BuildingType buildingType, string expected)
    {
        // Act
        var result = buildingType.ToLowerString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Mine", true)]
    [InlineData("InvalidType", false)]
    // ... other string values ...
    public void IsValidBuildingType_ShouldReturnExpectedResult(string type, bool expected)
    {
        // Act
        var result = type.IsValidBuildingType();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Mine", BuildingType.Mine)]
    // ... other valid string and BuildingType enum value pairs ...
    public void ToBuildingType_ShouldReturnBuildingType(string type, BuildingType expected)
    {
        // Act
        var result = type.ToBuildingType();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("InvalidType")]
    // ... other invalid string values ...
    public void ToBuildingType_ShouldThrowArgumentException(string type)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => type.ToBuildingType());
        Assert.Contains($"Invalid unit type: {type}", ex.Message);
    }
}