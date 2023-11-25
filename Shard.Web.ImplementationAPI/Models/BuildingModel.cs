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
    public DateTime? EstimatedBuildTime { get; set; }
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

    public async Task StartConstruction(IClock clock)
    {
        EstimatedBuildTime = clock.Now.Add(BuildingConstructionTime.TimeToBuild);

        await clock.Delay(TimeSpan.FromMinutes(5), CancellationTokenSource.Token);

        IsBuilt = true;
        EstimatedBuildTime = null;
        StartExtraction(clock);
    }

    private async Task StartExtraction(IClock clock)
    {
        var initialDelay = TimeSpan.FromMinutes(1);
        var repeatDelay = TimeSpan.FromMinutes(1);
        
        await clock.Delay(initialDelay, CancellationTokenSource.Token);

        while (!CancellationTokenSource.Token.IsCancellationRequested)
        {
            var resourceToMine = GetResourceToMine();

            try
            {
                Mine(resourceToMine);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            await clock.Delay(repeatDelay, CancellationTokenSource.Token);
        }
    }


    private ResourceKind GetResourceToMine()
    {
        var possibleResources = ResourceCategory.GetResourcesKindByCategory();

        if (possibleResources.Count == 0) throw new Exception("No resources to mine");
        if (possibleResources.Count == 1) return possibleResources.First();

        if (ResourceCategory == BuildingResourceCategory.Solid)
        {
            var planetAvailableResourceQuantity = Planet.ResourceQuantity.Where(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
            var targetResource = Planet.ResourceQuantity.First(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
            foreach (var resource in planetAvailableResourceQuantity)
            {
                // Pick the most abundant resource
                if (resource.Value > targetResource.Value)
                {
                    targetResource = resource;
                }

                // If there are multiple resources with the same quantity, pick the one with the highest rarity
                if (resource.Value == targetResource.Value)
                {
                    targetResource = possibleResources.IndexOf(targetResource.Key) > possibleResources.IndexOf(resource.Key) ? resource : targetResource;
                }
            }

            return targetResource.Key;
        }

        throw new Exception("Unhandled resource category");
    }

    private void Mine(ResourceKind resourceKind)
    {
        if (Planet.ResourceQuantity[resourceKind] == 0) return;

        Planet.ResourceQuantity[resourceKind] -= 1;
        User.ResourcesQuantity[resourceKind] += 1;
    }
}