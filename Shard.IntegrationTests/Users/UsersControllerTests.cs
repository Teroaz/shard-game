using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.IntegrationTests.Users;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockService;
    private readonly UsersController _controller;

    private readonly string _testUserId;
    private readonly UserDto _expectedUser;
    private readonly UserBodyDto _userBody;

    public UsersControllerTests()
    {
        _mockService = new Mock<IUserService>();
        _controller = new UsersController(_mockService.Object);

        _testUserId = "testId";
        var user = new UserModel("testUserId", "testPseudo", DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
        _expectedUser = new UserDto(user);
        _userBody = new UserBodyDto();
    }

    [Fact]
    public void GetUser_UserExists_ReturnsOkResultWithUser()
    {
        _mockService.Setup(s => s.GetUserById(_testUserId)).Returns(_expectedUser);

        var result = _controller.GetUser(_testUserId);

        Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(_expectedUser, ((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetUser_UserDoesNotExist_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetUserById(_testUserId)).Returns((UserDto)null);

        var result = _controller.GetUser(_testUserId);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void PutUser_BodyIsValid_ReturnsOkResultWithUser()
    {
        _mockService.Setup(s => s.IsBodyValid(_testUserId, _userBody)).Returns(true);
        _mockService.Setup(s => s.CreateUpdateUser(_testUserId, _userBody)).Returns(_expectedUser);

        var result = _controller.PutUser(_testUserId, _userBody);

        Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(_expectedUser, ((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public void PutUser_BodyIsNotValid_ReturnsBadRequest()
    {
        _mockService.Setup(s => s.IsBodyValid(_testUserId, _userBody)).Returns(false);

        var result = _controller.PutUser(_testUserId, _userBody);

        Assert.IsType<BadRequestResult>(result.Result);
    }
}