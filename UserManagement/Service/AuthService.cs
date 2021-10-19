using Microsoft.EntityFrameworkCore;
using Npgsql;
using Opw.HttpExceptions;
using UserManagement.Data;
using UserManagement.Data.Entity;
using UserManagement.Helpers;
using UserManagement.Models;
using UserManagement.Models.DTO;

namespace UserManagement.Service;

public class AuthService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly UserService _userService;
    private readonly PasswordHasher _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IServiceScopeFactory scopeFactory, UserService userService,
        ILogger<AuthService> logger)
    {
        _scopeFactory = scopeFactory;
        _userService = userService;
        _passwordHasher = new PasswordHasher();
        _logger = logger;
    }

    public async Task<User> Login(string name, string password)
    {
        var user = await _userService.FindOneByName(name);
        if (user == null || !_passwordHasher.Check(user.Password, password).Verified)
        {
            throw new UnauthorizedException("Invalid credentials");
        }
        return user;
    }

    public async Task<User> Register(UserRequest userRequest)
    {
        return await _userService.CreateUser(userRequest.Name, userRequest.Password, new List<string> {UserRoleNames.User});
    }
}