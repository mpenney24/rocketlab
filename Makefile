SOLUTION = LaunchPad.sln
PROJECT  = src/LaunchPad.Cli/LaunchPad.Cli.csproj
CONFIG   = Release
RID      = win-x64
OUT_DIR  = publish-output/bin

# Mitch - local fallback for default values if dev runs this locally
export LP_CI_BUILD_ID     ?= 0
export LP_GIT_COMMIT_SHA  ?= SHA
export LP_GIT_BRANCH_NAME ?= local

.PHONY: all restore build test publish audit

all: restore build test publish

# Mitch - added "locked-mode" to prevent any upstream package hashes from being changed (see RestorePackagesWithLockFile in Build.props for auto-gen of
#  these packages.lock.json files)
restore:
	dotnet restore $(SOLUTION) --locked-mode

build:
	dotnet build $(SOLUTION) -c $(CONFIG) --no-restore

test:
	dotnet test $(SOLUTION) -c $(CONFIG) --no-restore --no-build

publish:
	dotnet publish $(PROJECT) -c $(CONFIG) -r $(RID) --self-contained true -o ./$(OUT_DIR)

# UPDATE: added as part of moving audit to manual warnings instead of automated PRs (dependabot)
#  "outdated" tool will fail the audit on any outdated/deprecated packages, whereas "-warnaserror" will handle security vulnerabilites; SECURITY, VISIBILITY
audit:
	dotnet tool restore
	dotnet outdated $(SOLUTION) --fail-on-updates
	dotnet restore $(SOLUTION) --locked-mode -warnaserror