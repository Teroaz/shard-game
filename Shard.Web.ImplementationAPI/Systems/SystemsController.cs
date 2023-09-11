﻿using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Systems.DTOs;

namespace Shard.Web.ImplementationAPI.Systems;

[ApiController]
[Route("[controller]")]
public class SystemsController : ControllerBase
{
    private readonly ISystemsService _systemsService;

    public SystemsController(ISystemsService systemsService)
    {
        _systemsService = systemsService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SystemDto>), StatusCodes.Status200OK)]
    public ActionResult<List<SystemDto>> Get()
    {
        var systems = _systemsService.GetAllSystems();
        return Ok(systems);
    }

    [HttpGet("{systemName}")]
    [ProducesResponseType(typeof(SystemDto), StatusCodes.Status200OK)]
    public ActionResult<SystemDto> Get(string systemName)
    {
        var system = _systemsService.GetSystem(systemName);
        return system == null ? NotFound() : Ok(system);
    }

    [HttpGet("{systemName}/planets")]
    [ProducesResponseType(typeof(List<PlanetDto>), StatusCodes.Status200OK)]
    public ActionResult<List<PlanetDto>> GetPlanets(string systemName)
    {
        var system = _systemsService.GetSystem(systemName);
        return system == null ? NotFound() : Ok(system.Planets);
    }

    [HttpGet("{systemName}/planets/{planetName}")]
    [ProducesResponseType(typeof(PlanetDto), StatusCodes.Status200OK)]
    public ActionResult<PlanetDto> GetPlanet(string systemName, string planetName)
    {
        var system = _systemsService.GetSystem(systemName);
        var planet = system?.Planets.FirstOrDefault(planet => planet.Name == planetName);
        return planet == null ? NotFound() : Ok(planet);
    }
}