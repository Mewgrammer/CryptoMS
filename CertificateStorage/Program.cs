using CertificateStorage.Data;
using Contracts.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using X509.Certificate;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddLogging();

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
