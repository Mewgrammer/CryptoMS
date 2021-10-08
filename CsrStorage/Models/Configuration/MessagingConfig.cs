namespace CsrStorage.Models.Configuration;

public class MessagingConfig
{
    public string BootstrapServers { get; set; }
    public string ClientId { get; set; }
    
    public string SchemaRegistryUrl { get; set; }
}