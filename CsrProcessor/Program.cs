using System.Reflection;
using Contracts.Extensions;
using CsrProcessor.Messaging;
using CsrProcessor.Services;
using Microsoft.OpenApi.Models;
using X509.CSR;
using CsrStorageConfig = CsrProcessor.Models.Configuration.CsrStorageConfig;
using MessagingConfig = CsrProcessor.Models.Configuration.MessagingConfig;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddLogging();
builder.Services.Configure<CsrStorageConfig>(configuration);
builder.Services.Configure<MessagingConfig>(configuration.GetSection(nameof(CsrStorageConfig.Messaging)));

builder.Services.AddX509Csr();
builder.Services.AddSingleton<CsrConsumer>();
builder.Services.AddSingleton<CsrProcessingService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllersWithHttpExceptions();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "CSR Processor", Version = "v1"}); });

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