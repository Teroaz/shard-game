using Microsoft.AspNetCore.Mvc;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _userService;

    public UsersController(IUsersService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetUser(string id)
    {
        var user = _userService.GetUserById(id);
        return user == null ? NotFound() : Ok(new UserDto(user));
    }

    [HttpPut("{id}")]
    public ActionResult<UserDto> PutUser(string id, [FromBody] UserBodyDto userBody)
    {
        var isValid = _userService.IsBodyValid(id, userBody, HttpContext.User.IsInRole(Roles.Admin));
        if (!isValid) return BadRequest();

        var user = _userService.GetUserById(id);

        if (user == null)
        {
            user = new UserModel(userBody.Id, userBody.Pseudo);
            _userService.CreateUser(user);
        }
        else
        {
            user = new UserModel(userBody.Id, userBody.Pseudo);
            _userService.UpdateUser(id, user);
        }
        
        

        return Ok(new UserDto(user));
    }
}