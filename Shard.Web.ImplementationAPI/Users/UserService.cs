using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Model;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUserService
{
    Boolean IsBodyValid(string id, UserBodyDto? userBody);
    UserDto? UpdateUser(string id, UserBodyDto userBody);
}

public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository; 
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Boolean IsBodyValid(string id, UserBodyDto? userBody)
    {
        if (userBody == null || string.IsNullOrWhiteSpace(userBody.Id) || id != userBody.Id)
        {
            return false;
        }

        // Regular expression to check for alphanumerical characters, underscores, and dashes
        return new Regex("^[a-zA-Z0-9_-]+$").IsMatch(id);
    }

    public UserDto? UpdateUser(string id, UserBodyDto userBody)
    {
        var user = _userRepository.GetUserById(id);

        if (user == null)
        {
            return null;
        }
        
        return new UserDto(user);
    }
    
}