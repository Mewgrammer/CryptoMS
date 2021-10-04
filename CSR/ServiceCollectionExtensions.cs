using Microsoft.Extensions.DependencyInjection;

namespace X509.CSR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddX509Csr(this IServiceCollection services)
    {
        services.AddSingleton<CsrService>();
        return services;
    } 
}