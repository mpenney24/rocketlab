using System.Reflection;
using LaunchPad.Core;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

// launchpad --version
if (args.Contains("--version"))
{
    // Mitch - slightly updated the "version" command to give more information than just version number (now it includes branchId, commitSha, timestamp) 
    // Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
    var assembly = Assembly.GetExecutingAssembly();
    var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    Console.WriteLine(!string.IsNullOrWhiteSpace(infoVersion) 
        ? infoVersion 
        : assembly.GetName().Version);
    return 0;
}

// launchpad launch [--fast]   : fly the pretend rocket and stream its telemetry
if (args.Contains("launch"))
{
    int pause = args.Contains("--fast") ? 0 : 1000;

    foreach (var n in new[] { 3, 2, 1 })
    {
        Console.WriteLine($"T-{n} ...");
        Thread.Sleep(pause);
    }
    Console.WriteLine("LIFTOFF!");

    string? stage = null;
    foreach (var s in FlightSimulator.Fly())
    {
        var now = StageTable.StageAt(s.TimeSeconds);
        if (stage != null && now != stage)
        {
            Console.WriteLine($">>> {stage} done. Stage separation! <<<");
        }
        stage = now;

        Console.WriteLine($"T+{s.TimeSeconds,3}s  velocity {s.VelocityMs,5:0} m/s  altitude {s.AltitudeM / 1000,5:0.0} km  [{now}]");
        Thread.Sleep(pause / 5);
    }

    Console.WriteLine("Orbit achieved. Nice flying.");
    return 0;
}

// launchpad analyze [flight.csv]   : debrief a recorded flight
var path = args.Length >= 2 && args[0] == "analyze" ? args[1] : SettingsStore.GetDefaultFlightLog();
if (!File.Exists(path))
{
    Log.Error("Can't find flight log: {Path}", path);
    return 2;
}

var flight = FlightLog.Read(path);
Console.WriteLine(FlightAnalyzer.Summarize(flight));
Console.WriteLine($"Radio dropouts: {MissionStats.Dropouts}");
return 0;
