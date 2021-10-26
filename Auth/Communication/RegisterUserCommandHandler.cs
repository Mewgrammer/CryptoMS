using Auth.Service;
using MediatR;
using Shared.Communication.Commands;
using Shared.Communication.Responses;

namespace Auth.Communication;

public class RegisterUserRequestHandler : IRequestHandler<RegisterUserCommand, UserTokenResponse>
{
    private readonly AuthService _authService;
    private readonly TokenService _tokenService;

    public RegisterUserRequestHandler(AuthService authService, TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    public async Task<UserTokenResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterUserAsync(request);
        return new UserTokenResponse(await _tokenService.BuildTokenAsync(user));
    }
}
