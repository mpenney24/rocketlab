namespace LaunchPad.Core;

/// <summary>One telemetry reading from the rocket.</summary>
public record FlightSample(int TimeSeconds, double VelocityMs, double AltitudeM);

/// <summary>Everything we know about one flight: the readings, and whether it was aborted.</summary>
public class FlightRecord
{
    public List<FlightSample> Samples { get; } = new();
    public bool Aborted { get; set; }

    /// <summary>Why the flight was aborted, if the range safety officer told us.</summary>
    public string? AbortReason { get; set; }
}
