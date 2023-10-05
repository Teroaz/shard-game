using Shard.Web.ImplementationAPI.Model;

namespace Shard.Web.ImplementationAPI.Users;

public interface IUserRepository
{
    void AddUser(UserModel userModel);
    UserModel? GetUserById(string id);
}

public class UserRepository : IUserRepository
{ 
    List<UserModel> Users { get; init; }
    
    public UserRepository()
    {
        Users = new List<UserModel>();
    }
    
    // TODO add method to populate 2/3 users for testing
    
    public void AddUser(UserModel userModel)
    {
        Users.Add(userModel);
    }
    
    public UserModel? GetUserById(string id)
    {
        return Users.FirstOrDefault(user => user.Id == id);
    }
}