using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units.Models;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsLocationDto
{
    public string System {get; init; }

    public string? Planet {get; init; }
    
    public IReadOnlyDictionary<ResourceKind, int>? ResourcesQuantity { get; } 

    public UnitsLocationDto(UnitModel unitModel)
    {
        System = unitModel.System.Name;
        Planet = unitModel.Planet?.Name;
        ResourcesQuantity = unitModel.Type == UnitType.Scout ? unitModel.Planet?.ResourceQuantity : null;
    }
    
}