using System.Security.Claims;
using UserManagement.Models;

namespace UserManagement.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static IEnumerable<string> UserRoles(this ClaimsPrincipal principal)
    {
        return principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
    }

    public static string? Username(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }
}