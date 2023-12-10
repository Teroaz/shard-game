using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Users.Dtos;

public class UserBodyDto
{
    public string Id { get; init; }
    
    public string Pseudo { get; init; }
    
    public Dictionary<ResourceKind, int>? ResourcesQuantity { get; init; }
}