using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUsersService
{
    bool IsBodyValid(string id, UserBodyDto? userBody, bool isAdmin = false);
    void CreateUser(UserModel userModel);
    void UpdateUser(string userId, UserModel userModel);
    UserModel? GetUserById(string id);
}