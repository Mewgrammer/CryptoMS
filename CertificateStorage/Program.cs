using CertificateStorage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.Extensions;
using Shared.Models;
using X509.Certificate;


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext} {Message:lj}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
    .WriteTo.RollingFile(new RenderedCompactJsonFormatter(new JsonValueFormatter()), "logs/auth.json",
        LogEventLevel.Debug)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting application...");
    var configuration = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables()
        .Build();
    var serverConfig = new ServerConfiguration();
    configuration.GetSection("Server").Bind(serverConfig);
    builder.WebHost.UseKestrel()
        .UseUrls(serverConfig.Url);
    builder.Host.UseSerilog();


    builder.Services.AddX509Certificate();

    builder.Services.AddDbContext<CertStoreContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("CertStoreContext"))
            .UseSnakeCaseNamingConvention());

    builder.Services.AddControllersWithHttpExceptions();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Certificate Storage", Version = "v1"});
    });
    var app = builder
        .Build();

    if (app.Environment.IsDevelopment())
    {
        app.MigrateDatabase<CertStoreContext>();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Certificate Storage v1"));
    }

    app.MapControllersWithHttpExceptions();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}