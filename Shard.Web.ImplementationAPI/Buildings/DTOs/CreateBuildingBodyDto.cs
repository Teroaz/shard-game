namespace Shard.Web.ImplementationAPI.Buildings.DTOs;

public class CreateBuildingBodyDto
{
    public string Id { get; }
    public string? Type { get; }
    public string? BuilderId { get; }
    public string? ResourceCategory { get; }

    public CreateBuildingBodyDto(string? id, string type, string builderId, string? resourceCategory)
    {
        Id = id ?? Guid.NewGuid().ToString();
        Type = type;
        BuilderId = builderId;
        ResourceCategory = resourceCategory;    
    }
}