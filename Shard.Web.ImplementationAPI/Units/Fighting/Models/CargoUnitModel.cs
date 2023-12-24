using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Units.Fighting.Models;

public class CargoUnitModel : FightingUnitModel
{
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
        return new List<UnitType>();
    }

    public Dictionary<ResourceKind, int> LoadUnloadResources(Dictionary<ResourceKind, int> newResources)
    {
        var resourcesToLoadUnload = new Dictionary<ResourceKind, int>();

        foreach (var resource in newResources)
        {
            var currentQuantity = ResourcesQuantity.TryGetValue(resource.Key, out var quantity) ? quantity : 0;
            var difference = resource.Value - currentQuantity;
            resourcesToLoadUnload[resource.Key] = difference;
        }

        return resourcesToLoadUnload;
    }

    public bool CompareResources(Dictionary<ResourceKind, int> resources)
    {
        if (ResourcesQuantity == null) return false;

        return ResourcesQuantity.Count == resources.Count &&
               ResourcesQuantity.All(kv => resources.TryGetValue(kv.Key, out var value) && value == kv.Value);
    }
}