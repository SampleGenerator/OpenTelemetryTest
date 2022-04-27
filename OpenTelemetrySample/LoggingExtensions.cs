using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace OpenTelemetrySample;

public static class LoggingExtensions
{
    public static IServiceCollection AddAppLogging(
        this IServiceCollection services,
        ILoggingBuilder loggingBuilder,
        ResourceBuilder resourceBuilder
    )
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri("http://localhost:4317");
            });
            options.AddConsoleExporter();
        });

        services.Configure<OpenTelemetryLoggerOptions>(opt =>
        {
            opt.IncludeScopes = true;
            opt.ParseStateValues = true;
            opt.IncludeFormattedMessage = true;
        });

        return services;
    }
}
