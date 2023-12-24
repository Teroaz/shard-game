using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Buildings.Models;

public class StarportBuildingModel : BuildingModel
{
    public override BuildingType Type => BuildingType.Starport;

    private List<UnitType>? Queue { get; } = new();

    public StarportBuildingModel(string id, UserModel user, SystemModel system, PlanetModel planet) : base(id, user, system, planet)
    {
    }

    protected override void OnConstructionFinished(IClock clock)
    {
    }

    private static Dictionary<ResourceKind, int> GetCosts(UnitType unit)
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
            },
            {
                UnitType.Cargo,
                new Dictionary<ResourceKind, int>
                {
                    { ResourceKind.Carbon, 10 },
                    { ResourceKind.Iron, 10 },
                    { ResourceKind.Gold, 5 }
                }
            }
        };

        return costs[unit];
    }

    public void Add(UnitType unitType, UserModel user)
    {
        if (Queue == null)
        {
            throw new InvalidOperationException("Queue is not initialized.");
        }

        var resourcesToConsume = GetCosts(unitType);

        if (!user.HasEnoughResources(resourcesToConsume))
        {
            throw new InvalidOperationException("Not enough resources.");
        }

        Queue.Add(unitType);
        user.ConsumeResources(resourcesToConsume);
    }


}