using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings.DTOs;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Buildings;

[ApiController]
[Route("users/{userId}/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingsService _buildingsService;
    private readonly IUsersService _usersService;
    private readonly IUnitsService _unitsService;
    private readonly IClock _clock;

    public BuildingsController(IBuildingsService buildingsService, IUsersService usersService,
        IUnitsService unitsService, IClock clock)
    {
        _buildingsService = buildingsService;
        _usersService = usersService;
        _unitsService = unitsService;
        _clock = clock;
    }

    [HttpPost]
    public ActionResult<BuildingDto> CreateBuilding(string userId, [FromBody] CreateBuildingBodyDto createBuildingBodyDto)
    {
        var user = _usersService.GetUserById(userId);
        if (user == null) return NotFound();

        if (createBuildingBodyDto.BuilderId == null || createBuildingBodyDto.Type == null ||
            createBuildingBodyDto.Id == null)
        {
            return BadRequest();
        }

        if (!createBuildingBodyDto.Type.IsValidEnumValue<BuildingType>()) return BadRequest();
        if (createBuildingBodyDto.ResourceCategory != null && !createBuildingBodyDto.ResourceCategory.IsValidEnumValue<BuildingResourceCategory>()) return BadRequest();

        var unit = _unitsService.GetUnitByIdAndUser(user, createBuildingBodyDto.BuilderId);
        if (unit?.Planet == null) return BadRequest();

        var building = new BuildingModel(
            createBuildingBodyDto.Id, 
            user, 
            createBuildingBodyDto.Type.ToEnum<BuildingType>(),
            createBuildingBodyDto.ResourceCategory?.ToEnum<BuildingResourceCategory>(),
            unit.System, 
            unit.Planet
            );

        _buildingsService.AddBuilding(user, building);

        building.ConstructionTask = building.StartConstruction(_clock);
        
        return new BuildingDto(building);
    }

    [HttpGet]
    public ActionResult<List<BuildingDto>> GetBuildings(string userId)
    {
        var user = _usersService.GetUserById(userId);
        if (user == null) return NotFound();

        var buildings = _buildingsService.GetBuildingsByUser(user);

        return buildings.Select(building => new BuildingDto(building)).ToList();
    }

    [HttpGet("{buildingId}")]
    public async Task<ActionResult<BuildingDto>> Get(string userId, string buildingId)
    {
        var user = _usersService.GetUserById(userId);

        if (user == null) return NotFound();

        var building = _buildingsService.GetBuildingByIdAndUser(user, buildingId);

        if (building == null) return NotFound();

        var now = _clock.Now;
        var estimatedBuildingTime = building.EstimatedBuildTime ?? now;

        var timeLeft = estimatedBuildingTime - now;

        if (timeLeft.TotalSeconds > 2)
        {
            return new BuildingDto(building);
        }

        if (timeLeft.TotalSeconds is >= 0 and <= 2)
        {
            try
            {
                if (building.ConstructionTask != null)
                {
                    await building.ConstructionTask;
                }
            }
            catch (TaskCanceledException)
            {
                return NotFound();
            }

            building = _buildingsService.GetBuildingByIdAndUser(user, buildingId);
            // If building has been moved during the 2sec delay
            if (building == null) return NotFound();

            return new BuildingDto(building);
        }

        return new BuildingDto(building);
    }
    
    [HttpPost("{starportId}/queue")]
    public ActionResult<UnitModel> AddToQueue(string userId, string starportId, [FromBody] QueueDto queue)
    {
        var user = _usersService.GetUserById(userId);

        if (user == null)
        {
            return NotFound();
        }

        var buildings = _buildingsService.GetBuildingsByUser(user);;
        var starport = buildings.Find(building => building.Id == starportId);

        if (starport == null)
        {
            return NotFound();
        }

        if (starport.IsBuilt != true)
        {
            return BadRequest();
        }

        if(starport.Type != BuildingType.Starport)
        {
            return BadRequest();
        }

        try
        {
            var type = queue.Type.ToEnum<UnitType>();
            starport.AddToQueue(type, user);
            var unit = new UnitModel(queue.Type.ToEnum<UnitType>(), starport.System, starport.Planet);
            _unitsService.AddUnit(user, unit);
            return Ok(unit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}