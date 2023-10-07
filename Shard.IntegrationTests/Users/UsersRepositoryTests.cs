using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.IntegrationTests.Users;

public class UsersRepositoryTests
{
    private readonly UsersRepository _repository = new();

    [Fact]
    public void AddUser_NewUser_UserAddedToList()
    {
        var user = new UserModel("testId", "testPseudo", DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));

        _repository.AddUser(user);

        var retrievedUser = _repository.GetUserById("testId");
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Id, retrievedUser.Id);
    }

    [Fact]
    public void GetUserById_UserExists_ReturnsExpectedUser()
    {
        var user = new UserModel("testId2", "testPseudo2", DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
        _repository.AddUser(user);

        var retrievedUser = _repository.GetUserById("testId2");

        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Id, retrievedUser.Id);
        Assert.Equal(user.Pseudo, retrievedUser.Pseudo);
    }

    [Fact]
    public void GetUserById_UserDoesNotExist_ReturnsNull()
    {
        var result = _repository.GetUserById("nonExistentId");
        Assert.Null(result);
    }
}