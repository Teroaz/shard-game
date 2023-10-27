using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.IntegrationTests.Users;

public class UsersRepositoryTests
{
    [Fact]
    public void AddUser_ShouldAddUserToList()
    {
        var repo = new UsersRepository();
        var user = new UserModel("1", "testUser");

        repo.AddUser(user);

        var retrievedUser = repo.GetUserById("1");
        Assert.NotNull(retrievedUser);
        Assert.Equal(user, retrievedUser);
    }

    [Fact]
    public void GetUserById_ShouldRetrieveUserById()
    {
        var repo = new UsersRepository();
        var user = new UserModel("1", "testUser");
        repo.AddUser(user);

        var retrievedUser = repo.GetUserById("1");

        Assert.NotNull(retrievedUser);
        Assert.Equal("1", retrievedUser.Id);
    }
}