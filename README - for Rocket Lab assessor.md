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

# Welcome

Welcome to my solution for the Rocket Lab Build Engineer technical test. In this ReadMe you will find:

0. **How to run locally**

1. **Key Files to Examine** the key files I've added which you should explore
2. **Context Decision Record** a quick summary of my thinking around this task, and how I approached it
3. **Assumptions** a few assumptions I had to make about the nature of the software/solution, as there is no way to know for sure
4. **Priorities** which tasks I found most important, which order I tackled them, and why
5. **Out of Scope** which tasks I thought were Out of Scope [OOS] for this particular test/timeframe, but I would add/do later
6. **Future Tasks** tasks still left to do if I reached the 48-hour limit and still hadn't finished
7. **Conclusion**

Also, you can do a global search for "Mitch" to see anywhere I have modified code, where I give my explanations and justifcations for doing so.

This is to keep a concise record of my thoughts and processes, as well as show you how I work.

## How to run locally

```
docker build -t launchpad-local .
docker create --name temp-ext launchpad-local
docker cp temp-ext:/app/out/. <YOUR_LOCATION>
docker rm temp-ext

```

## Key Files to Examine

```
.github/ci.yml
.github/ci-docker.yml
.github/actions.lock
Directory.Build.props
Directory.Packages.props
Dockerfile
global.json
nuget.config
screenshots/
output/

```

## Context Decision Record

Before starting this task, I came to the following key decisions which influenced the rest of the work done:

1. No instruction was given on how to proceed - therefore, this task is TO EXAMINE HOW YOU THINK/WORK, SO GIVE A CLEAR AUDIT TRAIL AND DECISION RECORD
2. You are given a 48 hour window to complete the task - therefore, CONSIDER THIS TASK MISSION-CRITICAL, AS IF THE CI/CD SOLUTION WAS DUE BY THAT TIME, and use this deadline to influence how you spend your time/what you spend it on rather than trying to make the "perfect" solution (in other words, make it a successful/usable pipeline first, then in future the project can be correctly ticketed/estimated/scoped with the team)
3. Those at Rocket Lab know you don't have experience in every tool they're using - instead, USE YOUR EXISTING SKILLSET TO RECREATE EXACTLY HOW THE DEV HAD IT WORKING (in "build.txt") and if there's time incorporate extra tools (but try not to change the core code itself)
4. Immediately commit the code exactly as-is for historical accuracy
5. When making this solution, keep to Rocket Lab's core principles of Efficiency, Determinism, Portability, Repeatability, Traceability, and Security.
6. You are familiar with GitHub Actions, but this is not a listed Rocket Lab tool - TRY NOT TO ORCHESTRATE TOO MUCH AROUND IT, BUT INSTEAD MAKE SOMETHING WHICH IS PORTABLE and easy to migrate (i.e. Docker)

## Assumptions

1. There are dependencies with security warnings - FIX THOSE
2. Tests don't work - FIX THOSE
3. "build.txt" is actually "build.ps1", a PowerShell script - THE DEV RAN THIS ON A WINDOWS MACHINE, SO BUILD IT WITH WINDOWS RUNNERS
4. There are many references to Registry values - THIS IS WINDOWS SOFTWARE, NO NEED FOR CROSS-COMPILATION AT THIS STAGE
5. A test is flakey on your CI runner - FLAKEY TESTS ARE NOT ALLOWED IN AVIONICS PIPELINES, FIX IT and add metrics to determine severity
6. CLI command --version exists - SOFTWARE IS EXPECTED TO RECORD CURRENT VERSION FROM PIPELINE BUILD (add extra metadata while you're here for local builds)
7. Inclusion of .gitIgnore - IMPLIES YOU SHOULD MANAGE THIS PROJECT WITH GIT
8. An unused variable called "unused" was found in "FightAnalyser.cs" - YOU ARE EXPECTED TO ADD LINTING TO CATCH THESE
9. "build.txt" mentions remote caching of the binary in a buildshare network location - WE DON'T HAVE THIS SET UP; USE GITHUB REMOTE REPO FOR BINARIES INSTEAD
10. Wildcards found on some dependencies, as well as on SDK version - LOCK THESE DOWN TO PREVENT SUPPLY-CHAIN/TOOL-CHAIN DRIFT (move to central version manager "Directory.Packages.props" to prevent different projects using different versions)
11. Project is small and simple, therefore "do not kill an ant with a bazooka" by trying to parallelise or OVER-ENGINEER A SOLUTION. It's better for this to be quick, effective, and intelligently developed
12. Avionics build pipelines require A CLEAN BUILD EACH TIME FOR THE MAIN BRANCH; therefore, don't cache big docker images (like windowsservercore-ltsc2025)
13. No need to break the pipeline down into smaller steps - PROJECT TOO SMALL TO WARRANT IT

## Priorities

Given the small 48-hour window, the key things you should achieve are:
1. Run through the ReadMe and get a handle on the project (what it is, what it does)
2. Identify any initial findings, record them in a document (this)
3. Scope/estimate expected tasks and work, and what tools you might need - recognise what can/can't be done in this time (Out of Scope)
4. Get the software working locally (replicating dev environment exactly, if possible)
5. Create pipeline to imitate this process
6. Get a green build (fix tests) UPDATE: flakey test found on repeat builds, fix that too
7. Optimise build (use caching, add traceability metadata to binaries, etc.)
8. Make local development portable (use Docker)
9. EXTRA: create new CI pipeline using your Docker image
10. Finish writing up documentation
11. Package output binaries, screenshots, etc. into project, zip it, and submit

## Out of Scope [OOS]
1. Branch filtering on pipelines by name, prefix, etc. (add when there are multiple developers working on the project)
2. GitHub Action runners technically update automatically, meaning we can't be 100% Deterministic unless we self-host the runners - but given that this is just a technical assessment, we can let this slide for now
3. "windowsservercore-ltsc2025" also cannot have the OS locked down - in reality we would provide our own custom image from a Nexus, but for the sake of this task we'll use primarily software-layer pins to enforce our toolkits (like "global.json")

## Future Tasks
1. Search for "ACTION:" to see all decisions around future tickets, tasks, and further milestones

## Conclusion

Overall, I really enjoyed this task! It was an excellent showcase of how to build a pipeline, and I appreciated the little nuances thrown in (such as dependency wildcards, flakey tests, and unused variables).

I have compleed this task in 2 different ways:

1. The "Mitch" way (as I know how: purely GitHub Action-based, with rapid build times and complete granularity)
2. The Docker-based "Rocket Lab" way (more portable, orchestration moved into the Dockerfile, and buildable/deployable locally)

You can see screenshots in the "screenshots/" folder of different parts of the process, and I have provided the outputs of both CI pipeline builds in the "outputs/" folder for you to check out.

Thank you very much for your time, and if you have any questions or comments (or improvements!) don't hesitate to let me know.

Regards,
Mitch