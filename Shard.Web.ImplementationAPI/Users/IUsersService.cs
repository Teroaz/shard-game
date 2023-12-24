using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUsersService
{
    bool IsBodyValid(string id, UserBodyDto? userBody, bool isAdmin = false);
    void CreateUser(UserModel userModel);
    void UpdateUser(string userId, UserModel userModel);
    UserModel? GetUserById(string id);
}