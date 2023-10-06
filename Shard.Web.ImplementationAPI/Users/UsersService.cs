using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Model;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUserService
{
    Boolean IsIdConsistant(string id);
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

    public Boolean IsIdConsistant(string id)
    {
        var regex = new Regex("^[a-zA-Z0-9_-]+$");
        
        return regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id);
    }
    
    public UserDto? GetUserById(string id)
    {
        var user = _userRepository.GetUserById(id);
        
        return user == null ? null : new UserDto(user);
    }
    
    public Boolean IsBodyValid(string id, UserBodyDto? userBody)
    {
        if (
            userBody == null || 
            id != userBody.Id ||
            string.IsNullOrWhiteSpace(userBody.Pseudo)
            )
        {
            return false;
        }

        return IsIdConsistant(userBody.Id);
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