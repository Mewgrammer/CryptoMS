using UserManagement.Models;

namespace UserManagement.Data.Entity;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<UserRole>? Roles { get; set; }
}