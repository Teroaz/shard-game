using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class FighterUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Fighter;

    public FighterUnitModel(SystemModel system, PlanetModel? planet) : base(system, planet)
    {
    }

    public FighterUnitModel(string id, SystemModel system, PlanetModel? planet) : base(id, system, planet)
    {
    }
}