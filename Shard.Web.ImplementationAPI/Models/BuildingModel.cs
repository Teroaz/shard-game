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
    public Task? ConstructionTask { get; set; }
    public CancellationTokenSource CancellationTokenSource { get; set; } = new();

    public BuildingModel(string id, UserModel user, BuildingType type, BuildingResourceCategory resourceCategory, SystemModel system, PlanetModel planet)
    {
        Id = id;
        User = user;
        Type = type;
        ResourceCategory = resourceCategory;
        System = system;
        Planet = planet;
        IsBuilt = false;
    }

    public async void StartConstruction(IClock clock)
    {
        EstimatedBuildTime = clock.Now.Add(BuildingConstructionTime.TimeToBuild);

        await clock.Delay(TimeSpan.FromMinutes(5));
        
        IsBuilt = true;

        var initialDelay = TimeSpan.FromMinutes(1);
        var repeatDelay = TimeSpan.FromMinutes(1);

        ConstructionTask = clock.Delay(initialDelay).ContinueWith(async t =>
        {

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                var resourceCategory = GetResourceToMine();

                try
                {
                    Mine(resourceCategory);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                await clock.Delay(repeatDelay);
            }
        });

        await ConstructionTask;
        ConstructionTask.Start();
    }

    private ResourceKind GetResourceToMine()
    {
        switch (ResourceCategory)
        {
            case BuildingResourceCategory.Solid:
                var possibleResources = ResourceCategory.GetResourcesKindByCategory();
                var targetResource = Planet.ResourceQuantity.First(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
                foreach (var resource in Planet.ResourceQuantity)
                {
                    // Pick the most abundant resource
                    if (resource.Value > targetResource.Value)
                    {
                        targetResource = resource;
                    }

                    // If there are multiple resources with the same quantity, pick the one with the highest rarity
                    if (resource.Value == targetResource.Value)
                    {
                        targetResource = possibleResources.IndexOf(resource.Key) > possibleResources.IndexOf(targetResource.Key) ? resource : targetResource;
                    }
                }

                return targetResource.Key;
            case BuildingResourceCategory.Liquid:
                return ResourceKind.Water;
            case BuildingResourceCategory.Gaseous:
                return ResourceKind.Oxygen;
            default:
                throw new Exception("Unhandled resource category");
        }
    }

    public void Mine(ResourceKind resourceKind)
    {
        if (Planet.ResourceQuantity[resourceKind] == 0) return;

        Planet.ResourceQuantity[resourceKind]--;
        User.ResourcesQuantity[resourceKind]++;
    }
}