using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class ScoutUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Scout;

    public ScoutUnitModel(UserModel user, SystemModel system, PlanetModel? planet)
        : this(Guid.NewGuid().ToString(), user, system, planet)
    {
    }

    public ScoutUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet)
        : base(id, user, system, planet)
    {
    }
}