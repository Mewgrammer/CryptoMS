using Microsoft.EntityFrameworkCore;
using X509.CSR;
using X509.RegistrationAuthority.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddLogging();

builder.Services.AddX509Csr();

builder.Services.AddDbContext<RaContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("RaContext"))
        .UseSnakeCaseNamingConvention());

builder
    .Build()
    .Run();