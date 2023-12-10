using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Units;

[ApiController]
[Route("users/{userId}/[controller]")]
public class UnitsController : ControllerBase
{
    private readonly IUnitsService _unitsService;

    private readonly IUsersService _userService;

    private readonly ISystemsService _systemsService;

    private readonly IBuildingsService _buildingsService;

    private readonly IClock _clock;

    public UnitsController(IUnitsService unitsService, IUsersService userService, ISystemsService systemsService,
        IBuildingsService buildingsService, IClock clock)
    {
        _unitsService = unitsService;
        _userService = userService;
        _systemsService = systemsService;
        _buildingsService = buildingsService;
        _clock = clock;
    }

    [HttpGet]
    public ActionResult<List<UnitsDto>> Get(string userId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var units = _unitsService.GetUnitsByUser(user);
        var unitsDtos = units.Select(unit => new UnitsDto(unit)).ToList();

        return unitsDtos;
    }

    [HttpGet("{unitId}")]
    public async Task<ActionResult<UnitsDto>> Get(string userId, string unitId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var unit = _unitsService.GetUnitByIdAndUser(user, unitId);
        if (unit == null) return NotFound();

        var now = _clock.Now;
        var arrival = unit.EstimatedArrivalTime;

        var timeLeft = arrival - now;

        if (timeLeft.TotalSeconds > 2)
        {
            unit.Planet = null;
            return new UnitsDto(unit);
        }

        if (timeLeft.TotalSeconds is > 0 and <= 2)
        {
            if (unit.MoveTask != null)
            {
                await unit.MoveTask;
            }

            return new UnitsDto(unit);
        }

        return new UnitsDto(unit);
    }

    [HttpGet("{unitId}/location")]
    public ActionResult<UnitsLocationDto> GetLocation(string unitId, string userId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var unit = _unitsService.GetUnitByIdAndUser(user, unitId);
        if (unit == null) return NotFound();

        return new UnitsLocationDto(unit);
    }

    [Authorize]
    [HttpPut("{unitId}")]
    public ActionResult<UnitsDto> Put(string userId, string unitId, [FromBody] UnitsBodyDto unitsBodyDto)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var isValid = _unitsService.IsBodyValid(unitId, unitsBodyDto);
        if (!isValid) return BadRequest();

        if (!unitsBodyDto.Type.IsValidEnumValue<UnitType>()) return BadRequest();

        var baseSystem = _systemsService.GetSystem(unitsBodyDto.System);
        if (baseSystem == null) return NotFound();
        var basePlanet = baseSystem.Planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.Planet);

        var unitType = unitsBodyDto.Type.ToEnum<UnitType>();

        var oldUnit = _unitsService.GetUnitByIdAndUser(user, unitId);

        if (oldUnit == null)
        {
            if (HttpContext.User.IsInRole(Roles.Admin))
            {
                var newUnit = _unitsService.ConstructSpecificUnit(unitType, user, unitId, baseSystem, basePlanet);
                _unitsService.AddUnit(user, newUnit);
                return new UnitsDto(newUnit);
            }
            else
            {
                return Unauthorized();
            }
        }

        var destinationSystem = _systemsService.GetSystem(unitsBodyDto.DestinationSystem);
        var planets = _systemsService.GetAllSystems().SelectMany(system => system.Planets);
        var destinationPlanet = planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.DestinationPlanet);

        oldUnit.DestinationSystem = destinationSystem;
        oldUnit.DestinationPlanet = destinationPlanet;

        _unitsService.UpdateUnit(user, oldUnit);

        if (unitsBodyDto.DestinationSystem != unitsBodyDto.System ||
            unitsBodyDto.DestinationPlanet != unitsBodyDto.Planet)
        {
            var userBuildings = _buildingsService.GetBuildingsByUser(user).ToArray();
            foreach (var building in userBuildings)
            {
                if (building.System.Name != unitsBodyDto.System ||
                    building.Planet.Name != unitsBodyDto.Planet) continue;
                building.CancellationTokenSource.Cancel();
                _buildingsService.RemoveBuilding(user, building);
            }
        }

        oldUnit.Move(_clock, destinationSystem, destinationPlanet);

        return new UnitsDto(oldUnit);
    }
}