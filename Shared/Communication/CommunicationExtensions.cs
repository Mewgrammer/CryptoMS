using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Communication;

public static class CommunicationExtensions
{
    public static IServiceCollection AddCommunications(this IServiceCollection services, params Assembly[] assemblies)
    {
        var allAssemblies = assemblies.ToList();
        allAssemblies.Add(Assembly.GetExecutingAssembly());
        services.AddMediatR(allAssemblies.ToArray());
        services.AddHttpContextAccessor();
        services.AddFluentValidation();
        services.AddValidatorsFromAssemblies(allAssemblies);
        return services;
    }
}