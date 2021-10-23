namespace Contracts.Models;

public class AuthConfig
{
    public string AuthorityUrl { get; set; }
    public IEnumerable<string> AllowedIssuers { get; set; }
    public IEnumerable<string> AllowedAudiences { get; set; }
}