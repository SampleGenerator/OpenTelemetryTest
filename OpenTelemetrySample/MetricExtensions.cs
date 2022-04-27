using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace OpenTelemetrySample;

public static class MetricExtensions
{
    public static IServiceCollection AddAppMetrics(
        this IServiceCollection services, 
        ResourceBuilder resourceBuilder
    )
    {
        return services.AddOpenTelemetryMetrics(options =>
        {
            options.SetResourceBuilder(resourceBuilder)
                .AddMeter(Meters.SampleMeter.Name)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter()
                .AddConsoleExporter()
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri("http://localhost:4317");
                });
        });
    }

    public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder app)
    {
        return app.UseOpenTelemetryPrometheusScrapingEndpoint();
    }
}
