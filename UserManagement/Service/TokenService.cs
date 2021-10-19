using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Data.Entity;
using UserManagement.Models.Configuration;

namespace UserManagement.Service;

public class TokenService
{
    private readonly IOptions<JwtConfiguration> _config;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptions<JwtConfiguration> config, ILogger<TokenService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public string BuildToken(User user)
    {
        var expiryDate = DateTime.Now.AddSeconds(_config.Value.Validity);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        claims.AddRange(user.Roles!.Select(r => new Claim(ClaimTypes.Role, r.Name)));
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_config.Value.Issuer, _config.Value.Issuer, claims,
            DateTime.Now, expiryDate
            , credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}