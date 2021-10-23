using AutoMapper;
using Contracts;
using Contracts.Communication.Messaging;
using CsrStorage.Data.Entities;
using CsrStorage.Models.Configuration;
using CsrStorage.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CsrStorage.Messaging;

public class CsrProducer
{
    private readonly IMapper _mapper;
    private readonly IOptions<MessagingConfig> _config;
    private readonly ILogger<CsrProducer> _logger;
    private IModel? _channel;

    public CsrProducer(CsrStorageService csrStorageService, IMapper mapper, IOptions<MessagingConfig> config,
        ILogger<CsrProducer> logger)
    {
        _mapper = mapper;
        _config = config;
        _logger = logger;
        Connect();
        csrStorageService.CsrAdded += OnCsrAdded;
        csrStorageService.CsrRemoved += OnCsrRemoved;
    }

    private void Connect()
    {
        var factory = new ConnectionFactory {HostName = _config.Value.Host};
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        var queueConfig = _config.Value.Queue;
        _channel.QueueDeclare(queueConfig.Name, queueConfig.Durable, queueConfig.Exclusive,
            queueConfig.AutoDelete);
    }

    private void OnCsrAdded(CsrEntity csr)
    {
        var messageValue = _mapper.Map<CertificateRequestMessageBody>(csr);
        _channel?.PublishJson("", _config.Value.Queue.Name, messageValue);
        _logger.LogDebug($"Sent CsrAdded message on {Topics.CsrAdded}");
    }

    private void OnCsrRemoved(CsrEntity csr)
    {
        var messageValue = _mapper.Map<CertificateRequestMessageBody>(csr);
        _channel?.PublishJson(Topics.CsrRemoved, "csr-storage", messageValue);
        _logger.LogDebug($"Sent CsrRemoved message on {Topics.CsrRemoved}");
    }
}