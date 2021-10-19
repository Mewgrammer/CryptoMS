using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Extensions;
using UserManagement.Models;
using UserManagement.Models.DTO;
using UserManagement.Service;

namespace UserManagement.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/users")]
[Authorize(Roles = UserRoleNames.Admin)]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public UserInfoResponse GetUsers()
    {
        return new UserInfoResponse
        {
            Name = HttpContext.User.Username(),
            Roles = HttpContext.User.UserRoles(),
        };
    }
    
    [HttpDelete("{id}")]
    public async Task DeleteUser(Guid id)
    {
        await _userService.RemoveUser(id);
    }
}