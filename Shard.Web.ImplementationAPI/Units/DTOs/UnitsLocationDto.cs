using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsLocationDto
{
    public string System {get; init; }

    public string Planet {get; init; }
    
    public IReadOnlyDictionary<ResourceKind, int>? ResourcesQuantity { get; } 

    public UnitsLocationDto(UnitsModel unitsModel, IReadOnlyDictionary<ResourceKind, int>? resourceQuantity)
    {
        System = unitsModel.System;
        Planet = unitsModel.Planet;
        ResourcesQuantity = resourceQuantity ?? new Dictionary<ResourceKind, int>();
    }
    
}