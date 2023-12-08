using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;

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

        if (ResourceCategory == BuildingResourceCategory.Solid)
        {
            var planetAvailableResourceQuantity =
                Planet.ResourceQuantity.Where(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
            var targetResource =
                Planet.ResourceQuantity.First(resourceQuantity => possibleResources.Contains(resourceQuantity.Key));
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
                    targetResource =
                        possibleResources.IndexOf(targetResource.Key) > possibleResources.IndexOf(resource.Key)
                            ? resource
                            : targetResource;
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
}