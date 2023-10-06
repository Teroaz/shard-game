namespace Shard.Web.ImplementationAPI.Model;

public class UserModel
{
    public string Id { get; init; }
    
    public string Pseudo { get; init; }
    
    public string DateOfCreation { get; init; }
    
    public UserModel(string id, string pseudo, string dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
    }
}