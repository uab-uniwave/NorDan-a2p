# NuGet Package Upgrade Assessment

## Scope
- Project: `src/Installer/Installer.wixproj`
- Requested packages: `WixToolset.Sdk`, `WixToolset.Netfx.wixext`, `WixToolset.Sql.wixext`, `WixToolset.UI.wixext`, `WixToolset.Util.wixext`
- Target version: `7.0.0` (stable)

## Assessment Mode
- Intended mode: Quick assessment (`fullScan=false`)
- Tooling note: `generate_package_upgrade_assessment` does not currently support `.wixproj` input (supports `.csproj` / `.vbproj`).
- Fallback used: Manual project-file assessment + build validation.

## Current -> Proposed Versions (Manual)
- `WixToolset.Sdk`: `5.0.2` -> `7.0.0`
- `WixToolset.Netfx.wixext`: `5.0.2` -> `7.0.0`
- `WixToolset.Sql.wixext`: `5.0.2` -> `7.0.0`
- `WixToolset.UI.wixext`: `5.0.2` -> `7.0.0`
- `WixToolset.Util.wixext`: `5.0.2` -> `7.0.0`

## Reconciliation Result
- Status: **Unified**
- Decision: Apply version `7.0.0` uniformly in the installer project.

## Build/Compatibility Findings
1. WiX v7 requires OSMF EULA acceptance. Build error `WIX7015` was resolved by accepting EULA (`dotnet msbuild -t:AcceptEula -p:EulaId=wix7`).
2. Current blocker after version bump: `WIX0094` in `src/Installer/Package.wxs` (`WixComponentGroup:FilesPrefSuiteBackup` not found).

## Breaking-Change Posture
- API-diff artifacts (`apidiff/*.md`) are unavailable for this project type due assessment-tool input constraints.
- Execution will use build-driven remediation for installer authoring/linker issues surfaced after the WiX v7 upgrade.
