namespace Auth.Models.DTO;

public class UserInfoResponse
{
    public string? Name { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public DateTime ValidUntil { get; set; }
}