using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;

namespace Shard.Web.ImplementationAPI.Models;

public class BuildingModel
{
    public string Id { get; set; }
    public UserModel User { get; set; }
    public BuildingType Type { get; set; }
    public BuildingResourceCategory ResourceCategory { get; set; }
    public SystemModel System { get; set; }
    public PlanetModel Planet { get; set; }
    public bool IsBuilt { get; set; }
    public DateTime EstimatedBuildTime { get; set; }
    public Task? ConstructionTask { get; private set; }
    
    public BuildingModel(string id, UserModel user, BuildingType type, BuildingResourceCategory resourceCategory, SystemModel system, PlanetModel planet)
    {
        Id = id;
        User = user;
        Type = type;
        ResourceCategory = resourceCategory;
        System = system;
        Planet = planet;
        IsBuilt = false;
        EstimatedBuildTime = DateTime.Now.Add(BuildingConstructionTime.TimeToBuild);
    }
    
    public void StartConstruction(IClock clock)
    {
        // ConstructionTask = new Task(EstimatedBuildTime, () => IsBuilt = true, clock);
    }

    public ResourceKind GetResourceToMine(PlanetModel planetModel)
    {
        // switch (ResourceCategory)
        // {
        //     case BuildingResourceCategory.Solid:
        //         var solidResource = planetModel.ResourceQuantity.First();
        // }
        
        return ResourceKind.Aluminium;
    }
}