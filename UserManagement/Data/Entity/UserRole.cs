using UserManagement.Models;

namespace UserManagement.Data.Entity;

public class UserRole
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<User> Users { get; set; }
}