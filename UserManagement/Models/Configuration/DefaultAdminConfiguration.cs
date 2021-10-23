using UserManagement.Data.Entity;

namespace UserManagement.Models.Configuration;

public class DefaultAdminConfiguration
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = UserRoleNames.Admin;

}