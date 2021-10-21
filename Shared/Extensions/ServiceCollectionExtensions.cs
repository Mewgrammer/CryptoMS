using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Opw.HttpExceptions.AspNetCore;

namespace Contracts.Extensions;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddControllersWithHttpExceptions(this IServiceCollection services)
    {
        return services.AddControllers()
            .AddHttpExceptions(options =>
            {
                options.IncludeExceptionDetails = context =>
                    context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
            });
    }
}