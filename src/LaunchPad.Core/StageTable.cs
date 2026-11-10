namespace LaunchPad.Core;

/// <summary>Works out which stage of the rocket is burning at a given time.</summary>
public static class StageTable
{
    // Seconds after liftoff at which each stage finishes burning.
    private static readonly int[] StageEndTimes = { 162, 520 };
    private static readonly string[] StageNames = { "Stage 1", "Stage 2" };

    public static string StageAt(int timeSeconds)
    {
        for (int i = 0; i <= StageEndTimes.Length; i++)
        {
            if (timeSeconds < StageEndTimes[i])
            {
                return StageNames[i];
            }
        }

        return "Coast";
    }
}
