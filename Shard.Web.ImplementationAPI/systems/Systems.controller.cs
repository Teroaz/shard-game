using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.systems.dto;

namespace Shard.Web.ImplementationAPI.systems;

[ApiController]
[Route("[controller]")]
public class SystemsController
{
    private readonly SystemsService _systemsService;

    public SystemsController(SystemsService systemsService)
    {
        _systemsService = systemsService;
    }

    [HttpGet]
    public ActionResult<List<SystemDto>> Get()
    {
        var systems = _systemsService.GetAllSystems();
        return new OkObjectResult(systems);
    }

    [HttpGet("{systemName}")]
    public ActionResult<SystemDto> Get(string systemName)
    {
        var system = _systemsService.GetSystem(systemName);
        return system == null ? new NotFoundResult() : new OkObjectResult(system);
    }

    [HttpGet("{systemName}/planets")]
    public ActionResult<List<PlanetDto>> GetPlanets(string systemName)
    {
        var system = _systemsService.GetSystem(systemName);
        return system == null ? new NotFoundResult() : new OkObjectResult(system.Planets);
    }

    [HttpGet("{systemName}/planets/{planetName}")]
    public ActionResult<PlanetDto> GetPlanet(string systemName, string planetName)
    {
        var system = _systemsService.GetSystem(systemName);
        if (system == null)
        {
            return new NotFoundResult();
        }

        var planet = system.Planets.FirstOrDefault(planet => planet.Name == planetName);
        return planet == null ? new NotFoundResult() : new OkObjectResult(planet);
    }
}