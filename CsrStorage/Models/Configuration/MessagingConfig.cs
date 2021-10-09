using Contracts.Messaging;

namespace CsrStorage.Models.Configuration;

public class MessagingConfig
{
    public string Host { get; set; }
    public QueueConfig Queue { get; set; }
}