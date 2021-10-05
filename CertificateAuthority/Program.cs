using Certificate;
using CertificateAuthority.Data;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddX509Certificate();

builder.Services.AddDbContext<CaContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("CaContext"))
        .UseSnakeCaseNamingConvention());

builder
    .Build()
    .Run();