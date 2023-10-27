using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Xunit;

namespace Shard.IntegrationTests.Users;

public class UsersControllerTests
{
    private readonly Mock<IUsersService> _mockUserService;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockUserService = new Mock<IUsersService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    [Fact]
    public void GetUser_UserExists_ReturnsOkResult()
    {
        // Arrange
        var userId = "123";
        var userModel = new UserModel(userId, "John Doe");
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns(userModel);

        // Act
        var result = _controller.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnValue.Id);
    }

    [Fact]
    public void GetUser_UserDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = "456";
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns((UserModel)null);

        // Act
        var result = _controller.GetUser(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void PutUser_BodyIsInvalid_ReturnsBadRequestResult()
    {
        // Arrange
        var userId = "123";
        var userBody = new UserBodyDto { Id = userId, Pseudo = "New Name" };
        _mockUserService.Setup(s => s.IsBodyValid(userId, userBody)).Returns(false);

        // Act
        var result = _controller.PutUser(userId, userBody);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public void PutUser_UserExists_ReturnsOkResult()
    {
        // Arrange
        var userId = "123";
        var userBody = new UserBodyDto { Id = userId, Pseudo = "New Name" };
        var existingUserModel = new UserModel(userId, "Old Name");
        _mockUserService.Setup(s => s.IsBodyValid(userId, userBody)).Returns(true);
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns(existingUserModel);

        // Act
        var result = _controller.PutUser(userId, userBody);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnValue.Id);
    }

    [Fact]
    public void PutUser_UserDoesNotExist_CreatesUser()
    {
        // Arrange
        var userId = "456";
        var userBody = new UserBodyDto { Id = userId, Pseudo = "New Name" };
        _mockUserService.Setup(s => s.IsBodyValid(userId, userBody)).Returns(true);
        _mockUserService.Setup(s => s.GetUserById(userId)).Returns((UserModel)null);

        // Act
        var result = _controller.PutUser(userId, userBody);

        // Assert
        _mockUserService.Verify(s => s.CreateUser(It.IsAny<UserModel>()), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnValue.Id);
    }
}