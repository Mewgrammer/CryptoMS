using System.Reflection;
using System.Security.Claims;
using Auth.Data.Entity;
using Auth.Models;
using Auth.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Auth.Data;

public class UserDbInitializer : DbInitializer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DefaultAdminConfiguration _options;

    public UserDbInitializer(IOptions<DefaultAdminConfiguration> options, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
    }

    public override async void Seed()
    {
        using var scope = _scopeFactory.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        await SeedRoles(roleManager);

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await SeedUsers(userManager);
    }

    private async Task SeedRoles(RoleManager<UserRole> roleManager)
    {
        foreach (var field in typeof(UserRoleNames)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && f.FieldType == typeof(string)))
        {
            var roleName = field.GetRawConstantValue() as string;
            if (string.IsNullOrEmpty(roleName) || await roleManager.RoleExistsAsync(roleName)) continue;
            var role = new UserRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant()
            };
            if ((await roleManager.CreateAsync(role)).Succeeded)
            {
                await roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, role.Name));
            }
        }
    }

    private async Task SeedUsers(UserManager<User> userManager)
    {
        if (await userManager.FindByEmailAsync(_options.Email) != null) return;
        var user = new User
        {
            UserName = _options.Name,
            NormalizedUserName = _options.Name.ToUpperInvariant(),
            Email = _options.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, _options.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, _options.Role);
        }
    }
}