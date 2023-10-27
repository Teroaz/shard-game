using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Buildings.DTOs;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.Web.ImplementationAPI.Buildings;

[ApiController]
[Route("users/{userId}/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingsService _buildingsService;
    private readonly IUsersService _usersService;
    private readonly IUnitsService _unitsService;

    public BuildingsController(IBuildingsService buildingsService, IUsersService usersService,
        IUnitsService unitsService)
    {
        _buildingsService = buildingsService;
        _usersService = usersService;
        _unitsService = unitsService;
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

        if (!createBuildingBodyDto.Type.IsValidBuildingType()) return BadRequest();

        var unit = _unitsService.GetUnitByIdAndUser(user, createBuildingBodyDto.BuilderId);
        if (unit?.Planet == null) return BadRequest();
        
        var building = new BuildingModel(createBuildingBodyDto.Id, createBuildingBodyDto.Type.ToBuildingType(), unit.System, unit.Planet);

        _buildingsService.AddBuilding(user, building);

        return new BuildingDto(building);
    }
}