# ==========================================
# STAGE 1: Setup
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base
WORKDIR /src

# Traceability metadata (injected in ci.yml)
ARG LP_CI_BUILD_ID=local
ARG LP_GIT_COMMIT_SHA=local

COPY . .
RUN dotnet restore 
# nuget packages

# ==========================================
# STAGE 2: Lint
# ==========================================
# Mitch - should include linting styling? [UPDATE: NO, decide with team which style formatting to use, like prettier, etc.]
FROM base AS lint
RUN dotnet format --verify-no-changes --severity info

# ==========================================
# STAGE 3: Build
# ==========================================
FROM base AS build
RUN dotnet build -c Release \
    /p:AssemblyVersion=1.0.0.$LP_CI_BUILD_ID \
    /p:InformationalVersion=$LP_GIT_COMMIT_SHA \
    --no-restore

# ==========================================
# STAGE 4: Test
# ==========================================
FROM build AS test
RUN dotnet test -c Release --no-restore --logger "trx;LogFileName=test-results.trx"

# ==========================================
# STAGE 5: Publish
# ==========================================
FROM test AS publish
RUN dotnet publish src/LaunchPad.Cli/LaunchPad.Cli.csproj -c Release --no-build -o /app/publish