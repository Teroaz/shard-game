namespace Shard.Web.ImplementationAPI.Models;

public class UnitsModel
{
    public string Id { get; init; }
    
    public string Type { get; init; }
    
    public string System { get; init; }
    
    public string Planet { get; init; }
    
    public string UserId { get; init; }
    
    public UnitsModel(string id, string type, string system, string planet, string userId)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
        UserId = userId;
    }
}