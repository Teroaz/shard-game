using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<IReadOnlyList<SystemDto>> Get()
    {
        var systems = _systemsService.GetAllSystems();
        return systems.Select(s => new SystemDto(s)).ToList();
    }

    [HttpGet("{systemName}")]
    public ActionResult<SystemDto> Get(string systemName)
    {
        var system = _systemsService.GetSystem(systemName);
        if (system == null) return NotFound("System not found");
        
        return new SystemDto(system);
    }

    [HttpGet("{systemName}/planets")]
    public ActionResult<List<PlanetDto>> GetPlanets(string systemName)
    {
        var system = _systemsService.GetSystem(systemName);
        if (system == null) return NotFound("System not found");
        
        return system.Planets.Select(p => new PlanetDto(p)).ToList();
    }

    [HttpGet("{systemName}/planets/{planetName}")]
    public ActionResult<PlanetDto> GetPlanet(string systemName, string planetName)
    {
        var system = _systemsService.GetSystem(systemName);
        var planet = system?.Planets.FirstOrDefault(planet => planet.Name == planetName);
        if (planet == null) return NotFound("Planet not found");
        
        return new PlanetDto(planet);
    }
}