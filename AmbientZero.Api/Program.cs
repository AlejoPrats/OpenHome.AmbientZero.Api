using AmbientZero.Api.AuthHandler;
using AmbientZero.Api.Extensions;
using AmbientZero.Api.ScheduledTasks;
using AmbientZero.Service.Models.Options;
using AmbientZero.Service.Modules;
using Coravel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScheduler();

builder.Services.Configure<ServicesDependencies>(builder.Configuration.GetSection("ServicesDependencies"));
builder.Services.Configure<BatteryInfo>(builder.Configuration.GetSection("BatteryInfo"));
builder.Services.Configure<Secrets>(builder.Configuration.GetSection("Secrets"));

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddScheduledJobs();

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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

builder.Services.AddCustomAuthentication(builder.Configuration["Secrets:TokenEncryptionKey"]);
builder.Services.AddResponseCompression();

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

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<RebootJob>().Daily();
});

app.UseResponseCompression();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();