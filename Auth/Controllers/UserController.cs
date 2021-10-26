using Auth.Data.Entity;
using Auth.Extensions;
using Auth.Models;
using Auth.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/users")]
[Authorize(Roles = UserRoleNames.Admin)]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
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
        var toDelete = await _userManager.FindByIdAsync(id.ToString());
        await _userManager.DeleteAsync(toDelete);
    }
}