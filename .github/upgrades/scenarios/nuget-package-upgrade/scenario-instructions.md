# NuGet Package Upgrade

## Strategy
Unified single-project WiX package upgrade with build-driven remediation for installer authoring issues.

## Preferences
- **Flow Mode**: Automatic
- **Commit Strategy**: After Each Task
- **Pace**: Standard
- **Scope**: `src/Installer/Installer.wixproj` (single project)
- **Packages**: `WixToolset.Sdk`, `WixToolset.Netfx.wixext`, `WixToolset.Sql.wixext`, `WixToolset.UI.wixext`, `WixToolset.Util.wixext`
- **Target Version**: `7.0.0`
- **Include Prerelease**: `false`
- **Version Reconciliation**: Unified `7.0.0` across the scoped project
- **Package Management Mode**: Per-project package references (no CPM conversion)

## Decisions
- Use manual assessment fallback because the package-upgrade assessment tool does not accept `.wixproj` input.
- Keep the upgrade scoped to the installer project and remediate post-upgrade WiX build errors in task execution.

## Custom Instructions

## Source Control
- **Source Branch**: `upgrade-dotnet-10`
- **Working Branch**: `upgrade-wix-toolset-7`
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)
