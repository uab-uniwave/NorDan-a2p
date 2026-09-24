# NuGet Package Upgrade Plan

## Overview

**Target**: Upgrade WiX Toolset packages in the installer project to version 7.0.0 and keep the installer buildable.
**Scope**: Single installer project (`src/Installer/Installer.wixproj`) plus any directly related WiX authoring files required to restore a successful build.

## Tasks

### 01-upgrade-installer-wix-packages: Upgrade installer WiX packages to 7.0.0

Apply the unified package-version upgrade in `src/Installer/Installer.wixproj` from WiX Toolset 5.0.2 to 7.0.0 for `WixToolset.Sdk`, `WixToolset.Netfx.wixext`, `WixToolset.Sql.wixext`, `WixToolset.UI.wixext`, and `WixToolset.Util.wixext`. Keep package management per-project (no CPM conversion), since scope is a single project and all references already live in the project file.

Then resolve upgrade-blocking installer build issues surfaced after the version bump, starting with required WiX v7 EULA acceptance and any linker/compiler failures (currently `WIX0094` for missing `WixComponentGroup:FilesPrefSuiteBackup`). Use build-driven remediation and adjust only the minimum installer authoring needed to restore a successful build.

**Done when**: `src/Installer/Installer.wixproj` references WiX Toolset 7.0.0 for the SDK and all listed WiX extension packages, WiX v7 EULA acceptance is in place, installer/linker build errors introduced or surfaced by the upgrade are resolved, and the solution build completes without errors.
