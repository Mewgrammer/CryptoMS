using Contracts.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MessagingConfig = CsrProcessor.Models.Configuration.MessagingConfig;

namespace CsrProcessor.Messaging;

public class CsrConsumer
{
    private readonly IOptions<MessagingConfig> _config;
    private readonly ILogger<CsrConsumer> _logger;
    private IModel _channel;
    private EventingBasicConsumer _consumer;

    public event Action<CertificateRequestMessageBody>? CsrAdded;

    public CsrConsumer(IOptions<MessagingConfig> config,
        ILogger<CsrConsumer> logger)
    {
        _config = config;
        _logger = logger;
        Connect();
    }

    private void Connect()
    {
        var factory = new ConnectionFactory {HostName = _config.Value.Host};
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        var queueConfig = _config.Value.Queue;
        _channel.QueueDeclare(queueConfig.Name, queueConfig.Durable, queueConfig.Exclusive,
            queueConfig.AutoDelete);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Registered += (_, _) => { _logger.LogInformation("Consumer registered"); };
        _consumer.Unregistered += (_, _) => { _logger.LogInformation("Consumer un-registered"); };
        _consumer.Received += OnReceive;
        _channel.BasicConsume(queueConfig.Name, true, _consumer);
    }

    private void OnReceive(object? sender, BasicDeliverEventArgs e)
    {
        _logger.LogDebug($"Received CsrAdded message on {e.Exchange}.{e.RoutingKey}");
        var csr = e.GetBody<CertificateRequestMessageBody>();
        CsrAdded?.Invoke(csr);
    }

}