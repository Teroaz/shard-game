namespace Shard.Web.ImplementationAPI.Models;

public class UnitsModel
{
    public string Id { get; set; }
    
    public string Type { get; set; }
    
    public string System { get; set; }
    
    public string Planet { get; set; }
    
    public string UserId { get; set; }
    
    public UnitsModel(string id, string type, string system, string planet, string userId)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
        UserId = userId;
    }
}