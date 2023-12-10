using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Buildings.Models;

public abstract class BuildingModel
{
    public string Id { get; set; }
    public UserModel User { get; set; }
    public abstract BuildingType Type { get;}
    public SystemModel System { get; set; }
    public PlanetModel Planet { get; set; }
    public bool IsBuilt { get; set; }
    public DateTime? EstimatedBuildTime { get; set; }
    public Task? ConstructionTask { get; set; }
    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
    
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