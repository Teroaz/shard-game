using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units.DTOs;
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

    public UnitsController(IUnitsService unitsService, IUsersService userService, ISystemsService systemsService, IBuildingsService buildingsService, IClock clock)
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

        return Ok(unitsDtos);
    }

    [HttpGet("{unitId}")]
    public async Task<ActionResult<UnitsDto>> Get(string userId, string unitId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var unit = _unitsService.GetUnitByIdAndUser(user, unitId);
        if (unit == null) return NotFound();

        // if (unit.EstimatedArrivalTime < DateTime.Now) return new UnitsDto(unit);

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

        var resourceQuantity = unit.Planet?.ResourceQuantity;
        if (unit.Type == UnitType.Builder)
        {
            resourceQuantity = null;
        }

        return Ok(new UnitsLocationDto(unit, resourceQuantity));
    }

    [HttpPut("{unitId}")]
    public ActionResult<UnitsDto> Put(string userId, string unitId, [FromBody] UnitsBodyDto unitsBodyDto)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var isValid = _unitsService.IsBodyValid(unitId, unitsBodyDto);
        if (!isValid) return BadRequest();

        var destinationSystem = _systemsService.GetSystem(unitsBodyDto.DestinationSystem);
        var planets = _systemsService.GetAllSystems().SelectMany(system => system.Planets);
        var destinationPlanet = planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.DestinationPlanet);

        if (destinationSystem == null) return NotFound();

        var unit = _unitsService.GetUnitByIdAndUser(user, unitId);

        if (!unitsBodyDto.Type.IsValidEnumValue<UnitType>()) return BadRequest();

        var newUnit = new UnitModel(unitId, unitsBodyDto.Type.ToEnum<UnitType>(), destinationSystem, destinationPlanet);

        if (unit == null)
        {
            _unitsService.AddUnit(user, newUnit);
        }
        else
        {
            var oldUnit = _unitsService.GetUnitByIdAndUser(user, unitId);
            if (oldUnit == null) return NotFound();
            oldUnit.DestinationSystem = destinationSystem;
            oldUnit.DestinationPlanet = destinationPlanet;
            
            _unitsService.UpdateUnit(user, oldUnit);
            oldUnit.Move(_clock, destinationSystem, destinationPlanet);
            
            if (unitsBodyDto.DestinationSystem != unitsBodyDto.System || unitsBodyDto.DestinationPlanet != unitsBodyDto.Planet)
            {
                var userBuildings = _buildingsService.GetBuildingsByUser(user).ToArray();
                foreach (var building in userBuildings)
                {
                    if (building.System.Name != unitsBodyDto.System || building.Planet.Name != unitsBodyDto.Planet) continue;
                    building.CancellationTokenSource.Cancel();
                    _buildingsService.RemoveBuilding(user, building);
                }
            }
            
            newUnit = oldUnit;
        }

        return Ok(new UnitsDto(newUnit));
    }
}