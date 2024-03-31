using AmbientZero.Data;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Repositories;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Settings;
using AmbientZero.Service.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AmbientZeroContext>(options => options.UseSqlite("Filename=ambientZero.db"));
builder.Services.Configure<ServicesDependencies>(builder.Configuration.GetSection("ServicesDependencies"));

builder.Services.AddTransient<IMeasurmentService, MeasurmentService>();
builder.Services.AddTransient<ISensorService, SensorService>();
builder.Services.AddTransient<IDeviceInformationService, DeviceInformatonService>();
builder.Services.AddScoped<IMeasurmentLogRepository, MeasurmentLogRepository>();
builder.Services.AddScoped<IDeviceInformationRepository, DeviceInformationRepository>();
builder.Services.AddScoped<ISensorSettingRepository, SensorSettingRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});

var app = builder.Build();

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

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
