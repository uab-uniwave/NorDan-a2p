# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade the NorDan a2p solution to .NET 10 (`net10.0` / `net10.0-windows`) while keeping functional parity.
**Scope**: 3 projects (~13.9k LOC), with primary migration risk in the WinForms app due to API compatibility findings.

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Upgrade Strategy | All-at-Once | The solution is already on modern .NET and small enough for a single coordinated upgrade pass. |
| Unsupported API Handling | Fix Inline | API compatibility issues are concentrated in the WinForms project, and resolving them inline avoids deferred stub debt. |

## Tasks

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 3 projects, modern .NET baseline, and a shallow dependency graph with manageable scope for atomic migration.

### 01-prerequisites: Verify SDK and repo readiness for .NET 10

Validate that the build environment and repository configuration are ready for the target framework before project-level edits begin. This includes confirming .NET 10 SDK availability and checking whether `global.json` or solution-level settings need updates to avoid restore/build drift during execution.

This task also confirms that the existing solution baseline is understood (including the WiX installer project behavior) so the atomic upgrade can proceed without environment ambiguity.

**Done when**: .NET 10 SDK/tooling prerequisites are validated, baseline readiness issues are documented or fixed, and the solution is ready for atomic upgrade edits.

### 02-upgrade-projects: Apply .NET 10 upgrades across upgradeable projects

Perform the single-pass upgrade by updating target frameworks and package references across the managed projects, then implementing required code changes to resolve compatibility findings. The main scope is `a2p.Shared` and `a2p.WinForm`, with explicit handling for the large set of API compatibility findings reported for WinForms.

Because this is an all-at-once strategy, project, package, and code fixes are coordinated in one bounded pass rather than phased by tier. Any installer-specific adjustments required for solution-level compatibility are handled here as part of unified upgrade completion criteria.

**Done when**: Target framework/package changes are applied for the managed projects, inline API compatibility fixes are implemented, and the solution restores/builds without upgrade-related compile errors.

### 03-final-validation: Validate build and tests on upgraded solution

Run final validation on the upgraded codebase, including full solution build and available automated tests, and confirm that no dependency conflicts remain. This task captures any residual follow-up recommendations that are intentionally deferred beyond the upgrade scope.

Validation is treated as a dedicated close-out task to ensure the all-at-once migration ends in a clean, verifiable state suitable for commit and handoff.

**Done when**: Full solution build succeeds, tests pass (where available), and any non-blocking follow-up items are documented.
