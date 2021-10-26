using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using Shared.Models;

namespace Auth.Service;

public class TokenService
{
    private readonly JwtConfiguration _jwtConfig;
    private readonly ServerConfiguration _serverConfig;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TokenService> _logger;
    private readonly SymmetricSecurityKey _secret;

    private const int TokenValiditySeconds = 3600; 

    public TokenService(IOptions<JwtConfiguration> config, IOptions<ServerConfiguration> serverConfig, IServiceScopeFactory scopeFactory,
        ILogger<TokenService> logger)
    {
        _jwtConfig = config.Value;
        _serverConfig = serverConfig.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
    }

    public async Task<string> BuildTokenAsync(User user)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var expiryDate = DateTime.Now.AddSeconds(TokenValiditySeconds);
        var claims = await userManager.GetClaimsAsync(user);
        claims.AddRange(new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        });
        claims.AddRange((await userManager.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));

        var credentials = new SigningCredentials(_secret, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_serverConfig.ServiceName, _serverConfig.ServiceName, claims,
            DateTime.Now, expiryDate
            , credentials);
        _logger.LogInformation("built token for user {}", user.Id);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}