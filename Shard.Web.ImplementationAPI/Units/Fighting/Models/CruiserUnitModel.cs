using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Units.Fighting.Models;

public class CruiserUnitModel : FightingUnitModel
{
    protected override int AttackDamage => UnitFightingDetails.Cruiser.AttackDamage;
    public override int AttackPeriod => UnitFightingDetails.Cruiser.AttackPeriod;
    public override UnitType Type => UnitType.Cruiser;
    
    public CruiserUnitModel(UserModel user, SystemModel system, PlanetModel? planet)
        : this(Guid.NewGuid().ToString(), user, system, planet)
    {
    }

    public CruiserUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet)
        : base(id, user, system, planet, UnitFightingDetails.Cruiser.InitialHealth)
    {
    }

    protected override void TakeDamage(FightingUnitModel damageFromUnit, int damage)
    {
        Health -= damage;
    }

    protected override List<UnitType> GetPriorityTargetTypes()
    {
        return new List<UnitType> { UnitType.Fighter, UnitType.Cruiser, UnitType.Bomber };
    }
}