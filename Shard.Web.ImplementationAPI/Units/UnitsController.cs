using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Users;

namespace Shard.Web.ImplementationAPI.Units;

[ApiController]
public class UnitsController : ControllerBase
{
    
    private readonly IUnitsService _unitsService;
    
    private readonly IUserService _userService;
    public UnitsController(IUnitsService unitsService, IUserService userService)
    {
        _unitsService = unitsService;
        _userService = userService;
    }
    
    [HttpGet("/users/{userId}/Units")]
    public ActionResult Get(string userId)
    {
        var user = _userService.GetUserById(userId);

        if (user == null)
        {
            return NotFound("Not found.");
        }
        
        var units = _unitsService.GetUnitsByUser(userId);
        
        return Ok(units);
    }
    
    
    [HttpGet("/users/{userId}/Units/{unitId}")]
    public ActionResult<UnitsDto> Get(string unitId, string userId)
    {
        var unit = _unitsService.GetUnitByIdAndUser(unitId, userId);
        
        if(unit == null)
        {
            return NotFound("Not found.");
        }
        
        return Ok(new UnitsDto(unit));
    }
    
    [HttpGet("/users/{userId}/Units/{unitId}/location")]
    public ActionResult<UnitsLocationDto> GetLocation(string unitId, string userId)
    {
        var unit = _unitsService.GetUnitByIdAndUser(unitId, userId);
        
        if(unit == null)
        {
            return NotFound("Not found.");
        }
        
        return Ok(new UnitsLocationDto(unit));
    }

    [HttpPut("/users/{userId}/Units/{unitId}")]
    public ActionResult<UnitsDto> Put(string unitId, string userId, [FromBody] UnitsBodyDto unitsBodyDto)
    {
        
        var isValid = _unitsService.IsBodyValid(unitId, userId, unitsBodyDto);
        
        if(!isValid)
        {
            return BadRequest("Bad request.");
        }
        
        var user = _userService.GetUserById(userId);

        if (user == null)
        {
            return NotFound("Not found.");
        }
        
        var unit = _unitsService.CreateUpdateUnits(unitId, userId, unitsBodyDto);
        return Ok(new UnitsDto(unit));
    }
    
}