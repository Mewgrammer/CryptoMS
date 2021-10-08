namespace CsrProcessor.Models.Configuration;

public class MessagingConfig
{
    public string? BootstrapServers { get; set; }
    public string? GroupId { get; set; }
    public string? ClientId { get; set; }
    public IEnumerable<string>? Topics { get; set; }

}