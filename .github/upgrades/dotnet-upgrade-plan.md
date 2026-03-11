# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade FS Challenges\FS Challenges.fsproj
4. Upgrade ChallengeCore\ChallengeCore.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

No projects are excluded.

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### FS Challenges\FS Challenges.fsproj modifications

Project properties changes:
  - No target framework changes required (currently targets `netstandard2.0`, which is compatible with .NET 10.0)

No other changes detected by analysis.

#### ChallengeCore\ChallengeCore.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0-windows`
