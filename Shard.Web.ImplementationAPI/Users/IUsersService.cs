using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUserService
{
    Boolean IsIdConsistant(string id);
    Boolean IsBodyValid(string id, UserBodyDto? userBody);
    UserDto CreateUpdateUser(string id, UserBodyDto userBody);
    UserDto? GetUserById(string id);
}