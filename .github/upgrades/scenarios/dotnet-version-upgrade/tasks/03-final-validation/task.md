# 03-final-validation: Validate build and tests on upgraded solution

Run final validation on the upgraded codebase, including full solution build and available automated tests, and confirm that no dependency conflicts remain. This task captures any residual follow-up recommendations that are intentionally deferred beyond the upgrade scope.

Validation is treated as a dedicated close-out task to ensure the all-at-once migration ends in a clean, verifiable state suitable for commit and handoff.

## Validation Scope

- Execute full-solution build validation after managed project upgrades.
- Validate managed projects using the appropriate build tools:
  - `dotnet build` for SDK-style shared library checks
  - full `msbuild` for WinForms/COM and solution-level validation paths
- Re-check installer/WiX behavior now that managed outputs are retargeted to .NET 10.
- Discover and run test projects if present; if none exist, document test gap explicitly.

## Known Inputs from Prior Tasks

- Managed projects (`a2p.Shared`, `a2p.WinForm`) now target `net10.0-windows` and built successfully in task 02.
- Baseline installer issue from task 01 was tied to missing expected `build/Release/*` payload files during clean builds.

**Done when**: Full solution build succeeds, tests pass (where available), and any non-blocking follow-up items are documented.
