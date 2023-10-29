using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;

namespace Shard.IntegrationTests.Units;

public class UnitsDtoTests
{
    
    private readonly Mock<ISystemsService> _mockSystemsService;
    private const string TestSeed = "testSeed";
    private readonly MapGenerator _mapGenerator;
    private SystemModel _systemModel;

    public UnitsDtoTests()
    {
        var options = new MapGeneratorOptions { Seed = TestSeed };
        _mapGenerator = new MapGenerator(options);
        _mockSystemsService = new Mock<ISystemsService>();
        _mockSystemsService
            .Setup(m => m.GetRandomSystem())
            .Returns(new SystemModel(_mapGenerator.Generate().Systems[0]))
            ;
        
        _systemModel = _mockSystemsService.Object.GetRandomSystem()!;
        _mockSystemsService
            .Setup(m => m.GetRandomPlanet(_systemModel))
            .Returns(_systemModel.Planets[0])
            ;
    }
    
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var unitModel = new UnitModel("TestUnit", UnitType.Scout, _systemModel, _mockSystemsService.Object.GetRandomPlanet(_systemModel));

        // Act
        var unitsDto = new UnitsDto(unitModel);

        // Assert
        Assert.Equal(unitModel.Id, unitsDto.Id);
        Assert.Equal(unitModel.Type.ToString().ToLower(), unitsDto.Type);  // Assuming ToLowerString() converts the string to lowercase
        Assert.Equal(unitModel.System.Name, unitsDto.System);
        Assert.Equal(unitModel.Planet?.Name, unitsDto.Planet);
        Assert.Equal(unitModel.DestinationSystem.Name, unitsDto.DestinationSystem);
        Assert.Equal(unitModel.DestinationPlanet?.Name, unitsDto.DestinationPlanet);
        Assert.Equal(unitModel.EstimatedArrivalTime.ToString("yyyy-MM-ddTHH:mm:ss"), unitsDto.EstimatedArrivalTime);
    }
}