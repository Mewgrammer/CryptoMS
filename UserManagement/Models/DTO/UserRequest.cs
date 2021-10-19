using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.DTO;

public class UserRequest
{
    public string Name { get; set; }
    public string Password { get; set; }
}