using AmbientZero.Api.Auth;
using AmbientZero.Api.Extensions;
using AmbientZero.Data;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ServicesDependencies>(builder.Configuration.GetSection("ServicesDependencies"));
builder.Services.Configure<BatteryInfo>(builder.Configuration.GetSection("BatteryInfo"));
builder.Services.Configure<Secrets>(builder.Configuration.GetSection("Secrets"));

builder.Services.AddServices();
builder.Services.AddRepositories();

using var tempProvider = builder.Services.BuildServiceProvider();
var startupModule = tempProvider.GetRequiredService<IStartupModule>();

startupModule.AddDbContext(builder.Services);

builder.Services.AddScoped<IAuthorizationHandler, ProtectionHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});

startupModule.AddAuthentication(builder.Services);
startupModule.SetResponseCompression(builder.Services);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Protected", policy =>
        policy.Requirements.Add(new ProtectionRequirement()));

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LoginLimit", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(5),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));
});

var app = builder.Build();

startupModule.ValidateMigrations(app.Services);

if (args.Contains("--migrate"))
{    
    return;
}

app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseRateLimiter();
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
