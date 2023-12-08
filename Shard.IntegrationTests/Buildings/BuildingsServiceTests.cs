using Moq;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Buildings.Models;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;

namespace Shard.IntegrationTests.Buildings;

public class BuildingsServiceTests
{
    private readonly Mock<IBuildingsRepository> _mockRepo;
    private readonly BuildingsService _service;
    private readonly UserModel _user;
    private readonly Mock<ISystemsService> _mockSystemsService;

    public BuildingsServiceTests()
    {
        _mockRepo = new Mock<IBuildingsRepository>();
        _service = new BuildingsService(_mockRepo.Object);
        _user = new UserModel("JohnDoe");
        _mockSystemsService = new Mock<ISystemsService>();
        _mockSystemsService.Setup(m => m.GetRandomSystem()); 
        _mockSystemsService.Setup(m => m.GetRandomPlanet(It.IsAny<SystemModel>()));
    }

    [Fact]
    public void CanGetBuildingByIdAndUser()
    {
        // Arrange
        var system =  _mockSystemsService.Object.GetRandomSystem()!;
        var building = new BuildingModel("1", _user, BuildingType.Mine, BuildingResourceCategory.Gaseous, system!, _mockSystemsService.Object.GetRandomPlanet(system)!);
        _mockRepo.Setup(repo => repo.GetBuildingByIdAndUser(_user, "1")).Returns(building);

        // Act
        var result = _service.GetBuildingByIdAndUser(_user, "1");

        // Assert
        Assert.Equal(building, result);
    }

    [Fact]
    public void CanGetBuildingsByUser()
    {
        // Arrange
        var system =  _mockSystemsService.Object.GetRandomSystem()!;
        var buildings = new List<BuildingModel> {
            new("1", _user, BuildingType.Mine, BuildingResourceCategory.Gaseous, system, _mockSystemsService.Object.GetRandomPlanet(system)!),
            new("2", _user, BuildingType.Mine, BuildingResourceCategory.Gaseous, system, _mockSystemsService.Object.GetRandomPlanet(system)!)
        };

        _mockRepo.Setup(repo => repo.GetBuildingsByUser(_user)).Returns(buildings);

        // Act
        var result = _service.GetBuildingsByUser(_user);

        // Assert
        Assert.Equal(buildings, result);
    }
    
    [Fact]
    public void CanAddBuilding()
    {
        // Arrange
        var system =  _mockSystemsService.Object.GetRandomSystem()!;
        var building = new BuildingModel("1", _user, BuildingType.Mine, BuildingResourceCategory.Gaseous, system, _mockSystemsService.Object.GetRandomPlanet(system)!);

        // Act
        _service.AddBuilding(_user, building);

        // Assert
        _mockRepo.Verify(repo => repo.AddBuilding(_user, building), Times.Once);
    }

    [Fact]
    public void CanRemoveBuilding()
    {
        // Arrange
        var system =  _mockSystemsService.Object.GetRandomSystem()!;
        var building = new BuildingModel("1", _user, BuildingType.Mine, BuildingResourceCategory.Gaseous, system, _mockSystemsService.Object.GetRandomPlanet(system)!);

        // Act
        _service.RemoveBuilding(_user, building);

        // Assert
        _mockRepo.Verify(repo => repo.RemoveBuilding(_user, building), Times.Once);
    }

    [Fact]
    public void CanUpdateBuilding()
    {
        // Arrange
        var system =  _mockSystemsService.Object.GetRandomSystem()!;
        var building = new BuildingModel("1", _user, BuildingType.Mine, BuildingResourceCategory.Gaseous, system, _mockSystemsService.Object.GetRandomPlanet(system)!);

        // Act
        _service.UpdateBuilding(_user, building);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateBuilding(_user, building), Times.Once);
    }

}