using Shard.Web.ImplementationAPI.Buildings;

namespace Shard.Web.ImplementationAPI.Models;

public class BuildingModel
{
    public string Id { get; set; }
    public BuildingType Type { get; set; }
    public SystemModel System { get; set; }
    public PlanetModel Planet { get; set; }

    public BuildingModel(string id, BuildingType type, SystemModel system, PlanetModel planet)
    {
        Id = id;
        Type = type;
        System = system;
        Planet = planet;
    }
}