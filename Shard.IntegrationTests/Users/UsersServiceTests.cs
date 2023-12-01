using Moq;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Users;

public class UsersServiceTests
{
    private readonly Mock<IUsersRepository> _mockUsersRepo = new();
    private readonly Mock<ISystemsService> _mockSystemsService = new();
    private readonly Mock<IUnitsRepository> _mockUnitsRepo = new();
    private readonly Mock<ICommon> _mockCommon = new();
    private readonly UsersService _service;

    public UsersServiceTests()
    {
        _service = new UsersService(_mockUsersRepo.Object, _mockCommon.Object, _mockSystemsService.Object, _mockUnitsRepo.Object);
    }

    [Fact]
    public void GetUserById_ShouldReturnUser()
    {
        var user = new UserModel("1", "testUser");
        _mockUsersRepo.Setup(repo => repo.GetUserById("1")).Returns(user);

        var result = _service.GetUserById("1");

        Assert.Equal(user, result);
    }

    [Fact]
    public void IsBodyValid_ShouldReturnFalseForNullUserBody()
    {
        var result = _service.IsBodyValid("1", null);
        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_ShouldReturnFalseForMismatchedId()
    {
        var result = _service.IsBodyValid("1", new UserBodyDto { Id = "2", Pseudo = "testUser" });
        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_ShouldReturnFalseForInvalidPseudo()
    {
        var result = _service.IsBodyValid("1", new UserBodyDto { Id = "1", Pseudo = "" });
        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_ShouldReturnTrueForValidBody()
    {
        _mockCommon.Setup(common => common.IsIdConsistant("1", "^[a-zA-Z0-9_-]+$")).Returns(true);

        var result = _service.IsBodyValid("1", new UserBodyDto { Id = "1", Pseudo = "testUser" });

        Assert.True(result);
    }

    [Fact]
    public void CreateUser_ShouldThrowExceptionIfUserExists()
    {
        var user = new UserModel("1", "testUser");
        _mockUsersRepo.Setup(repo => repo.GetUserById("1")).Returns(user);

        Assert.Throws<Exception>(() => _service.CreateUser(user));
    }

    [Fact]
    public void CreateUser_ShouldThrowExceptionIfSystemNotFound()
    {
        var user = new UserModel("1", "testUser");
        _mockUsersRepo.Setup(repo => repo.GetUserById("1")).Returns((UserModel)null);
        _mockSystemsService.Setup(service => service.GetRandomSystem()).Returns((SystemModel)null);

        Assert.Throws<Exception>(() => _service.CreateUser(user));
    }

    //[Fact]
    // public void CreateUser_ShouldAddUserAndUnits()
    // {
    //     var user = new UserModel("1", "testUser");
    //     var system = _mockSystemsService.Object.GetRandomSystem();
    //
    //     _mockUsersRepo.Setup(repo => repo.GetUserById("1")).Returns((UserModel)null);
    //     _mockSystemsService.Setup(service => service.GetRandomSystem()).Returns((SystemModel)null);
    //
    //     _service.CreateUser(user);
    //
    //     _mockUsersRepo.Verify(repo => repo.AddUser(user), Times.Once);
    //     _mockUnitsRepo.Verify(repo => repo.AddUnit(user, It.Is<UnitModel>(unit => unit.Type == UnitType.Scout)), Times.Once);
    //     _mockUnitsRepo.Verify(repo => repo.AddUnit(user, It.Is<UnitModel>(unit => unit.Type == UnitType.Builder)), Times.Once);
    // }


    [Fact]
    public void UpdateUser_ShouldThrowExceptionIfUserNotFound()
    {
        var user = new UserModel("1", "testUser");
        _mockUsersRepo.Setup(repo => repo.GetUserById("1")).Returns((UserModel)null);

        Assert.Throws<Exception>(() => _service.UpdateUser("1", user));
    }

    [Fact]
    public void UpdateUser_ShouldUpdatePseudo()
    {
        var user = new UserModel("1", "original");
        var updatedUser = new UserModel("1", "updated");

        _mockUsersRepo.Setup(repo => repo.GetUserById("1")).Returns(user);

        _service.UpdateUser("1", updatedUser);

        Assert.Equal(updatedUser.Pseudo, user.Pseudo);
    }
}