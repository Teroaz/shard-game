using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _userService;
    private IClock _clock;

    public UsersController(IUsersService userService, IClock clock)
    {
        _userService = userService;
        _clock = clock;
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetUser(string id)
    {
        var user = _userService.GetUserById(id);
        return user == null ? NotFound() : Ok(new UserDto(user));
    }
    
    [Authorize]
    [AllowAnonymous]
    [HttpPut("{id}")]
    public ActionResult<UserDto> PutUser(string id, [FromBody] UserBodyDto userBody)
    {
        var isAdmin = HttpContext.User.IsInRole(Roles.Admin);
        var isValid = _userService.IsBodyValid(id, userBody, isAdmin);
        if (!isValid) return BadRequest();

        var user = _userService.GetUserById(id);

        if (user == null)
        {
            user = new UserModel(userBody.Id, userBody.Pseudo, _clock.Now);
            _userService.CreateUser(user);
        }
        else
        {
            user = new UserModel(userBody.Id, userBody.Pseudo, user.DateOfCreation);

            if (isAdmin && userBody.ResourcesQuantity != null)
            {
                user.ResourcesQuantity = userBody.ResourcesQuantity;
            }
            
            _userService.UpdateUser(id, user);
        }
        
        

        return Ok(new UserDto(user));
    }
}