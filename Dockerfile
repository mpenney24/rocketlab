# Mitch - using the same version as our ci.yml file
#  NOTE: can't specify exact version of dotnetsdk here - instead, "global.json" file ensures it will download the correct one (in our case, 10.0.404)
FROM mcr.microsoft.com/dotnet/sdk:10.0-windowsservercore-ltsc2025 AS build
WORKDIR /app

ARG LP_CI_BUILD_ID=0
ARG LP_GIT_COMMIT_SHA=SHA
ARG LP_GIT_BRANCH_NAME=local

# Mitch - copy lock files, props etc. to optimize Docker caching
COPY Directory.Build.props Directory.Packages.props global.json nuget.config LaunchPad.sln ./

# Mitch - now rest of code
COPY src/ ./src/
COPY tests/ ./tests/

# Mitch - offline enforcement in the container, to guarantee we only use cached local files; SECURITY
RUN dotnet restore LaunchPad.sln

COPY . .

# Mitch - deterministic build using settings found in "Directory.Build.props"
RUN dotnet publish src/LaunchPad.Cli/LaunchPad.Cli.csproj -c Release -o /app/out --no-restore -p:LP_CI_BUILD_ID=%LP_CI_BUILD_ID% -p:LP_GIT_COMMIT_SHA=%LP_GIT_COMMIT_SHA% -p:LP_GIT_BRANCH_NAME=%LP_GIT_BRANCH_NAME%

# Final runtime artifact stage
FROM mcr.microsoft.com/dotnet/runtime:10.0-windowsservercore-ltsc2025 AS runtime
WORKDIR /app
COPY --from=build /app/out /app/out