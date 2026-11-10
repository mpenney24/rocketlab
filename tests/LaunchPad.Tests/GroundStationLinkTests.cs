using LaunchPad.Core;

namespace LaunchPad.Tests;

public class GroundStationLinkTests
{
    [Fact]
    public async Task Gets_a_signal_within_the_timeout()
    {
        var link = new GroundStationLink();

        var gotSignal = await link.WaitForSignalAsync(TimeSpan.FromMilliseconds(50));

        Assert.True(gotSignal);
    }
}
