using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUsersRepository
{
    void AddUser(UserModel userModel);
    UserModel? GetUserById(string id);
}