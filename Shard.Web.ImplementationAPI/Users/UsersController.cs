using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPut("{id}")]
    public ActionResult<UserDto> PutUser(string id, [FromBody] UserBodyDto userBody)
    {
        var isValid = _userService.IsBodyValid(id, userBody);
        
        if(!isValid)
        {
            return BadRequest("Something went wrong.");
        }
        
        var user = _userService.CreateUpdateUser(id, userBody);
        
        return Ok(user);
    }
}