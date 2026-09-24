# 01-upgrade-installer-wix-packages: Upgrade installer WiX packages to 7.0.0

Apply the unified package-version upgrade in `src/Installer/Installer.wixproj` from WiX Toolset 5.0.2 to 7.0.0 for `WixToolset.Sdk`, `WixToolset.Netfx.wixext`, `WixToolset.Sql.wixext`, `WixToolset.UI.wixext`, and `WixToolset.Util.wixext`. Keep package management per-project (no CPM conversion), since scope is a single project and all references already live in the project file.

Then resolve upgrade-blocking installer build issues surfaced after the version bump with build-driven remediation and minimum required changes.

## Scope Inventory
- **Projects affected**: `src/Installer/Installer.wixproj` (direct), with project references to `src/Shared/Shared.csproj` and `src/WinForm/WinForm.csproj`.
- **Distinct concerns**:
  - Package/version upgrade in `.wixproj`
  - WiX v7 EULA acceptance gate (`WIX7015`)
  - Build validation at project and solution scope
- **Dependency and package findings** (`get_project_dependencies`):
  - Non-CPM project; package versions defined directly in `Installer.wixproj`
  - Package references confirmed at `7.0.0`: `WixToolset.Netfx.wixext`, `WixToolset.Sql.wixext`, `WixToolset.UI.wixext`, `WixToolset.Util.wixext`
  - SDK confirmed as `WixToolset.Sdk/7.0.0`
- **Installer authoring files reviewed**: `src/Installer/Package.wxs`, `src/Installer/Components.wxs`

## Research Notes
- The scenario assessment tool cannot process `.wixproj`, so this task uses manual assessment plus build validation.
- WiX v7 EULA was accepted with `dotnet msbuild src/Installer/Installer.wixproj -t:AcceptEula -p:EulaId=wix7`.
- `run_build` validates both installer project and full solution successfully after the package upgrade.

**Done when**: `src/Installer/Installer.wixproj` references WiX Toolset 7.0.0 for the SDK and all listed WiX extension packages, WiX v7 EULA acceptance is in place, installer/linker build errors introduced or surfaced by the upgrade are resolved, and the solution build completes without errors.
