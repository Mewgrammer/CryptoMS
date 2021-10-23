using System.Reflection;
using System.Text;
using Contracts.Communication;
using Contracts.Extensions;
using Contracts.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserManagement.Data;
using UserManagement.Data.Entity;
using UserManagement.Models.Configuration;
using UserManagement.Service;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .Build();

builder.WebHost.UseKestrel()
    .UseUrls();

builder.Services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
builder.Services.Configure<HashingConfiguration>(configuration.GetSection("Hash"));
builder.Services.Configure<DefaultAdminConfiguration>(configuration.GetSection("DefaultAdmin"));
builder.Services.Configure<IdentityOptions>(options => { options.User.RequireUniqueEmail = true; });
builder.Services.AddLogging();

builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddTransient<UserDbInitializer>();

builder.Services.AddCommunications(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<UserDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString("UserContext"))
            .UseSnakeCaseNamingConvention()
            .EnableDetailedErrors();
    }
);

builder.Services.AddIdentityCore<User>()
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<UserDbContext>();
builder.Services.AddControllersWithHttpExceptions();
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
            ValidAudiences = configuration.GetValue<IEnumerable<string>>("Jwt:AllowedAudiences"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret"))),
        };
    });
builder.Services.AddAuthorization(c => c.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build());
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
    app.UseDeveloperExceptionPage();
    app.MigrateDatabase<UserDbContext, UserDbInitializer>();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoMS User Management v1"); });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllersWithHttpExceptions();
app.Run();