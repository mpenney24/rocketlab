using System.Globalization;

namespace LaunchPad.Core;

/// <summary>
/// Reads flight logs. A log is a plain text file:
///   time_s,velocity_ms,altitude_m     (header, optional)
///   20,330,400                        (one reading per line)
///   ABORT,engine pressure low         (the reason is optional)
/// Lines we can't understand are radio dropouts: we skip them and count them.
/// </summary>
public static class FlightLog
{
    public static FlightRecord Read(string path)
    {
        var reader = new StreamReader(path);
        var lines = new List<string>();
        while (reader.ReadLine() is { } line)
        {
            lines.Add(line);
        }
        return Parse(lines);
    }

    public static FlightRecord Parse(IEnumerable<string> lines)
    {
        var flight = new FlightRecord();

        foreach (var line in lines)
        {
            if (line.Trim() == "" || line.StartsWith("time_s"))
            {
                continue;
            }

            if (line.StartsWith("ABORT"))
            {
                var parts = line.Split(',', 2);
                flight.Aborted = true;
                flight.AbortReason = parts.Length > 1 ? parts[1] : null;
            }
            else if (TryParseSample(line, out var sample))
            {
                flight.Samples.Add(sample);
                Interlocked.Increment(ref MissionStats.SamplesRead);
            }
            else
            {
                Interlocked.Increment(ref MissionStats.Dropouts);
            }
        }

        return flight;
    }

    private static bool TryParseSample(string line, out FlightSample sample)
    {
        sample = new FlightSample(0, 0, 0);
        var parts = line.Split(',');

        if (parts.Length == 3
            && int.TryParse(parts[0], out var time)
            && double.TryParse(parts[1], CultureInfo.InvariantCulture, out var velocity)
            && double.TryParse(parts[2], CultureInfo.InvariantCulture, out var altitude))
        {
            sample = new FlightSample(time, velocity, altitude);
            return true;
        }

        return false;
    }
}
