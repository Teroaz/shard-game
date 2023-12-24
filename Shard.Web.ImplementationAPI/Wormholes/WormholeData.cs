namespace Shard.Web.ImplementationAPI.Wormholes;

public record WormholeData(string ShardName, string BaseUri, string System, string User, string SharedPassword)
{
    public string ShardName { get; set; } = ShardName;
    public string BaseUri { get; set; } = BaseUri;
    public string System { get; set; } = System;
    public string User { get; set; } = User;
    public string SharedPassword { get; set; } = SharedPassword;
}