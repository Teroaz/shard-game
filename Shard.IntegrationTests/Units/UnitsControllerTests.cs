using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Shared.Core;
using Shard.Shared.Web.IntegrationTests.Clock;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.IntegrationTests.Units;

public class UnitsControllerTests
{
    private readonly Mock<IUnitsService> _mockUnitsService;
    private readonly Mock<IUsersService> _mockUsersService;
    private readonly Mock<ISystemsService> _mockSystemsService;
    private readonly Mock<IBuildingsService> _mockBuildingsService;
    private readonly Mock<IClock> _mockClock;
    private readonly MapGenerator _mapGenerator;
    private readonly UnitsController _unitsController;
    private const string TestSeed = "testSeed";

    public UnitsControllerTests()
    {
        var options = new MapGeneratorOptions { Seed = TestSeed };
        _mockUnitsService = new Mock<IUnitsService>();
        _mockUsersService = new Mock<IUsersService>();
        _mockSystemsService = new Mock<ISystemsService>();
        _mockBuildingsService = new Mock<IBuildingsService>();
        _mockClock = new Mock<IClock>();
        _mapGenerator = new MapGenerator(options);
        _unitsController = new UnitsController(
            _mockUnitsService.Object,
            _mockUsersService.Object,
            _mockSystemsService.Object,
            _mockBuildingsService.Object,
            _mockClock.Object
        );
        _mockSystemsService
            .Setup(m => m.GetRandomSystem())
            .Returns(new SystemModel(_mapGenerator.Generate().Systems[0]))
            ;

        var system = _mockSystemsService.Object.GetRandomSystem()!;
        _mockSystemsService
            .Setup(m => m.GetRandomPlanet(system))
            .Returns(system.Planets[0])
            ;
    }

    [Fact]
    public void Get_ShouldReturnUnitsForUser()
    {
        // Arrange
        var userId = "TestUserId";
        var user = new UserModel("TestUser");
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        var system = _mockSystemsService.Object.GetRandomSystem()!;
        var units = new List<UnitModel>
        {
            new UnitModel("TestUnit", UnitType.Scout, system, _mockSystemsService.Object.GetRandomPlanet(system))
        };

        _mockUnitsService.Setup(service => service.GetUnitsByUser(user)).Returns(units);

        // Act
        var result = _unitsController.Get(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUnits = Assert.IsType<List<UnitsDto>>(okResult.Value);
        Assert.Single(returnedUnits);
    }

    //[Fact]
    // public async Task Get_WithUnitId_ShouldReturnUnitForUser()
    // {
    //     // Arrange
    //     var userId = "TestUserId";
    //     var user = new UserModel("TestUser");
    //     var system = _mockSystemsService.Object.GetRandomSystem()!;
    //     var unit = new UnitModel("TestUnit", UnitType.Scout, system, _mockSystemsService.Object.GetRandomPlanet(system));
    //     _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
    //     _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);
    //
    //     // Act
    //     var result = await _unitsController.Get(userId, unit.Id);
    //
    //     // Assert
    //     Assert.IsType<OkObjectResult>(result.Result);
    // }

    [Fact]
    public void GetLocation_ShouldReturnUnitLocationForUser()
    {
        // Arrange
        var userId = "TestUserId";
        var user = new UserModel("TestUser");
        var system = _mockSystemsService.Object.GetRandomSystem()!;
        var unit = new UnitModel("TestUnit", UnitType.Scout, system, _mockSystemsService.Object.GetRandomPlanet(system));
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);

        // Act
        var result = _unitsController.GetLocation(unit.Id, userId);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetLocation_ShouldReturnNotFoundWhenUserDoesNotExist()
    {
        // Arrange
        var userId = "TestUserId";
        var user = new UserModel("TestUser");
        var system = _mockSystemsService.Object.GetRandomSystem()!;
        var unit = new UnitModel("TestUnit", UnitType.Scout, system, _mockSystemsService.Object.GetRandomPlanet(system));
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);

        // Act
        var result = _unitsController.GetLocation(unit.Id, "TestUser");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetLocation_ShouldReturnNotFoundWhenUnitDoesNotExist()
    {
        // Arrange
        var userId = "TestUserId";
        var user = new UserModel("TestUser");
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);

        // Act
        var result = _unitsController.GetLocation("TestUnit", userId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetLocation_ShouldReturnNullOnResourcesWhenUnitTypeIsBuilder()
    {
        // Arrange
        var userId = "TestUserId";
        var user = new UserModel("TestUser");
        var system = _mockSystemsService.Object.GetRandomSystem()!;
        var unit = new UnitModel("TestUnit", UnitType.Builder, system, _mockSystemsService.Object.GetRandomPlanet(system));
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);

        // Act
        var result = _unitsController.GetLocation(unit.Id, userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUnit = Assert.IsType<UnitsLocationDto>(okResult.Value);
        Assert.Null(returnedUnit.ResourcesQuantity);
    }

    [Fact]
    public async Task Get_WithUnitId_ShouldReturnUnitForUser()
    {
        // Arrange
        var userId = "TestUserId";
        var user = new UserModel("TestUser");
        var system = _mockSystemsService.Object.GetRandomSystem()!;
        var unit = new UnitModel("TestUnit", UnitType.Scout, system, _mockSystemsService.Object.GetRandomPlanet(system));
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);

        // Act
        var result = await _unitsController.Get(userId, unit.Id);

        // Assert
        // var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // var returnedUnit = Assert.IsType<UnitsDto>(okResult.Value);
        // Assert.Equal("TestUnit", returnedUnit.Id);
    }

    [Fact]
    public async Task Get_UserNotFound_ShouldReturnNotFound()
    {
        // Arrange
        _mockUsersService.Setup(service => service.GetUserById(It.IsAny<string>())).Returns((UserModel)null);

        // Act
        var result = await _unitsController.Get("nonexistentUserId", "anyUnitId");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Get_UnitNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var userId = "existingUserId";
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(new UserModel(userId));
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(It.IsAny<UserModel>(), It.IsAny<string>())).Returns((UnitModel)null);

        // Act
        var result = await _unitsController.Get(userId, "nonexistentUnitId");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Get_UnitArrivalInFuture_ShouldReturnUnit()
    {
        // Arrange
        var userId = "existingUserId";
        var unitId = "existingUnitId";
        var user = new UserModel(userId);
        var system = _mockSystemsService.Object.GetRandomSystem();
        var unit = new UnitModel(unitId, UnitType.Scout, system, null) { EstimatedArrivalTime = _mockClock.Object.Now.AddDays(1) };

        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, unitId)).Returns(unit);

