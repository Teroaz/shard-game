using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Buildings.Models;

public class MineBuildingModel : BuildingModel
{
    public override BuildingType Type => BuildingType.Mine;

    public BuildingResourceCategory ResourceCategory { get; }

    public MineBuildingModel(string id, UserModel user, SystemModel system, PlanetModel planet, BuildingResourceCategory resourceCategory) : base(id, user, system, planet)
    {
        ResourceCategory = resourceCategory;
    }

    protected override void OnConstructionFinished(IClock clock)
    {
        StartExtraction(clock);
    }

    private ResourceKind GetResourceToMine()
    {
        var possibleResources = ResourceCategory.GetResourcesKindByCategory();

        if (possibleResources.Count == 0) throw new Exception("No resources to mine");
        if (possibleResources.Count == 1) return possibleResources.First();

        if (ResourceCategory != BuildingResourceCategory.Solid) throw new Exception("Unhandled resource category");
        var planetAvailableResourceQuantity =
            Planet.ResourceQuantity.Where(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
        var targetResource =
            Planet.ResourceQuantity.First(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
        foreach (var resource in planetAvailableResourceQuantity)
        {
            // If the resource is more abundant than the current target resource, pick it
            if (resource.Value > targetResource.Value)
            {
                targetResource = resource;
            }

            // If there is a tie, pick the resource with the highest index (rarity) in the list
            if (resource.Value == targetResource.Value)
            {
                targetResource =
                    possibleResources.IndexOf(targetResource.Key) > possibleResources.IndexOf(resource.Key)
                        ? resource
                        : targetResource;
            }
        }

        return targetResource.Key;

    }

    private void Mine(ResourceKind resourceKind)
    {
        if (Planet.ResourceQuantity[resourceKind] == 0) return;

        Planet.ResourceQuantity[resourceKind] -= 1;
        User.ResourcesQuantity[resourceKind] += 1;
    }

    private async Task StartExtraction(IClock clock)
    {
        var repeatDelay = TimeSpan.FromMinutes(1);
        
        while (!CancellationTokenSource.Token.IsCancellationRequested)
        {
            await clock.Delay(repeatDelay, CancellationTokenSource.Token);

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
        }
    }
}