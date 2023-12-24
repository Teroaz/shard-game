using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Units.Fighting.Models;

public abstract class FightingUnitModel : UnitModel
{
    public int Health { get; set; }
    protected abstract int AttackDamage { get; }
    public abstract int AttackPeriod { get; }

    public Task? CombatTask { get; set; }

    protected FightingUnitModel(UserModel user, SystemModel system, PlanetModel? planet, int health)
        : this(Guid.NewGuid().ToString(), user, system, planet, health)
    {
    }

    protected FightingUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet, int health)
        : base(id, user, system, planet)
    {
        Health = health;
    }

    public void Combat(List<FightingUnitModel> otherFightingUnits)
    {
        var priorityTargetTypes = GetPriorityTargetTypes();

        var possibleTargets = otherFightingUnits;

        if (Planet != null)
        {
            possibleTargets = possibleTargets
                .Where(u => u.Planet != null && u.Planet.Name == Planet.Name).ToList();
        }
        else
        {
            possibleTargets = possibleTargets
                .Where(u => u.System?.Name == System?.Name).ToList();
        }

        var target = possibleTargets.MinBy(u => priorityTargetTypes.IndexOf(u.Type));

        target?.TakeDamage(this, AttackDamage);
    }

    protected abstract void TakeDamage(FightingUnitModel damageFromUnit, int damage);

    protected abstract List<UnitType> GetPriorityTargetTypes();
}