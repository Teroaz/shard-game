namespace Shard.Web.ImplementationAPI.Buildings.DTOs;

public class CreateBuildingBodyDto
{
    public string Id { get; set; }
    public string? Type { get; set; }
    public string? BuilderId { get; set; }
    public string? ResourceCategory { get; set; }

    public CreateBuildingBodyDto(string? id, string type, string builderId, string? resourceCategory)
    {
        Id = id ?? Guid.NewGuid().ToString();
        Type = type;
        BuilderId = builderId;
        ResourceCategory = resourceCategory;    
    }
}