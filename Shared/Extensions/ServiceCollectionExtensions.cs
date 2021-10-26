using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Opw.HttpExceptions.AspNetCore;
using Shared.Models;

namespace Shared.Extensions;

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

    public static IServiceCollection AddBasicJwtAuth(this IServiceCollection services, JwtConfiguration conf
        ,
        bool requireHttpsMetadata = false)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = conf.Issuer,
                    ValidAudience = conf.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf.Secret))
                };
            });
        services.AddAuthorization();
        return services;
    }
}