using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace OpenTelemetrySample;

public static class TracingExtensions
{
    public static IServiceCollection AddAppTracing(
        this IServiceCollection services, 
        ResourceBuilder resourceBuilder
    )
    {
        return services.AddOpenTelemetryTracing(builder => builder
            .SetResourceBuilder(resourceBuilder)
            .SetSampler(new AlwaysOnSampler())
            .AddAspNetCoreInstrumentation(opt =>
            {
                opt.RecordException = true;
                opt.Filter = ctx =>
                {
                    var path = ctx.Request.Path.Value?.ToLower();

                    if (path is null)
                    {
                        return true;
                    }

                    if (path.Contains("swagger") || path.Contains("_framework"))
                    {
                        return false;
                    }

                    return true;
                };

                opt.Enrich = (activity, eventName, rawObject) =>
                {
                    activity.SetTag("traceId", activity.TraceId);
                    activity.SetTag("displayName", activity.DisplayName);

                    if (rawObject is HttpRequest httpRequest)
                    {
                        activity.SetTag("requestProtocol", httpRequest.Protocol);
                    }
                    if (rawObject is HttpResponse httpResponse)
                    {
                        activity.SetTag("responseLength", httpResponse.ContentLength);
                    }
                };
            })
            .AddHttpClientInstrumentation(opt =>
            {
                opt.SetHttpFlavor = true;
                opt.Enrich = (activity, eventName, rawObject) =>
                {
                    if (rawObject is HttpRequestMessage request)
                    {
                        request.Headers.Add("X-Trace-Id", activity.TraceId.ToString());
                        activity.SetTag("requestVersion", request.Version);
                    }

                    activity.SetTag("traceId", activity.TraceId);
                    activity.SetTag("spanId", activity.SpanId);
                    activity.SetTag("displayName", activity.DisplayName);
                };
            })
            .AddGrpcClientInstrumentation(opt =>
            {
                opt.Enrich = (activity, eventName, rawObject) =>
                {
                    if (rawObject is HttpRequestMessage request)
                    {
                        request.Headers.Add("X-Trace-Id", activity.TraceId.ToString());
                        activity.SetTag("requestVersion", request.Version);
                    }

                    activity.SetTag("traceId", activity.TraceId);
                    activity.SetTag("spanId", activity.SpanId);
                    activity.SetTag("displayName", activity.DisplayName);
                };
            })
            .AddSqlClientInstrumentation(opt =>
            {
                opt.EnableConnectionLevelAttributes = true;
                opt.Enrich = (activity, eventName, rawObject) =>
                {
                    activity.SetTag("traceId", activity.TraceId);
                    activity.SetTag("spanId", activity.SpanId);
                    activity.SetTag("displayName", activity.DisplayName);
                };
            })
            //.AddRedisInstrumentation()
            .AddJaegerExporter()
        );
    }
}
