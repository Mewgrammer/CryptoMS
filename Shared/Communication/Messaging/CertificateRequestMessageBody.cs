using Newtonsoft.Json;

namespace Shared.Communication.Messaging;

public class CertificateRequestMessageBody
{
    [JsonRequired]
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonRequired]
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonRequired]
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
    
    [JsonRequired]
    [JsonProperty("certificateRequest")]
    public string CertificateRequest { get; set; }
}