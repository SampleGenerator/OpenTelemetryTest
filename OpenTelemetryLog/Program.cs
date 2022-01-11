using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options => options
        .AddConsoleExporter());
});

var logger = loggerFactory.CreateLogger<Program>();
var ex = new Exception("This is a beautiful exception.");
var ex2 = new ArgumentException("This is a beautiful exception.", ex);
logger.LogError(ex2, "Hello from {name} {price}.", "tomato", 2.99);