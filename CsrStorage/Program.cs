using System.Reflection;
using Contracts.Communication;
using Contracts.Extensions;
using Contracts.Swagger;
using CsrStorage.Data;
using CsrStorage.Messaging;
using CsrStorage.Models.Configuration;
using CsrStorage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using X509.CSR;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddLogging();
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

builder.Services.AddControllersWithHttpExceptions();
builder.Services.AddMicroserviceAuthentication(configuration.GetSection("Auth"));
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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSR Storage v1");
    });
}

app.MapControllersWithHttpExceptions();


var csrProducer = app.Services.GetService<CsrProducer>(); // Start the Producer
var csrStorage = app.Services.GetService<CsrStorageService>();
// TODO: REMOVE THIS
csrStorage?.AddCsr("TEST_CSR");
app.Run();