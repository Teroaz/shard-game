using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Users;

[ApiController]
[Route("[controller]/{id}")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _userService;
    private readonly IClock _clock;

    public UsersController(IUsersService userService, IClock clock)
    {
        _userService = userService;
        _clock = clock;
    }

    [HttpGet]
    public ActionResult<UserDto> GetUser(string id)
    {
        var user = _userService.GetUserById(id);
        if (user == null) return NotFound("User not found");
        
        return new UserDto(user);
    }

    [Authorize]
    [AllowAnonymous]
    [HttpPut]
    public ActionResult<UserDto> PutUser(string id, [FromBody] UserBodyDto userBody)
    {
        var isAdmin = HttpContext.User.IsInRole(Roles.Admin);
        var isValid = _userService.IsBodyValid(id, userBody, isAdmin);
        if (!isValid) return BadRequest();

        var user = _userService.GetUserById(id);

        if (user == null)
        {
            var isUserShard = HttpContext.User.IsInRole(Roles.Shard);
            if (isUserShard)
            {
                user = new UserModel(userBody.Id, userBody.Pseudo, userBody.DateOfCreation, true);
            }
            else
            {
                user = new UserModel(userBody.Id, userBody.Pseudo, _clock.Now);
            }

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


        return new UserDto(user);
    }
}