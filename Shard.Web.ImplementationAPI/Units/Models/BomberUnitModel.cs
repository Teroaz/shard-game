using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class BomberUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Bomber;

    public BomberUnitModel(SystemModel system, PlanetModel? planet) : base(system, planet)
    {
    }

    public BomberUnitModel(string id, SystemModel system, PlanetModel? planet) : base(id, system, planet)
    {
    }

}