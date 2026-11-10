namespace LaunchPad.Core;

/// <summary>
/// Pretend radio link to the rocket. Real radios take a moment to lock on, so we
/// simulate a random delay.
/// </summary>
public class GroundStationLink
{
    public async Task<bool> WaitForSignalAsync(TimeSpan timeout)
    {
        var signal = Task.Delay(Random.Shared.Next(0, 60));
        var giveUp = Task.Delay(timeout);

        var first = await Task.WhenAny(signal, giveUp);
        return first == signal;
    }
}
