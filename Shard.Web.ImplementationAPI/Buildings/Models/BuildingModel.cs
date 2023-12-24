using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Buildings.Models;

public abstract class BuildingModel
{
    public string Id { get; }
    protected UserModel User { get; }
    public abstract BuildingType Type { get;}
    public SystemModel System { get; }
    public PlanetModel Planet { get; }
    public bool IsBuilt { get; private set; }
    public DateTime? EstimatedBuildTime { get; private set; }
    public Task? ConstructionTask { get; set; }
    public CancellationTokenSource CancellationTokenSource { get; } = new();
    
    protected BuildingModel(string id, UserModel user, SystemModel system, PlanetModel planet)
    {
        Id = id;
        User = user;
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
        OnConstructionFinished(clock);
    }

    protected abstract void OnConstructionFinished(IClock clock);
}