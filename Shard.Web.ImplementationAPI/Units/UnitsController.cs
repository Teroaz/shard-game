using Microsoft.AspNetCore.Mvc;

namespace Shard.Web.ImplementationAPI.Units;

[ApiController]
public class UnitsController : ControllerBase
{
    
    private readonly IUnitsService _unitsService;
    
    public UnitsController(IUnitsService unitsService)
    {
        _unitsService = unitsService;
    }
    
    [HttpGet("/users/{unitId}/Units")]
    public ActionResult Get()
    {
        return Ok();
    }
    
}