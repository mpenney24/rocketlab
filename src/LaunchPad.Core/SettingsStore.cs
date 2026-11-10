using Microsoft.Win32;

namespace LaunchPad.Core;

public static class SettingsStore
{
    private const string KeyPath = @"Software\RocketLab\LaunchPad";
    private const string DefaultFlightLog = @"C:\LaunchPad\flights\first-flight.csv";

    /// <summary>Which flight log to analyze when none is given. Each user can change it in the registry.</summary>
    public static string GetDefaultFlightLog()
    {
        using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
        return key?.GetValue("FlightLog") as string ?? DefaultFlightLog;
    }
}
