using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings.DTOs;
using Shard.Web.ImplementationAPI.Buildings.Models;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;
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

        if (createBuildingBodyDto.BuilderId == null || createBuildingBodyDto.Type == null) return BadRequest();

        if (!createBuildingBodyDto.Type.IsValidEnumValue<BuildingType>()) return BadRequest();
        if (createBuildingBodyDto.ResourceCategory != null && !createBuildingBodyDto.ResourceCategory.IsValidEnumValue<BuildingResourceCategory>()) return BadRequest();

        var unit = _unitsService.GetUnitByIdAndUser(user, createBuildingBodyDto.BuilderId);
        if (unit?.Planet == null) return BadRequest();

        var buildingType = createBuildingBodyDto.Type.ToEnum<BuildingType>();
        if (buildingType == BuildingType.Mine && createBuildingBodyDto.ResourceCategory == null) return BadRequest();
        var resourceCategory = createBuildingBodyDto.ResourceCategory?.ToEnum<BuildingResourceCategory>();

        BuildingModel building = buildingType switch
        {
            BuildingType.Starport => new StarportBuildingModel(createBuildingBodyDto.Id, user, unit.System, unit.Planet),
            BuildingType.Mine => new MineBuildingModel(createBuildingBodyDto.Id, user, unit.System, unit.Planet, (BuildingResourceCategory)resourceCategory!),
            _ => throw new ArgumentOutOfRangeException()
        };

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
    public ActionResult<UnitsDto> AddToQueue(string userId, string starportId, [FromBody] QueueDto queue)
    {
        var user = _usersService.GetUserById(userId);
        if (user == null) return NotFound();

        var building = _buildingsService.GetBuildingByIdAndUser(user, starportId);
        if (building == null) return NotFound();
        if (!building.IsBuilt || building is not StarportBuildingModel starport) return BadRequest();

        if (!queue.Type.IsValidEnumValue<UnitType>()) return BadRequest();

        var queueUnitType = queue.Type.ToEnum<UnitType>();

        starport.AddToQueue(queueUnitType, user);
        var newUnit = _unitsService.ConstructSpecificUnit(queueUnitType, user, starport.System, starport.Planet);
        _unitsService.AddUnit(user, newUnit);
        return new UnitsDto(newUnit);
    }
}