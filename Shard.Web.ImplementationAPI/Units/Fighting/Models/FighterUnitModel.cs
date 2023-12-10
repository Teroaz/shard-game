using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Units.Fighting.Models;

public class FighterUnitModel : FightingUnitModel
{
    protected override int AttackDamage => UnitFightingDetails.Fighter.AttackDamage;
    public override int AttackPeriod => UnitFightingDetails.Fighter.AttackPeriod;
    public override UnitType Type => UnitType.Fighter;

    public FighterUnitModel(UserModel user, SystemModel system, PlanetModel? planet)
        : this(Guid.NewGuid().ToString(), user, system, planet)
    {
    }

    public FighterUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet)
        : base(id, user, system, planet, UnitFightingDetails.Fighter.InitialHealth)
    {
    }

    protected override void TakeDamage(FightingUnitModel damageFromUnit, int damage)
    {
        Health -= damage;
    }

    protected override List<UnitType> GetPriorityTargetTypes()
    {
        return new List<UnitType> { UnitType.Bomber, UnitType.Fighter, UnitType.Cruiser };
    }
}