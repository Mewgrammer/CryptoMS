using System.Reflection;
using CsrProcessor.Messaging;
using CsrProcessor.Services;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.Extensions;
using Shared.Models;
using X509.CSR;
using CsrStorageConfig = CsrProcessor.Models.Configuration.CsrStorageConfig;
using MessagingConfig = CsrProcessor.Models.Configuration.MessagingConfig;

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
    builder.Services.Configure<CsrStorageConfig>(configuration);
    builder.Services.Configure<MessagingConfig>(configuration.GetSection(nameof(CsrStorageConfig.Messaging)));

    builder.Services.AddX509Csr();
    builder.Services.AddSingleton<CsrConsumer>();
    builder.Services.AddSingleton<CsrProcessingService>();

    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    builder.Services.AddControllersWithHttpExceptions();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "CSR Processor", Version = "v1"});
    });

    var app = builder
        .Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSR Processor v1"));
    }

    app.MapControllersWithHttpExceptions();

    var csrProcessing = app.Services.GetService<CsrProcessingService>();
    csrProcessing?.Start();
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}