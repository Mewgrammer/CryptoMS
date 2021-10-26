using Shared.Communication.Messaging;

namespace CsrProcessor.Models.Configuration;

public class MessagingConfig
{
    public string Host { get; set; }
    public QueueConfig Queue { get; set; }
}