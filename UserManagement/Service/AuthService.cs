using System.Security.Claims;
using Contracts.Communication.Commands;
using Microsoft.AspNetCore.Identity;
using Opw.HttpExceptions;
using UserManagement.Data.Entity;
using UserManagement.Models;
using UserManagement.Models.DTO;

namespace UserManagement.Service;

public class AuthService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IServiceScopeFactory scopeFactory, ILogger<AuthService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    public async Task<User> LoginUserAsync(LoginUserCommand userCredentials)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByNameAsync(userCredentials.UserName);
        if (user == null || !await userManager.CheckPasswordAsync(user, userCredentials.Password))
            throw new UnauthorizedException("invalid credentials");
        return user;
    }

    public async Task<User> RegisterUserAsync(RegisterUserCommand userData)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = new User
        {
            UserName = userData.UserName,
            Email = userData.Email,
        };
        var createUserResult = await userManager.CreateAsync(user, userData.Password);
        if (!createUserResult.Succeeded) throw new BadRequestException(createUserResult.ToString());
        await userManager.AddToRolesAsync(user, new List<string> {UserRoleNames.Guest, UserRoleNames.User});
        await userManager.AddClaimsAsync(user, new[]
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        });
        return user;
    }
}