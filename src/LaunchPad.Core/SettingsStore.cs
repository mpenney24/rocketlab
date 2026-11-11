using Microsoft.Win32;

namespace LaunchPad.Core;

public static class SettingsStore
{
    private const string KeyPath = @"Software\RocketLab\LaunchPad";
    private const string DefaultFlightLog = @"C:\LaunchPad\flights\first-flight.csv";

    /// <summary>Which flight log to analyze when none is given. Each user can change it in the registry.</summary>
    public static string GetDefaultFlightLog()
    {
        // Mitch - ASSUMPTION: tells us that this software is for Windows machines, so will use Windows runners for the build (for ease) 
        //  and ignore any cross-compilation concerns for now
        using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
        return key?.GetValue("FlightLog") as string ?? DefaultFlightLog;
    }
}
