using System.Diagnostics.Metrics;

namespace OpenTelemetrySample;

public static class Meters
{
    public static Meter SampleMeter { get; set; } = new("Sample Meter", "1.0.0");
}
