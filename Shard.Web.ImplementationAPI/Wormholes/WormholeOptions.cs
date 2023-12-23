namespace Shard.Web.ImplementationAPI.Wormholes;

public record WormholeOptions
{
    public Dictionary<string, WormholeData> shards { get; set; }
}