using Contracts.Communication.Commands;
using Contracts.Communication.Contracts;
using MediatR;
using UserManagement.Service;

namespace UserManagement.Communication;

public class LoginUserRequestHandler : IRequestHandler<LoginUserCommand, UserTokenResponse>
{
    private readonly AuthService _authService;
    private readonly TokenService _tokenService;

    public LoginUserRequestHandler(AuthService authService, TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    public async Task<UserTokenResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.LoginUserAsync(command);
        return new UserTokenResponse(await _tokenService.BuildTokenAsync(user));
    }
}