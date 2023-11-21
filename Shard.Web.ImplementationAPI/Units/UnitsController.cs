using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.Web.ImplementationAPI.Units;

[ApiController]
public class UnitsController : ControllerBase
{
    private readonly IUnitsService _unitsService;

    private readonly IUserService _userService;

    private readonly ISystemsService _systemsService;

    public UnitsController(IUnitsService unitsService, IUserService userService, ISystemsService systemsService)
    {
        _unitsService = unitsService;
        _userService = userService;
        _systemsService = systemsService;
    }

    [HttpGet("/users/{userId}/Units")]
    public ActionResult Get(string userId)
    {
        var user = _userService.GetUserById(userId);

        if (user == null) return NotFound();

        var units = _unitsService.GetUnitsByUser(userId);

        return Ok(units);
    }


    [HttpGet("/users/{userId}/Units/{unitId}")]
    public ActionResult<UnitsDto> Get(string unitId, string userId)
    {
        var unit = _unitsService.GetUnitByIdAndUser(unitId, userId);

        if (unit == null) return NotFound();

        return Ok(new UnitsDto(unit));
    }

    [HttpGet("/users/{userId}/Units/{unitId}/location")]
    public ActionResult<UnitsLocationDto> GetLocation(string unitId, string userId)
    {
        var unit = _unitsService.GetUnitByIdAndUser(unitId, userId);

        if (unit == null) return NotFound();

        var system = _systemsService.GetSystem(unit.System);
        var planet = system?.Planets.FirstOrDefault(planet => planet.Name == unit.Planet);
        var resourceQuantity = planet?.ResourceQuantity;
        return Ok(new UnitsLocationDto(unit, resourceQuantity));
    }

    [HttpPut("/users/{userId}/Units/{unitId}")]
    public ActionResult<UnitsDto> Put(string unitId, string userId, [FromBody] UnitsBodyDto unitsBodyDto)
    {
        var isValid = _unitsService.IsBodyValid(unitId, userId, unitsBodyDto);

        if (!isValid) return BadRequest();

        var user = _userService.GetUserById(userId);

        if (user == null) return NotFound();

        var unit = _unitsService.CreateUpdateUnits(unitId, userId, unitsBodyDto);
        return Ok(new UnitsDto(unit));
    }
}