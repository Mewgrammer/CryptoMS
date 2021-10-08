using AutoMapper;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using Contracts;
using Contracts.Messages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MessagingConfig = CsrProcessor.Models.Configuration.MessagingConfig;

namespace CsrProcessor.Messaging;

public class CsrConsumer
{
    private readonly IOptions<MessagingConfig> _config;
    private readonly ILogger<CsrConsumer> _logger;
    private readonly IConsumer<Ignore, string> _consumer;
    private bool _running;

    public event Action<CertificateRequestMessageBody>? CsrAdded;

    public CsrConsumer(IOptions<MessagingConfig> config,
        ILogger<CsrConsumer> logger)
    {
        _config = config;
        _logger = logger;
        _consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            {
                BootstrapServers = config.Value.BootstrapServers,
                GroupId = config.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            })
            .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
            .Build();
    }
    
    public void Start(CancellationToken cancellationToken = new())
    {
        if (_running) return;
        _consumer.Subscribe(_config.Value.Topics ?? new[] {Topics.CsrAdded});
        _running = true;
        _logger.LogInformation("Csr Consumer listening...");
        while (_running)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                _logger.LogDebug($"consumed message from topic {consumeResult.Topic}");
                var messageBody = JsonConvert.DeserializeObject<CertificateRequestMessageBody>(consumeResult.Message.Value);
                if (messageBody != null)
                    CsrAdded?.Invoke(messageBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        _consumer.Unsubscribe();
        _logger.LogInformation("Csr Consumer stopped");
    }

    public void Stop()
    {
        _running = false;
    }


}