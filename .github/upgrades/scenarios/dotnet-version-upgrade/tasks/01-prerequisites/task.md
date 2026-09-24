# 01-prerequisites: Verify SDK and repo readiness for .NET 10

Validate that the build environment and repository configuration are ready for the target framework before project-level edits begin. This includes confirming .NET 10 SDK availability and checking whether `global.json` or solution-level settings need updates to avoid restore/build drift during execution.

This task also confirms that the existing solution baseline is understood (including the WiX installer project behavior) so the atomic upgrade can proceed without environment ambiguity.

## Research Findings

- **SDK validation**: `validate_dotnet_sdk_installation(net10.0)` returned success (compatible SDK is installed).
- **global.json validation**: `validate_dotnet_sdk_in_globaljson` reported no `global.json` present, so no SDK pin blocks the upgrade.
- **Project inventory**: Solution contains 3 projects:
  - `src/a2p.WinForm/a2p.WinForm.csproj`
  - `src/a2p.Shared/a2p.Shared.csproj`
  - `build/a2p.Installer/a2p.Installer.wixproj`
- **Baseline build behavior**: Full solution build currently fails in `a2p.Installer.wixproj` with many `WIX0103` missing-file errors pointing to `build/Release/*` artifacts (for example `..\Release\a2p.WinForm.dll`). This indicates installer build depends on pre-produced release outputs and should be validated with its expected packaging flow, not as a raw clean build gate.

## Scope Inventory

- **Projects affected by this task**: none for source edits; verification-only task covering solution-level readiness.
- **Distinct concerns**:
  1. .NET 10 SDK/toolchain readiness
  2. SDK pinning (`global.json`) readiness
  3. Baseline build constraints for WiX installer project
- **Baseline decision**: Proceed with upgrade tasks focused on managed projects (`a2p.Shared`, `a2p.WinForm`), while treating WiX packaging prerequisites as a known validation constraint to handle during final validation.

**Done when**: .NET 10 SDK/tooling prerequisites are validated, baseline readiness issues are documented or fixed, and the solution is ready for atomic upgrade edits.
