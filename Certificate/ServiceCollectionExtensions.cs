using Microsoft.Extensions.DependencyInjection;

namespace X509.Certificate;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddX509Certificate(this IServiceCollection services)
    {
        return services;
    } 
}