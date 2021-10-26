namespace Auth.Service;

public class UserService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UserService> _logger;

    public UserService(IServiceScopeFactory scopeFactory, ILogger<UserService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
}