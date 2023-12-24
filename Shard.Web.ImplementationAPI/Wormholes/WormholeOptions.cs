namespace Shard.Web.ImplementationAPI.Wormholes;

public record WormholeOptions
{
    public List<WormholeData> WormholeDatas { get; set; } = new();
}