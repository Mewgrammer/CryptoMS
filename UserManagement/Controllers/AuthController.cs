using Contracts.Communication.Commands;
using Contracts.Communication.Contracts;
using Contracts.Communication.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.DTO;
using UserInfoResponse = Contracts.Communication.Contracts.UserInfoResponse;

namespace UserManagement.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<UserInfoResponse> GetUserInfo()
    {
        return await _mediator.Send(new UserInfoQuery());
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<UserTokenResponse> Login([FromBody] UserRequest userRequest)
    {
        var command = new LoginUserCommand
        {
            UserName = userRequest.Name,
            Password = userRequest.Password
        };
        return await _mediator.Send(command);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<UserTokenResponse> Register([FromBody] UserRequest userRequest)
    {
        var command = new RegisterUserCommand
        {
            UserName = userRequest.Name,
            Password = userRequest.Password
        };
        return await _mediator.Send(command);
    }
}