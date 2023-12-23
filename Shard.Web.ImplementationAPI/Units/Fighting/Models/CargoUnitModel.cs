using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Units.Fighting.Models;

public class CargoUnitModel : FightingUnitModel
{
    
    public Dictionary<ResourceKind, int> ResourcesQuantity { get; set; }
    
    protected override int AttackDamage => UnitFightingDetails.Cargo.AttackDamage;
    public override int AttackPeriod => UnitFightingDetails.Cargo.AttackPeriod;
    public override UnitType Type => UnitType.Cargo;

    public CargoUnitModel(UserModel user, SystemModel system, PlanetModel? planet, Dictionary<ResourceKind, int>? resourcesQuantity)
        : this(Guid.NewGuid().ToString(), user, system, planet)
    { 
        ResourcesQuantity = resourcesQuantity ?? new Dictionary<ResourceKind, int>();
    }

    public CargoUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet, Dictionary<ResourceKind, int>? resourcesQuantity = null)
        : base(id, user, system, planet, UnitFightingDetails.Cargo.InitialHealth)
    {
        ResourcesQuantity = resourcesQuantity ?? new Dictionary<ResourceKind, int>();
    }

    protected override void TakeDamage(FightingUnitModel damageFromUnit, int damage)
    {
        Health -= damage;
    }

    protected override List<UnitType> GetPriorityTargetTypes()
    {
        return new List<UnitType> {};
    }
    
    public Dictionary<ResourceKind, int> LoadUnloadResources(Dictionary<ResourceKind, int> newResources)
    {
        Dictionary<ResourceKind, int> resourcesToLoadUnload = new Dictionary<ResourceKind, int>();
        foreach (var resource in newResources)
        {
            try
            {
                if (ResourcesQuantity[resource.Key] > 0)
                {
                    int difference = resource.Value - ResourcesQuantity[resource.Key];
                    resourcesToLoadUnload[resource.Key] = difference;
                }
                else
                {
                    resourcesToLoadUnload[resource.Key] = resource.Value;
                }
            }
            catch (KeyNotFoundException)
            {
                resourcesToLoadUnload[resource.Key] = resource.Value;
            }
        }
        return resourcesToLoadUnload;
    }

}