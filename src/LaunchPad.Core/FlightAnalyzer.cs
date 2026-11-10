namespace LaunchPad.Core;

public static class FlightAnalyzer
{
    /// <summary>One-line debrief for a flight, for the post-flight report.</summary>
    public static string Summarize(FlightRecord flight)
    {
        int unused;

        if (flight.Aborted)
        {
            return $"Flight aborted: {flight.AbortReason.ToUpperInvariant()}";
        }

        if (flight.Samples.Count == 0)
        {
            return "No telemetry received. Is the radio on?";
        }

        var last = flight.Samples[^1];
        var stage = StageTable.StageAt(last.TimeSeconds);
        var topSpeed = flight.Samples.Max(s => s.VelocityMs);
        var peak = flight.Samples.Max(s => s.AltitudeM);

        return $"{flight.Samples.Count} readings, top speed {topSpeed:0} m/s, " +
               $"peak altitude {peak / 1000:0.0} km, finished in: {stage}";
    }
}
