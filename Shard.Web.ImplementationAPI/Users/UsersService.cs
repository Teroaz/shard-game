using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Model;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public class UsersService : IUserService
{
    
    private readonly IUsersRepository _usersRepository; 
    
    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public Boolean IsIdConsistant(string id)
    {
        var regex = new Regex("^[a-zA-Z0-9_-]+$");
        
        return regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id);
    }
    
    public UserDto? GetUserById(string id)
    {
        var user = _usersRepository.GetUserById(id);
        
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
        var user = _usersRepository.GetUserById(id);
        if (user == null)
        {
            user = new UserModel(userBody.Id, userBody.Pseudo, DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
            _usersRepository.AddUser(user);
        }
        
        return new UserDto(user);
    }
    
}