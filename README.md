```
        /\
       /  \
      | RL |        LaunchPad
      |    |        a tiny rocket launch simulator
     /|    |\
    / |____| \
   /_/ /||\ \_\
       /||\
      ( || )
       ''''
```

# LaunchPad

LaunchPad is a small command line tool used by the launch operations team. It does two jobs:

1. **Launch** a (pretend) rocket and stream its speed and altitude as it climbs to orbit.
2. **Analyze** a recorded flight and print a one-line debrief.

You have inherited this repository. It has no CI, and the only way it has ever been
built is the `build.ps1` script on the previous maintainer's workstation.

## Try it

```
dotnet run --project src/LaunchPad.Cli -- launch --fast       # fly the rocket (drop --fast for the full countdown)
dotnet run --project src/LaunchPad.Cli -- analyze samples/first-flight.csv
dotnet run --project src/LaunchPad.Cli -- --version
```

## Layout

| Path | What's in it |
|------|--------------|
| `src/LaunchPad.Core` | The brains: the pretend rocket, flight log reader, stage table, flight debrief |
| `src/LaunchPad.Cli` | The `launchpad` command |
| `tests/LaunchPad.Tests` | xUnit tests |
| `samples/` | Recorded flights to try it on |
| `build.ps1` | The existing build/publish script (renamed to build.txt to avoid email servers blocking the attachment) |

## Flight logs

A flight log is just a text file with one reading per line:

```
time_s,velocity_ms,altitude_m
20,330,400
40,660,1600
ABORT,engine pressure low        <- only if something went wrong; the reason is optional
```

Radio noise sometimes garbles a line. The tool skips those and counts them as "dropouts".

## Building

```
dotnet build
dotnet test
```
