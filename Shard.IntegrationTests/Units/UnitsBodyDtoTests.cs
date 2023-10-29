using Shard.Web.ImplementationAPI.Units.DTOs;

namespace Shard.IntegrationTests.Units;

public class UnitsBodyDtoTests
{
    [Fact]
    public void Properties_SetAndGet_ReturnsExpectedValues()
    {
        // Arrange
        var unitsBodyDto = new UnitsBodyDto()
        {
            Id = "unit1",
            Type = "TypeA",
            System = "SystemX",
            Planet = "PlanetY",
            DestinationSystem = "SystemZ",
            DestinationPlanet = "PlanetW"
        };

        // Act & Assert
        Assert.Equal("unit1", unitsBodyDto.Id);
        Assert.Equal("TypeA", unitsBodyDto.Type);
        Assert.Equal("SystemX", unitsBodyDto.System);
        Assert.Equal("PlanetY", unitsBodyDto.Planet);
        Assert.Equal("SystemZ", unitsBodyDto.DestinationSystem);
        Assert.Equal("PlanetW", unitsBodyDto.DestinationPlanet);
    }
}