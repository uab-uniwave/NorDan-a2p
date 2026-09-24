# 02-upgrade-projects: Apply .NET 10 upgrades across upgradeable projects

Perform the single-pass upgrade by updating target frameworks and package references across the managed projects, then implementing required code changes to resolve compatibility findings. The main scope is `a2p.Shared` and `a2p.WinForm`, with explicit handling for the large set of API compatibility findings reported for WinForms.

Because this is an all-at-once strategy, project, package, and code fixes are coordinated in one bounded pass rather than phased by tier. Any installer-specific adjustments required for solution-level compatibility are handled here as part of unified upgrade completion criteria.

## Scope Inventory

- **Projects in scope**:
  - `src/a2p.Shared/a2p.Shared.csproj`
  - `src/a2p.WinForm/a2p.WinForm.csproj`
- **Dependency relationship**: `a2p.WinForm` references `a2p.Shared`, so both managed projects need coordinated TFM/package updates in one pass.
- **Distinct concerns**:
  1. Retarget both managed projects from `net8.0-Windows` to `net10.0-windows`.
  2. Apply package upgrades recommended by assessment (`NuGet.0002` set) in both projects.
  3. Validate WinForms/System.Drawing compile behavior after retargeting.

## Assessment Findings Recorded

- `a2p.Shared`: 6 issues total (1 mandatory project TFM update + 5 recommended package updates).
- `a2p.WinForm`: 7731 issues total (1 mandatory TFM update + 5 package updates + large API-compatibility set dominated by WinForms/System.Drawing references).
- Recommended package bumps in both projects:
  - `Microsoft.Extensions.Caching.Memory` → `10.0.12`
  - `Microsoft.Extensions.Configuration.FileExtensions` → `10.0.12`
  - `Microsoft.Extensions.Configuration.Json` → `10.0.12`
  - `Microsoft.Extensions.Logging` → `10.0.12`
  - `Microsoft.Extensions.Logging.EventLog` → `10.0.12`

## Project/Dependency Notes

- Package management mode is per-project `PackageReference` (no `Directory.Packages.props` / CPM in use).
- `a2p.WinForm` uses WinForms and COM interop (`PrefSales`) and has custom output paths under `build/`.
- `a2p.Shared` includes explicit assembly references to external PrefSuite interop DLLs from Program Files; these may affect build portability but are unchanged by this task.

**Done when**: Target framework/package changes are applied for the managed projects, inline API compatibility fixes are implemented, and the solution restores/builds without upgrade-related compile errors.
