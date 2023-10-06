using Shard.Web.ImplementationAPI.Model;

namespace Shard.Web.ImplementationAPI.Users;


public class UsersesRepository : IUsersRepository
{ 
    private readonly List<UserModel> _users = new List<UserModel>();

    public void AddUser(UserModel userModel)
    {
        _users.Add(userModel);
    }
    
    public UserModel? GetUserById(string id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }
}