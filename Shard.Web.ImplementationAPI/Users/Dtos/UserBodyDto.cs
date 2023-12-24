using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Users.Dtos;

public class UserBodyDto
{
    public required string Id { get; init; }

    public required string Pseudo { get; init; }

    public DateTime DateOfCreation { get; init; }
    
    public Dictionary<ResourceKind, int>? ResourcesQuantity { get; init; }
}