using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Users;

public class UsersRepository : IUsersRepository
{
    private readonly List<UserModel> _users = new();

    public void AddUser(UserModel userModel)
    {
        _users.Add(userModel);
    }

    public UserModel? GetUserById(string id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }
}