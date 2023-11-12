using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Buildings.DTOs;

public class BuildingDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string System { get; set; }
    public string Planet { get; set; }

    public BuildingDto(BuildingModel buildingModel)
    {
        Id = buildingModel.Id;
        Type = buildingModel.Type.ToLowerString();
        System = buildingModel.System.Name;
        Planet = buildingModel.Planet.Name;
    }
}