using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Model;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUserService
{
    Boolean IsBodyValid(string id, UserBodyDto? userBody);
    UserDto CreateUpdateUser(string id, UserBodyDto userBody);
    UserDto? GetUserById(string id);
}

public class UsersService : IUserService
{
    
    private readonly IUserRepository _userRepository; 
    
    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public UserDto? GetUserById(string id)
    {
        var user = _userRepository.GetUserById(id);
        
        return user == null ? null : new UserDto(user);
    }
    
    public Boolean IsBodyValid(string id, UserBodyDto? userBody)
    {
        if (userBody == null || string.IsNullOrWhiteSpace(userBody.Id) || id != userBody.Id)
        {
            return false;
        }

        var regex = new Regex("^[a-zA-Z0-9_-]+$");
        
        return !regex.IsMatch(id);
    }

    public UserDto CreateUpdateUser(string id, UserBodyDto userBody)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
        {
            user = new UserModel(userBody.Id, userBody.Pseudo, DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
            _userRepository.AddUser(user);
        }
        
        return new UserDto(user);
    }
    
}