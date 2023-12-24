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
        if (user == null) return NotFound("User not found");

        var units = _unitsService.GetUnitsByUser(user);
        var unitsDtos = units.Select(unit => new UnitsDto(unit)).ToList();

        return unitsDtos;
    }

    [HttpGet("{unitId}")]
    public async Task<ActionResult<UnitsDto>> Get(string userId, string unitId)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound("User not found");

        var unit = _unitsService.GetUnitByIdAndUser(user, unitId);
        if (unit == null) return NotFound("Unit not found");

        var arrival = unit.EstimatedArrivalTime;

        var timeLeft = arrival - _clock.Now;

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
        if (user == null) return NotFound("User not found");

        var unit = _unitsService.GetUnitByIdAndUser(user, unitId);
        if (unit == null) return NotFound("Unit not found");

        return new UnitsLocationDto(unit);
    }

    [Authorize]
    [AllowAnonymous]
    [HttpPut("{unitId}")]
    public async Task<ActionResult<UnitsDto>> Put(string userId, string unitId, [FromBody] UnitsBodyDto unitsBodyDto)
    {
        var user = _userService.GetUserById(userId);
        if (user == null) return NotFound("User not found");

        var isValid = _unitsService.IsBodyValid(unitId, unitsBodyDto);
        if (!isValid) return BadRequest("Invalid body");

        if (!unitsBodyDto.Type.IsValidEnumValue<UnitType>()) return BadRequest("Invalid unit type");
        var unitType = unitsBodyDto.Type.ToEnum<UnitType>();

        var oldUnit = _unitsService.GetUnitByIdAndUser(user, unitId);

        var baseSystem = unitsBodyDto.System != null ? _systemsService.GetSystem(unitsBodyDto.System) : null;
        var basePlanet = baseSystem?.Planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.Planet);
        
        var resourcesQuantity = unitsBodyDto.ResourcesQuantity;

        var newUnit = _unitsService.ConstructSpecificUnit(unitType, user, unitId, baseSystem, basePlanet, resourcesQuantity);
        if (newUnit is FightingUnitModel fightingUnitModel && unitsBodyDto.Health != null)
        {
            fightingUnitModel.Health = unitsBodyDto.Health.Value;
        }

        if (oldUnit == null)
        {
            if (!HttpContext.User.IsInRole(Roles.Admin) && !HttpContext.User.IsInRole(Roles.Shard)) return Unauthorized();

            if (HttpContext.User.IsInRole(Roles.Shard) && HttpContext.User.Identity?.Name != null)
            {
                var wormhole = _wormholesService.GetWormholeByShardName(HttpContext.User.Identity.Name);
                if (wormhole != null)
                {
                    var wormholeSystem = _systemsService.GetSystem(wormhole.System);
                    if (wormholeSystem == null) throw new Exception("Wormhole system is null");
                    newUnit.System = wormholeSystem;
                }
            }

            _unitsService.AddUnit(user, newUnit);

            return new UnitsDto(newUnit);
        }
        
        var destinationShard = unitsBodyDto.DestinationShard;
        if (destinationShard != null)
        {
            var unitUri = await _wormholesService.Jump(user, oldUnit, destinationShard);
            _unitsService.RemoveUnit(user, oldUnit);
            return RedirectPermanentPreserveMethod(unitUri);
        }

        if (unitsBodyDto.DestinationSystem == null) return BadRequest("Destination system is null");
        var destinationSystem = _systemsService.GetSystem(unitsBodyDto.DestinationSystem);
        if (destinationSystem == null) return BadRequest("Destination system not found");

        var planets = _systemsService.GetAllSystems().SelectMany(system => system.Planets);
        var destinationPlanet = planets.FirstOrDefault(planet => planet.Name == unitsBodyDto.DestinationPlanet);

        oldUnit.DestinationSystem = destinationSystem;
        oldUnit.DestinationPlanet = destinationPlanet;

        _unitsService.UpdateUnit(user, oldUnit);

        if (resourcesQuantity != null && basePlanet?.ResourceQuantity.Values.Sum() != resourcesQuantity.Values.Sum())
        {
            if (oldUnit is not CargoUnitModel cargoUnitModel) return BadRequest("Unit is not cargo unit");

            var starport = _buildingsService.GetBuildingsByUser(user)
                .Find(building =>
                    building.Planet.Name == unitsBodyDto.Planet && building.Type == BuildingType.Starport);

            if (starport == null && !cargoUnitModel.CompareResources(resourcesQuantity))
            {
                return BadRequest("No starport on this planet");
            }

            var resources = cargoUnitModel.LoadUnloadResources(resourcesQuantity);

            var isSuccess = user.TrySubtractResources(resources);
            if (!isSuccess) return BadRequest("Not enough resources");

            cargoUnitModel.ResourcesQuantity = resourcesQuantity;
        }

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

        oldUnit.Move(_clock, destinationSystem, destinationPlanet);
        return new UnitsDto(oldUnit);
    }
}