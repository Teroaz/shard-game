using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class BuilderUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Builder;

    public BuilderUnitModel(SystemModel system, PlanetModel? planet) : base(system, planet)
    {
    }

    public BuilderUnitModel(string id, SystemModel system, PlanetModel? planet) : base(id, system, planet)
    {
    }

}