        // Act
        var result = await _unitsController.Get(userId, unitId);

        // Assert
        var returnedUnit = Assert.IsType<UnitsDto>(result.Value);
        Assert.Equal(unitId, returnedUnit.Id);
    }


    [Fact]
    public async Task Get_UnitArrivalWithinTwoSeconds_ShouldHandleCorrectly()
    {
        // Arrange
        var userId = "existingUserId";
        var unitId = "existingUnitId";
        var user = new UserModel(userId);
        var system = _mockSystemsService.Object.GetRandomSystem();
        var planet = _mockSystemsService.Object.GetRandomPlanet(system);
        var unit = new UnitModel(unitId, UnitType.Scout, system, planet) { EstimatedArrivalTime = _mockClock.Object.Now.AddSeconds(1) };

        unit.Move(new FakeClock(), system, null);

        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
        _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, unitId)).Returns(unit);

        // Act
        var result = await _unitsController.Get(userId, unitId);

        // Assert
        var returnedUnit = Assert.IsType<UnitsDto>(result.Value);
        Assert.Equal(unitId, returnedUnit.Id);
    }

    [Fact]
    public void Put_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        _mockUsersService.Setup(service => service.GetUserById(It.IsAny<string>())).Returns((UserModel)null);

        // Act
        var result = _unitsController.Put("nonExistentUserId", "anyUnitId", new UnitsBodyDto());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Put_InvalidRequestBody_ReturnsBadRequest()
    {
        // Arrange
        var userId = "existingUserId";
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(new UserModel(userId));
        _mockUnitsService.Setup(service => service.IsBodyValid(It.IsAny<string>(), It.IsAny<UnitsBodyDto>())).Returns(false);

        // Act
        var result = _unitsController.Put(userId, "anyUnitId", new UnitsBodyDto());

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public void Put_DestinationSystemNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = "existingUserId";
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(new UserModel(userId));
        _mockUnitsService.Setup(service => service.IsBodyValid(It.IsAny<string>(), It.IsAny<UnitsBodyDto>())).Returns(true);
        _mockSystemsService.Setup(service => service.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);

        // Act
        var result = _unitsController.Put(userId, "anyUnitId", new UnitsBodyDto { DestinationSystem = "nonExistentSystem" });

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Put_InvalidUnitType_ReturnsBadRequest()
    {
        // Arrange
        var userId = "existingUserId";
        var unitsBodyDto = new UnitsBodyDto { Type = "InvalidType" };
        _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(new UserModel(userId));
        _mockUnitsService.Setup(service => service.IsBodyValid(It.IsAny<string>(), unitsBodyDto)).Returns(true);

        // Act
        var result = _unitsController.Put(userId, "anyUnitId", unitsBodyDto);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }
}