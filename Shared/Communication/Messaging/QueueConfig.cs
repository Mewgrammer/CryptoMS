namespace Shared.Communication.Messaging;

public class QueueConfig
{
    public string Name { get; set; }
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }

}