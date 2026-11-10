using LaunchPad.Core;

namespace LaunchPad.Tests;

public class SimulatorTests
{
    [Fact]
    public void Rocket_gets_faster_until_orbit()
    {
        var samples = FlightSimulator.Fly().ToList();

        Assert.Equal(0, samples[0].VelocityMs);
        Assert.Equal(FlightSimulator.OrbitalVelocityMs, samples[^1].VelocityMs);
        Assert.True(samples[5].VelocityMs > samples[4].VelocityMs);
    }

    [Fact]
    public void Stage_changes_after_first_burn()
    {
        Assert.Equal("Stage 1", StageTable.StageAt(0));
        Assert.Equal("Stage 1", StageTable.StageAt(161));
        Assert.Equal("Stage 2", StageTable.StageAt(162));
        Assert.Equal("Stage 2", StageTable.StageAt(300));
    }
}
