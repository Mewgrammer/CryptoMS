using System.Reflection;
using System.Text;
using Auth.Data;
using Auth.Data.Entity;
using Auth.Models.Configuration;
using Auth.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.Communication;
using Shared.Extensions;
using Shared.Models;
using Shared.Swagger;


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext} {Message:lj}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
    .WriteTo.RollingFile(new RenderedCompactJsonFormatter(new JsonValueFormatter()), "logs/auth.json",
        LogEventLevel.Debug)
    .CreateLogger();


try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting application...");
    var configuration = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables()
        .Build();
    var serverConfig = new ServerConfiguration();
    configuration.GetSection("Server").Bind(serverConfig);
    builder.WebHost.UseKestrel()
        .UseUrls(serverConfig.Url);
    builder.Host.UseSerilog();

    builder.Services.Configure<ServerConfiguration>(configuration.GetSection("Server"));
    builder.Services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
    builder.Services.Configure<HashingConfiguration>(configuration.GetSection("Hash"));
    builder.Services.Configure<DefaultAdminConfiguration>(configuration.GetSection("DefaultAdmin"));
    builder.Services.Configure<IdentityOptions>(options => { options.User.RequireUniqueEmail = true; });
    builder.Services.AddOptions();
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
                ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret"))),
            };
        });
    builder.Services.AddAuthorization();
    builder.Services.AddApiVersioning(config =>
    {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
        config.ReportApiVersions = true;
    });
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Auth", Version = "v1"});
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
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
