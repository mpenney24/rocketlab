namespace LaunchPad.Core;

/// <summary>A very simple pretend rocket. Climbs steadily until it reaches orbit.</summary>
public static class FlightSimulator
{
    public const double OrbitalVelocityMs = 7800;
    public const double OrbitAltitudeM = 200_000;

    public static IEnumerable<FlightSample> Fly(int durationSeconds = 480, int stepSeconds = 20)
    {
        for (int t = 0; t <= durationSeconds; t += stepSeconds)
        {
            double velocity = Math.Min(OrbitalVelocityMs, t * 16.5);
            double altitude = Math.Min(OrbitAltitudeM, t * t);
            yield return new FlightSample(t, velocity, altitude);
        }
    }
}
