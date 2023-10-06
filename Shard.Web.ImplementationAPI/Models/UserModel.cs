namespace Shard.Web.ImplementationAPI.Model;

public class UserModel
{
    public string Id { get; set; }
    
    public string Pseudo { get; set; }
    
    public string DateOfCreation { get; set; }
    
    public UserModel(string id, string pseudo, string dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
    }
}