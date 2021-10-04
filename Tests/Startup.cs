using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using X509.CSR;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace Tests;

public class Startup
{

    private IConfigurationRoot _configuration;
    public Startup()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json")
            .Build();
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddX509Csr();
    }
}