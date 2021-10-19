using System.Reflection;
using Contracts.Swagger;
using Microsoft.AspNetCore.Mvc;
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

builder.Services.AddX509Csr();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "RA", Version = "v1"});
    c.OperationFilter<VersionParameterOperationFilter>();
    c.DocumentFilter<VersionParameterDocumentFilter>();
});

var app = builder
    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoMS v1");
    });
}

app.UseAuthorization();
app.MapControllers();


app.Run();