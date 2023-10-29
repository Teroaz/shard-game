using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Buildings.DTOs;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.IntegrationTests.Buildings;

public class BuildingsControllerTests
{
    private readonly BuildingsController _controller;
    private readonly Mock<IBuildingsService> _mockBuildingService;
    private readonly Mock<IUsersService> _mockUserService;
    private readonly Mock<IUnitsService> _mockUnitsService;

    private readonly Mock<ISystemsService> _mockSystemsService;

    public BuildingsControllerTests()
    {
        _mockBuildingService = new Mock<IBuildingsService>();
        _mockUserService = new Mock<IUsersService>();
        _mockUnitsService = new Mock<IUnitsService>();

        _controller = new BuildingsController(_mockBuildingService.Object, _mockUserService.Object,
            _mockUnitsService.Object);
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
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto("1", "Type", "BuilderId"));

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
        var result = _controller.CreateBuilding(userId, new CreateBuildingBodyDto(null, null, null));

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }
}