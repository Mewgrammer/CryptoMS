using CsrProcessor.Messaging;

namespace CsrProcessor.Services;

public class CsrProcessingService
{
    private readonly CsrConsumer _csrConsumer;
    private readonly ILogger<CsrProcessingService> _logger;

    public CsrProcessingService(CsrConsumer csrConsumer, ILogger<CsrProcessingService> logger)
    {
        _csrConsumer = csrConsumer;
        _logger = logger;
    }

    public void Start()
    {
        _logger.LogInformation("Csr Processor starting...");
        _csrConsumer.Start();
    }

    public void Stop()
    {
        _logger.LogInformation("Csr Processor stopping...");
        _csrConsumer.Stop();
    }
}