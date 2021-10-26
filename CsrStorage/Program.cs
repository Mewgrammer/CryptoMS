using System.Reflection;
using CsrStorage.Data;
using CsrStorage.Messaging;
using CsrStorage.Models.Configuration;
using CsrStorage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.Communication;
using Shared.Extensions;
using Shared.Models;
using Shared.Swagger;
using X509.CSR;

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
    builder.Services.AddOptions();
    builder.Services.Configure<CsrStorageConfig>(configuration);
    builder.Services.Configure<MessagingConfig>(configuration.GetSection(nameof(CsrStorageConfig.Messaging)));

    builder.Services.AddSingleton<CsrProducer>();
    builder.Services.AddSingleton<CsrStorageService>();
    builder.Services.AddX509Csr();
    builder.Services.AddCommunications(Assembly.GetExecutingAssembly());
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    builder.Services.AddEntityFrameworkNpgsql()
        .AddDbContext<CsrDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CsrContext"))
                .UseSnakeCaseNamingConvention()
                .EnableDetailedErrors()
        );

    var jwtConfig = new JwtConfiguration();
    configuration.Bind("Jwt", jwtConfig);
    builder.Services.AddControllersWithHttpExceptions();
    builder.Services.AddBasicJwtAuth(jwtConfig);
    builder.Services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
        config.ReportApiVersions = true;
    });
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "CSR Storage", Version = "v1"});
        c.OperationFilter<VersionParameterOperationFilter>();
        c.DocumentFilter<VersionParameterDocumentFilter>();
    });

    var app = builder
        .Build();

    if (app.Environment.IsDevelopment())
    {
        app.MigrateDatabase<CsrDbContext>();
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSR Storage v1"); });
    }

    app.UseBasicJwtAuth();
    app.MapControllersWithHttpExceptions();


    var csrProducer = app.Services.GetService<CsrProducer>(); // Start the Producer
    var csrStorage = app.Services.GetService<CsrStorageService>();
// TODO: REMOVE THIS
    csrStorage?.AddCsr("TEST_CSR");
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