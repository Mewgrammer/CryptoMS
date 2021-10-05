using Microsoft.Extensions.DependencyInjection;

namespace Certificate;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddX509Certificate(this IServiceCollection services)
    {
        return services;
    } 
}