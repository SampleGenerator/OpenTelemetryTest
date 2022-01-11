using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;

ActivitySource activitySource = new("MyCompany.MyProduct.MyLibrary");

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource("MyCompany.MyProduct.MyLibrary")
            .AddConsoleExporter()
            .Build();

using var activity = activitySource.StartActivity("SayHello");
activity?.SetTag("foo", 1);
activity?.SetTag("bar", "Hello, World!");
activity?.SetTag("baz", new int[] { 1, 2, 3 });
activity?.Stop();