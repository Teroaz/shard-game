using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Utils;
using Shard.Web.ImplementationAPI.Wormholes;

namespace Shard.Web.ImplementationAPI.Units;

[ApiController]
[Route("users/{userId}/[controller]")]
public class UnitsController : ControllerBase
{
    private readonly IUnitsService _unitsService;

    private readonly IUsersService _userService;

    private readonly ISystemsService _systemsService;

    private readonly IBuildingsService _buildingsService;

    private readonly IWormholesService _wormholesService;

    private readonly IClock _clock;

    public UnitsController(IUnitsService unitsService, IUsersService userService, ISystemsService systemsService,
        IBuildingsService buildingsService, IWormholesService wormholesService, IClock clock)
    {
        _unitsService = unitsService;
        _userService = userService;
        _systemsService = systemsService;
        _buildingsService = buildingsService;
        _wormholesService = wormholesService;
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
    public async Task<ActionResult<UnitsDto>> Put(string userId, string unitId, [FromBody] UnitsBodyDto unitsBodyDto)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound();

        var isValid = _unitsService.IsBodyValid(unitId, unitsBodyDto);
        if (!isValid) return BadRequest("Invalid body");

        if (!unitsBodyDto.Type.IsValidEnumValue<UnitType>()) return BadRequest("Invalid unit type");

        var baseSystem = _systemsService.GetSystem(unitsBodyDto.System);
        // if (baseSystem == null) return NotFound();
        var basePlanet = baseSystem?.Planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.Planet);

        var unitType = unitsBodyDto.Type.ToEnum<UnitType>();
        var resourcesQuantity = unitsBodyDto.ResourcesQuantity;

        var oldUnit = _unitsService.GetUnitByIdAndUser(user, unitId);
        var newUnit = _unitsService.ConstructSpecificUnit(unitType, user, unitId, baseSystem, basePlanet, resourcesQuantity);

        if (oldUnit == null)
        {
            if (HttpContext.User.IsInRole(Roles.Admin) || HttpContext.User.IsInRole(Roles.Shard))
            {
                if (HttpContext.User.IsInRole(Roles.Shard))
                {
                    var shard = _wormholesService.GetShardData(HttpContext.User.Identity.Name);
                    newUnit.System = _systemsService.GetSystem(shard.Value.System);
                }

                _unitsService.AddUnit(user, newUnit);
            }
            else if (HttpContext.User.IsInRole(Roles.User))
            {
                return Unauthorized();
            }
            else
            {
                return Forbid("You are not allowed to create units");
            }

            return new UnitsDto(newUnit);
        }

        var destinationShard = unitsBodyDto.DestinationShard;

        if (destinationShard != null)
        {
            if (!HttpContext.User.IsInRole(Roles.User) && !HttpContext.User.IsInRole(Roles.Admin)) return Forbid("You are not allowed to jump shards");
            
            var unitUri = await _wormholesService.Jump(user, oldUnit, destinationShard);
            _unitsService.RemoveUnit(user, oldUnit);
            return RedirectPermanentPreserveMethod(unitUri);
        }

        var destinationSystem = _systemsService.GetSystem(unitsBodyDto.DestinationSystem);
        var planets = _systemsService.GetAllSystems().SelectMany(system => system.Planets);
        var destinationPlanet = planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.DestinationPlanet);

        oldUnit.DestinationSystem = destinationSystem;
        oldUnit.DestinationPlanet = destinationPlanet;

        _unitsService.UpdateUnit(user, oldUnit);

        if (resourcesQuantity != null)
        {
            if (oldUnit is not CargoUnitModel cargoUnitModel)
            {
                return BadRequest();
            }

            var starport = _buildingsService.GetBuildingsByUser(user)
                .Find(building =>
                    building.Planet.Name == unitsBodyDto.Planet && building.Type == BuildingType.Starport);

            if (starport == null)
            {
                return BadRequest("No starport on this planet");
            }

            var resources = cargoUnitModel.LoadUnloadResources(resourcesQuantity);

            var isSuccess = user.TrySubtractResources(resources);

            if (!isSuccess) return BadRequest("Not enough resources");

            cargoUnitModel.ResourcesQuantity = resourcesQuantity;
        }

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