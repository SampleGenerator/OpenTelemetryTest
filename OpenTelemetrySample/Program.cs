using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetrySample;
using OpenTelemetrySample.Entities;
using OpenTelemetrySample.Services.Abstractions;
using OpenTelemetrySample.Services.Implementations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var assemblyVersion = Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString() ?? "unknown";

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("OpenTelemetrySample",
        serviceVersion: assemblyVersion,
        serviceInstanceId: Environment.MachineName
    );

builder.Services.AddAppLogging(builder.Logging, resourceBuilder);
builder.Services.AddAppMetrics(resourceBuilder);
builder.Services.AddAppTracing(resourceBuilder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(cfg =>
{
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IPlaceholderService, PlaceholderService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.UseAppMetrics();

app.Run();