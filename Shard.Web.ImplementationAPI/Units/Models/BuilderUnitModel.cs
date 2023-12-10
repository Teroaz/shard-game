using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Units.Models;

public class BuilderUnitModel : UnitModel
{
    public override UnitType Type => UnitType.Builder;

    public BuilderUnitModel(UserModel user, SystemModel system, PlanetModel? planet)
        : base(user, system, planet)
    {
    }

    public BuilderUnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet)
        : base(id, user, system, planet)
    {
    }
}