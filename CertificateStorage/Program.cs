using Certificate;
using CertificateStorage.Data;
using Microsoft.EntityFrameworkCore;

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

builder
    .Build()
    .Run();