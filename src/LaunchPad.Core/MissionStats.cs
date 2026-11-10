namespace LaunchPad.Core;

/// <summary>Running totals shown at the end of a run.</summary>
public static class MissionStats
{
    public static int SamplesRead;
    public static int Dropouts;   // lines we couldn't make sense of (radio noise)

    public static void Reset()
    {
        SamplesRead = 0;
        Dropouts = 0;
    }
}
