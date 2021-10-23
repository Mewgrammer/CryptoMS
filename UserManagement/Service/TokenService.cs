using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using UserManagement.Data.Entity;
using UserManagement.Models.Configuration;

namespace UserManagement.Service;

public class TokenService
{
    private readonly IOptions<JwtConfiguration> _config;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TokenService> _logger;
    private readonly SymmetricSecurityKey _secret;

    public TokenService(IOptions<JwtConfiguration> config, IServiceScopeFactory scopeFactory,
        ILogger<TokenService> logger)
    {
        _config = config;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Secret));
    }

    public async Task<string> BuildTokenAsync(User user)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var expiryDate = DateTime.Now.AddSeconds(_config.Value.Validity);
        var claims = await userManager.GetClaimsAsync(user);
        claims.AddRange(new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        });
        claims.AddRange((await userManager.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));

        var credentials = new SigningCredentials(_secret, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_config.Value.Issuer, _config.Value.Issuer, claims,
            DateTime.Now, expiryDate
            , credentials);
        _logger.LogInformation("built token for user {}", user.Id);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}