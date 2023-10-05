using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPut("{id}")]
    public ActionResult<UserDto> PutUser(string id, [FromBody] UserBodyDto userBody)
    {
        var isValid = _userService.IsBodyValid(id, userBody);
        
        if(!isValid)
        {
            return BadRequest();
        }
        
        var user = _userService.UpdateUser(id, userBody);
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
}