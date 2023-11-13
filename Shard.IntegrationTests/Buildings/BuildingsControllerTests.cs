using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Buildings.DTOs;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Buildings;

public class BuildingsControllerTests
{
    private readonly BuildingsController _controller;
    private readonly Mock<IBuildingsService> _mockBuildingService;
    private readonly Mock<IUsersService> _mockUserService;
    private readonly Mock<IUnitsService> _mockUnitsService;
    private readonly Mock<IClock> _mockClock;

    private readonly Mock<ISystemsService> _mockSystemsService;

    public BuildingsControllerTests()
    {
        _mockBuildingService = new Mock<IBuildingsService>();
        _mockUserService = new Mock<IUsersService>();
        _mockUnitsService = new Mock<IUnitsService>();
        _mockClock = new Mock<IClock>();

        _controller = new BuildingsController(_mockBuildingService.Object, _mockUserService.Object,
            _mockUnitsService.Object, _mockClock.Object);
        _mockSystemsService = new Mock<ISystemsService>();
        _mockSystemsService.Setup(m => m.GetRandomSystem());
        _mockSystemsService.Setup(m => m.GetRandomPlanet(It.IsAny<SystemModel>()));
    }

    [Fact]
    public void CreateBuilding_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "someUserId";

        // Act
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto("1", "Type", "BuilderId", "liquid"));

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void CreateBuilding_ReturnsBadRequest_WhenParametersAreInvalid()
    {
        // Arrange
        var userId = "someUserId";
        var user = new UserModel("someUser");
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns(user);

        // Act
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto(null, null, null, null));

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }
    
    [Fact]
    public void CreateBuilding_ReturnsBadRequest_WhenBuildingTypeIsInvalid()
    {
        // Arrange
        var userId = "invalidTypeUserId";
        var user = new UserModel("userWithInvalidType");
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns(user);

        // Act
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto("1", "InvalidType", "BuilderId", "liquid"));

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public void CreateBuilding_ReturnsBadRequest_WhenBuilderIdIsInvalidOrUnitHasNoPlanet()
    {
        // Arrange
        var userId = "validUserButInvalidUnitId";
        var user = new UserModel("userWithInvalidUnitId");
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(s => s.GetUnitByIdAndUser(user, "BuilderId")).Returns((UnitModel)null);

        // Act
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto("1", "Type", "BuilderId", "liquid"));
        
        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public void CreateBuilding_ReturnsBuildingDto_WhenAllConditionsAreMet()
    {
        // Arrange
        var userId = "someUserId";
        var user = new UserModel("someUser");

        var options = new MapGeneratorOptions { Seed = "TestSeed" };
        var mapGenerator = new MapGenerator(options);
        var sectorSpecification = mapGenerator.Generate();

        _mockSystemsService.Setup(m => m.GetRandomSystem()).Returns(new SystemModel(sectorSpecification.Systems[0]));
        _mockSystemsService.Setup(m => m.GetRandomPlanet(It.IsAny<SystemModel>()))
            .Returns(new PlanetModel(sectorSpecification.Systems[0].Planets[0]));
        
        var unit = new UnitModel(UnitType.Builder, _mockSystemsService.Object.GetRandomSystem(), _mockSystemsService.Object.GetRandomPlanet(It.IsAny<SystemModel>()));
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(u => u.GetUnitByIdAndUser(user, "BuilderId")).Returns(unit);

        // Act
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto("1", BuildingType.Mine.ToLowerString(), "BuilderId", "liquid"));
        
        // Assert
        Assert.IsType<BuildingDto>(result.Value);
        Assert.Equal("1", result.Value.Id);
        Assert.Equal(BuildingType.Mine.ToLowerString(), result.Value.Type);
        Assert.Equal(unit.System.Name, result.Value.System);
        Assert.Equal(unit.Planet?.Name, result.Value.Planet);
        
    }

    
}