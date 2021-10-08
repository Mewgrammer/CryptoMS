using AutoMapper;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Contracts;
using Contracts.Messages;
using CsrStorage.Data.Entities;
using CsrStorage.Models.Configuration;
using CsrStorage.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CsrStorage.Messaging;

public class CsrProducer
{
    private readonly IMapper _mapper;
    private readonly ILogger<CsrProducer> _logger;
    private readonly IProducer<Null, string> _producer;

    public CsrProducer(CsrStorageService csrStorageService, IMapper mapper, IOptions<MessagingConfig> config,
        ILogger<CsrProducer> logger)
    {
        _mapper = mapper;
        _logger = logger;
        var schemaRegistry =
            new CachedSchemaRegistryClient(new SchemaRegistryConfig {Url = config.Value.SchemaRegistryUrl});
        _producer = new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = config.Value.BootstrapServers,
                ClientId = config.Value.ClientId
            })
            .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
            .Build();
        csrStorageService.CsrAdded += OnCsrAdded;
        csrStorageService.CsrRemoved += OnCsrRemoved;
    }

    private async void OnCsrAdded(CertificateRequestEntity csr)
    {
        var messageValue = _mapper.Map<CertificateRequestMessageBody>(csr);
        await _producer.ProduceAsync(Topics.CsrAdded,
            new Message<Null, string> {Value = JsonConvert.SerializeObject(messageValue)});
    }

    private async void OnCsrRemoved(CertificateRequestEntity csr)
    {
        var messageValue = _mapper.Map<CertificateRequestMessageBody>(csr);
        var deliveryResult =await _producer.ProduceAsync(Topics.CsrRemoved,
            new Message<Null, string> {Value = JsonConvert.SerializeObject(messageValue)});
        _logger.LogDebug($"Produced message on {deliveryResult.Topic}");
    }
}