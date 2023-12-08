using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class CruiserUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Cruiser;

    public CruiserUnitModel(SystemModel system, PlanetModel? planet) : base(system, planet)
    {
    }

    public CruiserUnitModel(string id, SystemModel system, PlanetModel? planet) : base(id, system, planet)
    {
    }

}