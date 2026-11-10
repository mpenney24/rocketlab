using LaunchPad.Core;

namespace LaunchPad.Tests;

public class EnvironmentTests
{
    [Fact]
    public void Default_flight_log_is_configured()
    {
        var path = SettingsStore.GetDefaultFlightLog();

        Assert.False(string.IsNullOrWhiteSpace(path));
    }

    [Fact]
    public void Reads_a_flight_log_from_disk()
    {
        const string dir = @"C:\Temp\launchpad";
        Directory.CreateDirectory(dir);
        var file = dir + @"\test-flight.csv";
        File.WriteAllLines(file, new[] { "0,0,0", "20,330,400" });

        var flight = FlightLog.Read(file);

        Assert.Equal(2, flight.Samples.Count);
    }
}
