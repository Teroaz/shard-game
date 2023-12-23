using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units.Models;

namespace Shard.Web.ImplementationAPI.Wormholes;

public interface IWormholesService
{
    public KeyValuePair<string, WormholeData> GetShardData(string shard);

    public Task<string> Jump(UserModel user, UnitModel unit, string shard);
}