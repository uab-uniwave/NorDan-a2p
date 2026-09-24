# .NET Version Upgrade

## Strategy
**Selected**: All-At-Once
**Rationale**: The solution has 3 projects already on modern .NET with no .NET Framework migration boundary, so a single-pass upgrade is the lowest-overhead path.

### Execution Constraints
- Upgrade all upgradeable projects in one atomic pass, without dependency-tier phasing.
- Apply API replacements inline for this run (no deferred stub workflow).
- Update TFMs and package versions before restore/build validation.
- Complete full solution build/test validation after upgrade changes are applied.

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0
- **Commit Strategy**: Single Commit at End

## Upgrade Options

### Strategy
- Upgrade Strategy: All-at-Once

### Compatibility
- Unsupported API Handling: Fix Inline

## Source Control
- **Source Branch**: master
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## User Preferences
### Technical Preferences
- Allow warning suppressions for pre-existing WinForms warnings to complete this .NET 10 upgrade validation.
