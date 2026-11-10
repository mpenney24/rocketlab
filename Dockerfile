# Mitch - build.txt(/.ps1) shows LaunchPad is Windows ONLY application - therefore, transfer the impl in this file into Docker instructions, and build for Windows.
#  ACTION: severity low, priority medium - keep all existing executions for now, but revisit to question/optimise process.

# Mitch - opted for "windowsservercore-10.0.26100" as it contains the latest registry infra needed to fix "'Registry.CurrentUser' is only supported on: 'windows'" errors
#  HOWEVER, this is a big image! The alternative was to use Windows CI runners in GitHub Actions, but I opted to be platform agnostic so this can run on
#  on jenkins, TeamCity, GitLab CI, etc. in a single command than needing to pre-install .NET SDKS on the runners. This can be changed later once I know Rocket Lab's 
#  workflow processes and preferences.
FROM mcr.microsoft.com/dotnet/sdk:10.0-windowsservercore-ltsc2025 AS base
WORKDIR C:/src/LaunchPad

# Mitch - traceability metadata (injected via ".github/workflows/ci.yml" on CI, defaulted here locally)
ARG LP_CI_BUILD_ID=local
ARG LP_GIT_COMMIT_SHA=local

COPY . .

RUN dotnet restore LaunchPad.sln

# ==========================================
# STAGE 2: Lint
# ==========================================
# Mitch - should include linting styling? [UPDATE: NO, discuss with the team which style formatting to use, like prettier, etc. - fine for now, not a priority. 
#   ACTION: severity low, priority low - Create Jira ticket to return to this later]
# Mitch - linting entire project each time is 10s - is this acceptable? Can we lint only changed files? [UPDATE: we can, but linting is only 10s for now, and adding
#  complexity will make it longer. 
#   ACTION: severity low, priority low - Create Jira ticket to return to this when code base size increases OR step > 20s]
# Mitch - SHOULD YOU HAVE SEVERITY ERROR HERE?
FROM base AS lint
RUN dotnet format --verify-no-changes --severity error

# ==========================================
# STAGE 3: Build
# ==========================================
FROM base AS build 
RUN dotnet build LaunchPad.sln -c Release \
    /p:AssemblyVersion=1.0.0.${LP_CI_BUILD_ID} \
    /p:InformationalVersion=${LP_GIT_COMMIT_SHA} \
    /p:TreatWarningsAsErrors=true \
    --no-restore

# ==========================================
# STAGE 4: Test
# ==========================================
FROM build AS test
# Mitch - could add "--collect:"XPlat Code Coverage"" flag as we can then get more metrics. [UPDATE: OOS for this change]
#  ACTION: severity low, priority low - Create Jira ticket for adding code quality/coverage/speed metrics to the project in future
RUN dotnet test tests\LaunchPad.Tests\LaunchPad.Tests.csproj -c Release --no-build

# ==========================================
# STAGE 5: Publish
# ==========================================
FROM test AS publish
RUN dotnet publish src\LaunchPad.Cli\LaunchPad.Cli.csproj -c Release -o "C:\src\LaunchPad\out"

# ==========================================
# STAGE 6: Export
# ==========================================
FROM scratch AS export
COPY --from=publish C:\src\LaunchPad\out C:\out