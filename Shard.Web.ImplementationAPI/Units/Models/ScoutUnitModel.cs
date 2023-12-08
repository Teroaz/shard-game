using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class ScoutUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Scout;
    
    public ScoutUnitModel(SystemModel system, PlanetModel? planet) : base(system, planet)
    {
    }

    public ScoutUnitModel(string id, SystemModel system, PlanetModel? planet) : base(id, system, planet)
    {
    }
    
}