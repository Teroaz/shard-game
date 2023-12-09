using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public abstract class FightingUnitModel : UnitModel
{
    public int Health { get; set; }
    protected abstract int AttackDamage { get; }
    protected abstract int AttackPeriod { get; }

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

    public async Task StartCombat(IClock clock, IUnitsService unitsService)
    {
        var priorityTargetTypes = GetPriorityTargetTypes();
        
        while (Health > 0)
        {
            await clock.Delay(TimeSpan.FromSeconds(AttackPeriod - clock.Now.Second % AttackPeriod));

            var possibleTargets = unitsService.GetFightingUnits().Where(u => u.User.Id != User.Id);

            if (Planet != null)
            {
                possibleTargets = possibleTargets
                    .Where(u => u.Planet != null && u.Planet.Name == Planet.Name);
            }
            else
            {
                possibleTargets = possibleTargets
                    .Where(u => u.System.Name == System.Name);
            }

            var target = possibleTargets.ToArray().MinBy(u => priorityTargetTypes.IndexOf(u.Type));

            if (target == null) break;

            target.TakeDamage(this, AttackDamage);

            if (target.Health <= 0)
            {
                unitsService.RemoveUnit(target.User, target);
            }
        }
    }

    protected abstract void TakeDamage(FightingUnitModel damageFromUnit, int damage);

    protected abstract List<UnitType> GetPriorityTargetTypes();
}