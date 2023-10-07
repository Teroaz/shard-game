using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsDto
{
    public string Id { get; init; }
    
    public string Type { get; init; }
    
    public string System { get; init; }
    
    public string Planet { get; init; }
    
    public UnitsDto (UnitModel unitModel)
    {
        Id = unitModel.Id;
        Type = unitModel.Type;
        System = unitModel.System;
        Planet = unitModel.Planet;
    }
}