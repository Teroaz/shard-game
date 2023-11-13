using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsDto
{
    public string Id { get; init; }

    public string Type { get; init; }

    public string System { get; init; }

    public string? Planet { get; init; }

    public string DestinationSystem { get; init; }

    public string? DestinationPlanet { get; init; }

    public string? EstimatedArrivalTime { get; init; }
    
    public Dictionary<ResourceKind, int>? ResourcesQuantity { get; set; }

    public UnitsDto(UnitModel unitModel)
    {
        Id = unitModel.Id;
        Type = unitModel.Type.ToLowerString();
        System = unitModel.System.Name;
        Planet = unitModel.Planet?.Name;
        DestinationSystem = unitModel.DestinationSystem.Name;
        DestinationPlanet = unitModel.DestinationPlanet?.Name;
        EstimatedArrivalTime = unitModel.EstimatedArrivalTime.ToString("yyyy-MM-ddTHH:mm:ss");
        ResourcesQuantity = unitModel.Planet?.ResourceQuantity;
    }

}