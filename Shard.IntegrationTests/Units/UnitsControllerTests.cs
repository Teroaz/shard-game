using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.IntegrationTests.Units;

public class UnitsControllerTests
{
    private readonly Mock<IUnitsService> _mockUnitsService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ISystemsService> _mockSystemsService;
    private readonly UnitsController _controller;

    public UnitsControllerTests()
    {
        _mockUnitsService = new Mock<IUnitsService>();
        _mockUserService = new Mock<IUserService>();
        _mockSystemsService = new Mock<ISystemsService>();
        _controller = new UnitsController(_mockUnitsService.Object, _mockUserService.Object, _mockSystemsService.Object);
    }

    [Fact]
    public void GetUnit_WhenUnitDoesNotExist_ReturnsNotFound()
    {
        _mockUnitsService.Setup(x => x.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>())).Returns(value: null);

        var result = _controller.Get("testUnitId", "testUserId") as ActionResult<UnitsDto>;

        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetUnit_WhenUnitExists_ReturnsOk()
    {
        var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId", "testUserId");
        _mockUnitsService.Setup(x => x.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>())).Returns(unit);

        var result = _controller.Get("testUnitId", "testUserId") as ActionResult<UnitsDto>;

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetLocation_WhenUnitDoesNotExist_ReturnsNotFound()
    {
        _mockUnitsService.Setup(x => x.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>())).Returns(value: null);

        var result = _controller.GetLocation("testUnitId", "testUserId") as ActionResult<UnitsLocationDto>;

        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetLocation_WhenUnitExistsButSystemDoesNotExist_ReturnsOkWithoutResourceQuantity()
    {
        var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId", "testUserId");
        _mockUnitsService.Setup(x => x.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>())).Returns(unit);
        _mockSystemsService.Setup(x => x.GetSystem(It.IsAny<string>())).Returns(value: null);

        var result = _controller.GetLocation("testUnitId", "testUserId") as ActionResult<UnitsLocationDto>;

        Assert.NotNull(result);
        var locationDto = result.Value;
        Assert.Null(locationDto?.ResourcesQuantity);
    }

    [Fact]
    public void Put_WhenBodyIsInvalid_ReturnsBadRequest()
    {
        _mockUnitsService.Setup(x => x.IsBodyValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UnitsBodyDto>()))
            .Returns(false);

        var result = _controller.Put("testUnitId", "testUserId", new UnitsBodyDto()) as ActionResult<UnitsDto>;

        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public void Put_WhenUserDoesNotExist_ReturnsNotFound()
    {
        _mockUnitsService.Setup(x => x.IsBodyValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UnitsBodyDto>()))
            .Returns(true);
        _mockUserService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(value: null);

        var result = _controller.Put("testUnitId", "testUserId", new UnitsBodyDto()) as ActionResult<UnitsDto>;

        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Put_WhenUserExistsAndValidBody_ReturnsOk()
    {
        var user = new UserModel("testUserId", "testPseudo", DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
        var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId", "testUserId");

        _mockUnitsService.Setup(x => x.IsBodyValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UnitsBodyDto>()))
            .Returns(true);
        _mockUserService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(new UserDto(user));
        _mockUnitsService
            .Setup(x => x.CreateUpdateUnits(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UnitsBodyDto>()))
            .Returns(unit);

        var result = _controller.Put("testUnitId", "testUserId", new UnitsBodyDto()) as ActionResult<UnitsDto>;

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
