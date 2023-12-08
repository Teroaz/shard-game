using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.Models;

namespace Shard.IntegrationTests.Units;

public class UnitModelTests
{
    private readonly Mock<ISystemsService> _mockSystemsService;
    private const string TestSeed = "testSeed";
    private readonly MapGenerator _mapGenerator;
    private SystemModel _systemModel;
    private PlanetModel _planetModel;
    private readonly Mock<IClock> _mockClock;

    public UnitModelTests()
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
        
        _planetModel = _systemModel.Planets[0];
        
        _mockClock = new Mock<IClock>();
        _mockClock.Setup(m => m.Now).Returns(new DateTime(2023, 10, 29, 12, 0, 0));  // Set a fixed current time
    }
    
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange

        // Act
        var unitModel = new ScoutUnitModel(_systemModel, _planetModel);

        // Assert
        Assert.NotNull(unitModel.Id);
        Assert.Equal(UnitType.Scout, unitModel.Type);
        Assert.Equal(_systemModel, unitModel.System);
        Assert.Equal(_planetModel, unitModel.Planet);
        Assert.Equal(unitModel.DestinationSystem, unitModel.DestinationSystem);
        Assert.Equal(unitModel.DestinationPlanet, unitModel.DestinationPlanet);
    }

    [Fact]
    public void Move_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var unitModel = new ScoutUnitModel(_systemModel, _planetModel);

        // Act
        unitModel.Move(_mockClock.Object, unitModel.DestinationSystem, unitModel.DestinationPlanet);

        // Assert
        Assert.Equal(unitModel.DestinationSystem, unitModel.DestinationSystem);
        Assert.Equal(unitModel.DestinationPlanet, unitModel.DestinationPlanet);
        Assert.Equal(new DateTime(2023, 10, 29, 12, 0, 0), unitModel.EstimatedArrivalTime);
    }
}