namespace UserManagement.Models.Configuration;

public class JwtConfiguration
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public IEnumerable<string> AllowedAudiences { get; set; }
    public long Validity { get; set; }
}