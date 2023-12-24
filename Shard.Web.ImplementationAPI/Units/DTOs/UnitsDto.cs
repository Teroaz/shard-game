using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsDto
{
    public string Id { get; init; }

    public string Type { get; init; }

    public string System { get; init; }

    public string? Planet { get; init; }

    public string? DestinationSystem { get; init; }

    public string? DestinationPlanet { get; init; }

    public string? EstimatedArrivalTime { get; init; }

    public Dictionary<string, int>? ResourcesQuantity { get; set; }

    public int? Health { get; set; }

    public UnitsDto(UnitModel unitModel)
    {
        Id = unitModel.Id;
        Type = unitModel.Type.ToLowerString();
        System = unitModel.System.Name;
        Planet = unitModel.Planet?.Name;
        DestinationSystem = unitModel.DestinationSystem?.Name;
        DestinationPlanet = unitModel.DestinationPlanet?.Name;
        EstimatedArrivalTime = unitModel.EstimatedArrivalTime.ToString("yyyy-MM-ddTHH:mm:ss");
        ResourcesQuantity = unitModel.Planet?.ResourceQuantity.ToDictionary(resource => resource.Key.ToString().ToLower(), resource => resource.Value);

        if (unitModel is FightingUnitModel fightingUnitModel)
        {
            Health = fightingUnitModel.Health;
        }

        if (unitModel is CargoUnitModel cargo)
        {
            ResourcesQuantity = cargo.ResourcesQuantity?.ToDictionary(resource => resource.Key.ToString().ToLower(), resource => resource.Value);
        }
    }
}