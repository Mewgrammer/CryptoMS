using Contracts.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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

    public static IServiceCollection AddMicroserviceAuthentication(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = config.GetValue<string>("Auth:Authority");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidAudiences = config.GetValue<IEnumerable<string>>("Auth:AllowedAudiences"),
                    ValidIssuers = config.GetValue<IEnumerable<string>>("Auth:AllowedIssuers")
                };
            });
        services.AddAuthorization(c => c.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());
        return services;
    }
}