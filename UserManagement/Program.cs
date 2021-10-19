using System.Reflection;
using System.Text;
using Contracts.Extensions;
using Contracts.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Opw.HttpExceptions.AspNetCore;
using UserManagement.Data;
using UserManagement.Helpers;
using UserManagement.Models.Configuration;
using UserManagement.Service;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
builder.Services.Configure<HashingConfiguration>(configuration.GetSection("Hash"));
builder.Services.Configure<DefaultAdminConfiguration>(configuration.GetSection("DefaultAdmin"));
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<UserService>();

builder.Services.AddLogging();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<UserDbContext>(
    options =>
        options.UseNpgsql(configuration.GetConnectionString("UserContext"))
            .UseSnakeCaseNamingConvention()
            .EnableDetailedErrors()
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllers()
    .AddHttpExceptions(options =>
    {
        options.IncludeExceptionDetails = context =>
            context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
    });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = configuration.GetValue<string>("Jwt:Issuer"),
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret"))),
        };
    });


builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "UserManagement", Version = "v1"});
    c.OperationFilter<VersionParameterOperationFilter>();
    c.DocumentFilter<VersionParameterDocumentFilter>();
});

var app = builder
    .Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase<UserDbContext>();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoMS User Management v1"); });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpExceptions();


app.Run();