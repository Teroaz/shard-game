using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Units;

namespace Shard.Web.ImplementationAPI.Buildings.Models;

public class StarportBuildingModel : BuildingModel
{
    public override BuildingType Type => BuildingType.Starport;
    
    public List<UnitType>? Queue { get; set; } = new();

    public StarportBuildingModel(string id, UserModel user, SystemModel system, PlanetModel planet) : base(id, user, system, planet)
    {
    }

    protected override void OnConstructionFinished(IClock clock)
    {
    }

    private static Dictionary<ResourceKind, int> GetStarportCosts(UnitType unit)
    {
        var costs = new Dictionary<UnitType, Dictionary<ResourceKind, int>>
        {
            {
                UnitType.Scout,
                new Dictionary<ResourceKind, int>
                {
                    { ResourceKind.Carbon, 5 },
                    { ResourceKind.Iron, 5 }
                }
            },
            {
                UnitType.Builder,
                new Dictionary<ResourceKind, int>
                {
                    { ResourceKind.Carbon, 5 },
                    { ResourceKind.Iron, 10 }
                }
            },
            {
                UnitType.Fighter,
                new Dictionary<ResourceKind, int>
                {
                    { ResourceKind.Iron, 20 },
                    { ResourceKind.Aluminium, 10 }
                }
            },
            {
                UnitType.Cruiser,
                new Dictionary<ResourceKind, int>
                {
                    { ResourceKind.Iron, 30 },
                    { ResourceKind.Titanium, 10 }
                }
            },
            {
                UnitType.Bomber,
                new Dictionary<ResourceKind, int>
                {
                    { ResourceKind.Iron, 60 },
                    { ResourceKind.Gold, 20 }
                }
            }
        };

        return costs[unit];
    }

    public async void AddToQueue(UnitType unitType, UserModel user)
    {
        var resourcesToConsume = GetStarportCosts(unitType);
        if (Queue != null && user.HasEnoughResources(resourcesToConsume))
        {
            Queue.Add(unitType);
            user.ConsumeResources(resourcesToConsume);
        }
        else
        {
            throw new Exception("Not enough resources");
        }
    }
}