using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

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
}