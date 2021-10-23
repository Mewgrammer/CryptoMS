using Contracts.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions.AspNetCore;

namespace Contracts.Extensions;

public static class ApplicationExtensions
{
    public static void EnsureDatabaseCreated<T>(this WebApplication app) where T : DbContext {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.EnsureCreated();
    } 
    
    public static void MigrateDatabase<T>(this WebApplication app) where T : DbContext {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        var migrator = dbContext.Database.GetService<IMigrator>();
        migrator.Migrate();
    }

    public static void MigrateDatabase<TContext, TInitializer>(this WebApplication app)
        where TContext : DbContext
        where TInitializer : DbInitializer
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        var migrator = dbContext.Database.GetService<IMigrator>();
        migrator.Migrate();
        var dbInitializer = scope.ServiceProvider.GetService<TInitializer>();
        dbInitializer?.Seed();

    } 
    
    public static ControllerActionEndpointConventionBuilder MapControllersWithHttpExceptions(this WebApplication app)
    {
        app.UseHttpExceptions();
        return app.MapControllers();
    }
    
    public static IApplicationBuilder MapControllersWithHttpExceptions(this IApplicationBuilder app)
    {
        return app.UseAuthorization()
            .UseAuthentication();
    }

    
    
}