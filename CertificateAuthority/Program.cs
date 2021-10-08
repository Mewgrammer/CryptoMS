using CertificateAuthority.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using X509.Certificate;
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

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "CA", Version = "v1"}); });

var app = builder
    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoMS v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();