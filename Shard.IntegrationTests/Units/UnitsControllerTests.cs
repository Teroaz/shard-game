using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.IntegrationTests.Units;

public class UnitsControllerTests
{
    private readonly Mock<IUnitsService> _mockUnitsService;
    private readonly Mock<IUsersService> _mockUsersService;
    private readonly Mock<ISystemsService> _mockSystemsService;
    private readonly Mock<IClock> _mockClock;
    private readonly UnitsController _unitsController;

    public UnitsControllerTests()
    {
        _mockUnitsService = new Mock<IUnitsService>();
        _mockUsersService = new Mock<IUsersService>();
        _mockSystemsService = new Mock<ISystemsService>();
        _mockClock = new Mock<IClock>();
        _unitsController = new UnitsController(
            _mockUnitsService.Object,
            _mockUsersService.Object,
            _mockSystemsService.Object,
            _mockClock.Object
        );
        _mockSystemsService.Setup(m => m.GetRandomSystem());
    }

    // [Fact]
    // public void Get_ShouldReturnUnitsForUser()
    // {
    //     // Arrange
    //     var userId = "TestUserId";
    //     var user = new UserModel("TestUser");
    //     _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
    //     var units = new List<UnitModel>
    //     {
    //         new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null)
    //     };
    //     
    //     _mockUnitsService.Setup(service => service.GetUnitsByUser(user)).Returns(units);
    //
    //     // Act
    //     var result = _unitsController.Get(userId);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //     var returnedUnits = Assert.IsType<List<UnitsDto>>(okResult.Value);
    //     Assert.Single(returnedUnits);
    // }

    // [Fact]
    // public async Task Get_WithUnitId_ShouldReturnUnitForUser()
    // {
    //     // Arrange
    //     var userId = "TestUserId";
    //     var user = new UserModel("TestUser");
    //     var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
    //     _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
    //     _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);
    //
    //     // Act
    //     var result = await _unitsController.Get(userId, "TestUnit");
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //     var returnedUnit = Assert.IsType<UnitsDto>(okResult.Value);
    // }
    //
    // [Fact]
    // public void GetLocation_ShouldReturnUnitLocationForUser()
    // {
    //     // Arrange
    //     var userId = "TestUserId";
    //     var user = new UserModel("TestUser");
    //     var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
    //     _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
    //     _mockUnitsService.Setup(service => service.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);
    //
    //     // Act
    //     var result = _unitsController.GetLocation("TestUnit", userId);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //     var returnedLocation = Assert.IsType<UnitsLocationDto>(okResult.Value);
    // }
    //
    // [Fact]
    // public void Put_ShouldUpdateUnitForUser()
    // {
    //     // Arrange
    //     var userId = "TestUserId";
    //     var user = new UserModel("TestUser");
    //     var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
    //     var unitsBodyDto = new UnitsBodyDto
    //     {
    //         Id = "TestUnit",
    //         Type = "Scout",
    //         System = "TestSystem",
    //         DestinationSystem = "DestinationSystem",
    //         DestinationPlanet = "DestinationPlanet"
    //     };
    //     _mockUsersService.Setup(service => service.GetUserById(userId)).Returns(user);
    //     _mockUnitsService.Setup(service => service.IsBodyValid("TestUnit", unitsBodyDto)).Returns(true);
    //     _mockSystemsService.Setup(service => service.GetSystem("DestinationSystem"));
    //
    //     // Act
    //     var result = _unitsController.Put(userId, "TestUnit", unitsBodyDto);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //     var returnedUnit = Assert.IsType<UnitsDto>(okResult.Value);
    // }
}