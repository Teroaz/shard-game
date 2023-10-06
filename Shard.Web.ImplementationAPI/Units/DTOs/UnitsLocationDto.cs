using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsLocationDto
{
    public string System {get; init; }

    public string Planet {get; init; }

    public UnitsLocationDto(UnitsModel unitsModel)
    {
        System = unitsModel.System;
        Planet = unitsModel.Planet;
    }
    
}