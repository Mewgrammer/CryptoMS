using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Extensions;
using UserManagement.Models.DTO;
using UserManagement.Service;

namespace UserManagement.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly AuthService _authService;

    public AuthController(TokenService tokenService, AuthService authService)
    {
        _tokenService = tokenService;
        _authService = authService;
    }

    [HttpGet]
    public UserInfoResponse GetUserInfo()
    {
        return new UserInfoResponse
        {
            Name = HttpContext.User.Username(),
            Roles = HttpContext.User.UserRoles(),
            ValidUntil = DateTimeOffset.FromUnixTimeSeconds(
                long.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "exp")?.Value ?? "0")
            ).DateTime
        };
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<TokenResponse> Login([FromBody] UserRequest userRequest)
    {
        var loggedInUser = await _authService.Login(userRequest.Name, userRequest.Password);
        return new TokenResponse
        {
            Token = _tokenService.BuildToken(loggedInUser)
        };
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<TokenResponse> Register([FromBody] UserRequest userRequest)
    {
        var createdUser = await _authService.Register(userRequest);
        return new TokenResponse
        {
            Token = _tokenService.BuildToken(createdUser)
        };
    }
}