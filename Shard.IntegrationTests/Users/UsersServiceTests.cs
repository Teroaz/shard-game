using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Users;

public class UsersServiceTests
{
    private readonly Mock<IUsersRepository> _mockUsersRepository;
    private readonly Mock<ISystemsService> _mockSystemsService;
    private readonly Mock<ICommon> _mockCommon;
    private readonly UsersService _service;

    private const string TestUserId = "testUserId";
    private readonly UserModel _expectedUserModel;

    private const string TestSeed = "testSeed";
    private readonly SystemSpecification _systemSpecification;

    public UsersServiceTests()
    {
        _mockUsersRepository = new Mock<IUsersRepository>();
        _mockSystemsService = new Mock<ISystemsService>();
        Mock<IUnitsRepository> mockUnitsRepository = new();

        var options = new MapGeneratorOptions { Seed = TestSeed };
        var mapGenerator = new MapGenerator(options);
        _systemSpecification = mapGenerator.Generate().Systems[0];

        _mockCommon = new Mock<ICommon>();
        _service = new UsersService(_mockUsersRepository.Object, _mockCommon.Object, _mockSystemsService.Object,
            mockUnitsRepository.Object);

        var now = DateTimeOffset.Now;

        _expectedUserModel = new UserModel(TestUserId, "testPseudo");
    }

    // [Fact]
    // public void GetUserById_UserExists_ReturnsExpectedUserModel()
    // {
    //     _mockUsersRepository.Setup(r => r.GetUserById(TestUserId)).Returns(_expectedUserModel);
    //
    //     var result = _service.GetUserById(TestUserId);
    //
    //     Assert.Equal(_expectedUserModel, result);
    // }
    //
    // [Fact]
    // public void GetUserById_UserDoesNotExist_ReturnsNull()
    // {
    //     _mockUsersRepository.Setup(r => r.GetUserById(TestUserId)).Returns((UserModel)null);
    //
    //     var result = _service.GetUserById(TestUserId);
    //
    //     Assert.Null(result);
    // }
    //
    // [Fact]
    // public void IsBodyValid_BodyIsValid_ReturnsTrue()
    // {
    //     _mockCommon.Setup(c => c.IsIdConsistant(TestUserId, "^[a-zA-Z0-9_-]+$")).Returns(true);
    //
    //     var result = _service.IsBodyValid(TestUserId, new UserBodyDto { Id = TestUserId, Pseudo = "testPseudo" });
    //
    //     Assert.True(result);
    // }
    //
    // [Fact]
    // public void IsBodyValid_BodyIsNotValid_ReturnsFalse()
    // {
    //     _mockCommon.Setup(c => c.IsIdConsistant(TestUserId, "^[a-zA-Z0-9_-]+$")).Returns(false);
    //
    //     var result = _service.IsBodyValid(TestUserId, new UserBodyDto { Id = "testAnotherId", Pseudo = "testPseudo" });
    //
    //     Assert.False(result);
    // }
    //
    // [Fact]
    // public void CreateUpdateUser_UserDoesNotExist_CreatesUserAndReturnsDto()
    // {
    //     var userBodyDto = new UserBodyDto { Id = TestUserId, Pseudo = "testPseudo" };
    //     _mockUsersRepository.Setup(r => r.GetUserById(TestUserId)).Returns((UserModel)null);
    //     _mockSystemsService.Setup(s => s.GetAllSystems()).Returns(new List<SystemModel>
    //     {
    //         new(_systemSpecification)
    //     });
    //
    //     var result = _service.CreateUpdateUser(TestUserId, userBodyDto);
    //
    //     Assert.NotNull(result);
    //     Assert.Equal(TestUserId, result.Id);
    //     Assert.Equal(userBodyDto.Pseudo, result.Pseudo);
    // }
    //
    // [Fact]
    // public void CreateUpdateUser_UserExists_UpdatesUserAndReturnsDto()
    // {
    //     var userBodyDto = new UserBodyDto { Id = TestUserId, Pseudo = "newPseudo" };
    //     var existingUser =
    //         new UserModel(TestUserId, "oldPseudo", DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
    //     _mockUsersRepository.Setup(r => r.GetUserById(TestUserId)).Returns(existingUser);
    //
    //     var result = _service.CreateUpdateUser(TestUserId, userBodyDto);
    //
    //     Assert.NotNull(result);
    //     Assert.Equal(TestUserId, result.Id);
    //     Assert.Equal(userBodyDto.Pseudo, result.Pseudo);
    // }
}