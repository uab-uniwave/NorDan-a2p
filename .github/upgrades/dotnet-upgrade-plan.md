# .NET 9.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade build\a2p.Installer\a2p.Installer.wixproj
4. Upgrade src\a2p.Infrastructure\a2p.Infrastructure.csproj
5. Upgrade src\a2p.Domain\a2p.Domain.csproj
6. Upgrade src\a2p.Application\a2p.Application.csproj
7. Upgrade src\a2p.WinForm\a2p.WinForm.csproj

## Settings

### Excluded projects

| Project name | Description |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

| Package Name                                 | Current Version | New Version | Description                                   |
|:---------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.Extensions.Configuration.FileExtensions | 9.0.0          | 9.0.9      | Recommended for .NET 9.0                      |
| Microsoft.Extensions.Configuration.Json      | 9.0.0          | 9.0.9      | Recommended for .NET 9.0                      |
| Microsoft.Extensions.Logging                 | 9.0.0          | 9.0.9      | Recommended for .NET 9.0                      |
| Microsoft.Extensions.Logging.EventLog        | 9.0.0          | 9.0.9      | Recommended for .NET 9.0                      |

### Project upgrade details

#### build\a2p.Installer\a2p.Installer.wixproj modifications

Project properties changes:
  - Target framework should be changed from `native` to `net9.0`

#### src\a2p.Infrastructure\a2p.Infrastructure.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### src\a2p.Domain\a2p.Domain.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### src\a2p.Application\a2p.Application.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### src\a2p.WinForm\a2p.WinForm.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0-windows` to `net9.0-windows`

NuGet packages changes:
  - Microsoft.Extensions.Configuration.FileExtensions should be updated from `9.0.0` to `9.0.9` (*recommended for .NET 9.0*)
  - Microsoft.Extensions.Configuration.Json should be updated from `9.0.0` to `9.0.9` (*recommended for .NET 9.0*)
  - Microsoft.Extensions.Logging should be updated from `9.0.0` to `9.0.9` (*recommended for .NET 9.0*)
  - Microsoft.Extensions.Logging.EventLog should be updated from `9.0.0` to `9.0.9` (*recommended for .NET 9.0*)

