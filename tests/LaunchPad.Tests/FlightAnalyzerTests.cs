using LaunchPad.Core;

namespace LaunchPad.Tests;

public class FlightAnalyzerTests
{
    [Fact]
    public void Summarizes_a_good_flight()
    {
        var flight = FlightLog.Parse(new[] { "0,0,0", "100,1650,10000", "????", "200,3300,40000" });

        var summary = FlightAnalyzer.Summarize(flight);

        Assert.Contains("3 readings", summary);
        Assert.Contains("top speed 3300 m/s", summary);
    }

    [Fact]
    public void Reports_the_abort_reason()
    {
        var flight = FlightLog.Parse(new[] { "ABORT,engine pressure low" });

        Assert.Equal("Flight aborted: ENGINE PRESSURE LOW", FlightAnalyzer.Summarize(flight));
    }

    [Fact]
    public void Says_so_when_there_is_no_telemetry()
    {
        Assert.Contains("No telemetry", FlightAnalyzer.Summarize(new FlightRecord()));
    }
}
