using Shard.Web.ImplementationAPI.Model;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUserRepository
{
    void AddUser(UserModel userModel);
    UserModel? GetUserById(string id);
}

public class UsersRepository : IUserRepository
{ 
    List<UserModel> Users { get; init; }
    
    public UsersRepository()
    {
        Users = new List<UserModel>();
    }

    public void AddUser(UserModel userModel)
    {
        Users.Add(userModel);
    }
    
    public UserModel? GetUserById(string id)
    {
        return Users.FirstOrDefault(user => user.Id == id);
    }
}