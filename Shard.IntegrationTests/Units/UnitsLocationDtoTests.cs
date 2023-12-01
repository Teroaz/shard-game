using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;

namespace Shard.IntegrationTests.Units;

public class UnitsLocationDtoTests
{
    
    private readonly Mock<ISystemsService> _mockSystemsService;
    private const string TestSeed = "testSeed";
    private readonly MapGenerator _mapGenerator;
    private SystemModel _systemModel;

    public UnitsLocationDtoTests()
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
            
        var resourceQuantity = new Dictionary<ResourceKind, int>
        {
            { ResourceKind.Carbon, 100 },
            { ResourceKind.Gold, 200 }
        };

        // Act
        var unitsLocationDto = new UnitsLocationDto(unitModel, resourceQuantity);

        // Assert
        Assert.Equal(unitModel.System.Name, unitsLocationDto.System);
        Assert.Equal(unitModel.Planet?.Name, unitsLocationDto.Planet);
        Assert.Equal(resourceQuantity[ResourceKind.Carbon], unitsLocationDto.ResourcesQuantity?[ResourceKind.Carbon]);
        Assert.Equal(resourceQuantity[ResourceKind.Gold], unitsLocationDto.ResourcesQuantity?[ResourceKind.Gold]);
    }
}