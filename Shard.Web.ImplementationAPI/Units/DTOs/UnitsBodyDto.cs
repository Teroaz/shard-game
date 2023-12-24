using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsBodyDto
{
    public string Id { get; init; }

    public string Type { get; init; }
    
    public int? Health { get; set; }

    public string? System { get; init; }

    public string? Planet { get; init; }

    public string? DestinationShard { get; set; }
    public string? DestinationSystem { get; init; }

    public string? DestinationPlanet { get; init; }
    public Dictionary<ResourceKind, int>? ResourcesQuantity { get; init; }
}