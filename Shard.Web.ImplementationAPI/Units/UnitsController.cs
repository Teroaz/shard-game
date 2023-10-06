using Microsoft.AspNetCore.Mvc;
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
            return NotFound();
        }
        
        return Ok();
    }

    [HttpPut("/users/{userId}/Units/{unitId}")]
    public ActionResult Put()
    {
        return Ok();
    }
    
}