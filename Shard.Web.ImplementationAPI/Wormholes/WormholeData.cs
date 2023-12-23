namespace Shard.Web.ImplementationAPI.Wormholes;

public class WormholeData
{
    public string BaseUri { get; set; }
    public string System { get; set; }
    public string User { get; set; }
    public string SharedPassword { get; set; }

    public WormholeData(string baseUri, string system, string user, string sharedPassword)
    {
        BaseUri = baseUri;
        System = system;
        User = user;
        SharedPassword = sharedPassword;
    }
}