using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Units.Fighting.Models;

public class BomberUnitModel : FightingUnitModel
{
    protected override int AttackDamage => UnitFightingDetails.Bomber.AttackDamage;
    public override int AttackPeriod => UnitFightingDetails.Bomber.AttackPeriod;
    public override UnitType Type => UnitType.Bomber;

    public BomberUnitModel(UserModel user, SystemModel system, PlanetModel? planet)
        : this(Guid.NewGuid().ToString(), user, system, planet)
    {
    }

    public BomberUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet)
        : base(id, user, system, planet, UnitFightingDetails.Bomber.InitialHealth)
    {
    }

    protected override void TakeDamage(FightingUnitModel damageFromUnit, int damage)
    {
        if (damageFromUnit.Type == UnitType.Cruiser)
        {
            Health -= damage / 10;
        }
        else
        {
            Health -= damage;
        }
    }

    protected override List<UnitType> GetPriorityTargetTypes()
    {
        return new List<UnitType> { UnitType.Cruiser, UnitType.Bomber, UnitType.Fighter };
    }
}