$basePath = "C:\Repos\Nordan-Alu2Prefsuite\build\Release"   # ← Change this to your target base folder
$outputPath = "C:\Repos\Nordan-Alu2Prefsuite\build"

# List of folders to include
$allowedFolders = @("SQL", "PrefSuiteBackup", "Excel", "runtime" , "win-x64", "native")

# Create output folder if it doesn't exist
if (-not (Test-Path $outputPath)) {
    New-Item -Path $outputPath -ItemType Directory | Out-Null
}

# Function to sanitize file names
function Sanitize-FileName {
    param([string]$name)
    return ($name -replace '[^A-Za-z0-9]', '_')
}

# Function to generate .wxs file content for each folder
function Generate-WxsFile {
    param (
        [string]$GroupId,
        [string]$DirectoryRef,
        [string]$RelativeSubPath,
        [System.IO.FileInfo[]]$Files,
        [string]$TargetFilePath,
        [bool]$IsRoot = $false
    )

    $componentEntries = @()
    foreach ($file in $Files) {
        $sanitizedFileName = Sanitize-FileName $file.Name
        $sourcePath = if ($IsRoot) { "..\Release\$($file.Name)" } else { "..\Release\$RelativeSubPath$($file.Name)" }

        $componentEntry = @"
      <Component>
        <File Source="$sourcePath" />
      </Component>
"@
        $componentEntries += $componentEntry
    }

    $xmlContent = @"
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://wixtoolset.org/schemas/v4/wxs">
  <Fragment>
    <ComponentGroup Id="${GroupId}" DirectoryRef="$Directory">
$($componentEntries -join "`n")
    </ComponentGroup>
  </Fragment>
</Wix>
"@

    $xmlContent | Out-File -FilePath $TargetFilePath -Encoding utf8
    Write-Host "Created: $TargetFilePath"
}

# Process allowed subfolders
Get-ChildItem -Path $basePath -Directory | Where-Object { $allowedFolders -contains $_.Name } | ForEach-Object {
    $folder = $_
    $folderName = $folder.Name
    $folderPath = $folder.FullName
    $wxsFile = "$outputPath\Components${folderName}.wxs"
    $files = Get-ChildItem -Path $folderPath -File
    if ($files.Count -gt 0) {
        Generate-WxsFile -GroupId $folderName -Directory $folderName -RelativeSubPath "$folderName\" -Files $files -TargetFilePath $wxsFile
    }
}

# Process root folder files (not in any subfolder)
$rootFiles = Get-ChildItem -Path $basePath -File
if ($rootFiles.Count -gt 0) {
    $wxsFile = "${outputPath}\ComponentsProduct.wxs"
    Generate-WxsFile -GroupId "ComponentsProduct" -Directory "INSTALLFOLDER" -RelativeSubPath "" -Files $rootFiles -TargetFilePath $wxsFile -IsRoot $true
}