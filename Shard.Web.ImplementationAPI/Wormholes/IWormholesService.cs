using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Wormholes;

public interface IWormholesService
{
    public WormholeData? GetWormholeByShardName(string shard);

    public Task<string> Jump(UserModel user, UnitModel unit, string shard);
}