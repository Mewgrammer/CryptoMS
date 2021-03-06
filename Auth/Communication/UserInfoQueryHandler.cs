using Auth.Extensions;
using MediatR;
using Shared.Communication.Queries;
using Shared.Communication.Responses;

namespace Auth.Communication;

public class UserInfoQueryHandler : IRequestHandler<UserInfoQuery, UserInfoResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfoQueryHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<UserInfoResponse> Handle(UserInfoQuery query, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var validity = DateTimeOffset.FromUnixTimeSeconds(
            long.Parse(user?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value ?? "0"));
        return Task.FromResult(new UserInfoResponse(user?.Username()!, user?.UserRoles() ?? Array.Empty<string>(), validity.DateTime));
    }
}