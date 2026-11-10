using LaunchPad.Core;

namespace LaunchPad.Tests;

public class FlightLogTests
{
    [Fact]
    public void Reads_telemetry_lines()
    {
        var flight = FlightLog.Parse(new[] { "time_s,velocity_ms,altitude_m", "20,330,400", "40,660,1600" });

        Assert.Equal(2, flight.Samples.Count);
        Assert.Equal(660, flight.Samples[1].VelocityMs);
    }

    [Fact]
    public void Skips_radio_dropouts_and_counts_them()
    {
        MissionStats.Reset();

        var flight = FlightLog.Parse(new[] { "20,330,400", "??? static ???", "40,660,1600", "40,," });

        Assert.Equal(2, flight.Samples.Count);
        Assert.Equal(2, MissionStats.Dropouts);
    }

    [Fact]
    public void Understands_aborts()
    {
        var flight = FlightLog.Parse(new[] { "20,330,400", "ABORT,engine pressure low" });

        Assert.True(flight.Aborted);
        Assert.Equal("engine pressure low", flight.AbortReason);
    }
}
