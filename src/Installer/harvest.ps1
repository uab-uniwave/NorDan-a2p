[CmdletBinding()]
param(
	[string]$SourceRoot,
	[string]$OutputFile
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($SourceRoot)) {
	$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
	$SourceRoot = [System.IO.Path]::GetFullPath((Join-Path $scriptDirectory '..\..\output\WinForm'))
}

if ([string]::IsNullOrWhiteSpace($OutputFile)) {
	$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
	$OutputFile = [System.IO.Path]::GetFullPath((Join-Path $scriptDirectory 'HarvestedFiles.wxs'))
}

function ConvertTo-WixId {
	param([string]$Value)

	if ([string]::IsNullOrWhiteSpace($Value)) {
		return 'Entry'
	}

	$sanitized = ($Value -replace '[^A-Za-z0-9_]+', '_').Trim('_')
	if ([string]::IsNullOrWhiteSpace($sanitized)) {
		return 'Entry'
	}

	return $sanitized
}

function Escape-Xml {
	param([string]$Value)

	if ($null -eq $Value) {
		return ''
	}

	return [System.Security.SecurityElement]::Escape($Value)
}

function Get-RelativePath {
	param(
		[string]$BasePath,
		[string]$TargetPath
	)

	$normalizedBase = ([System.IO.Path]::GetFullPath($BasePath)).TrimEnd('\','/')
	$normalizedTarget = ([System.IO.Path]::GetFullPath($TargetPath)).TrimEnd('\','/')
	if ([string]::Equals($normalizedBase, $normalizedTarget, [System.StringComparison]::OrdinalIgnoreCase)) {
		return ''
	}

	$baseUri = New-Object System.Uri($normalizedBase + [System.IO.Path]::DirectorySeparatorChar)
	$targetUri = New-Object System.Uri([System.IO.Path]::GetFullPath($TargetPath))
	$relative = [System.Uri]::UnescapeDataString($baseUri.MakeRelativeUri($targetUri).ToString())
	return $relative.Replace('/', [System.IO.Path]::DirectorySeparatorChar)
}

$resolvedSourceRoot = (Resolve-Path -Path $SourceRoot -ErrorAction Stop).Path
$projectDirectory = (Resolve-Path -Path (Split-Path -Parent $MyInvocation.MyCommand.Path) -ErrorAction Stop).Path

if (-not (Test-Path -Path $resolvedSourceRoot -PathType Container)) {
	throw "Source directory not found: $resolvedSourceRoot"
}

$excludedNames = @(
	'appsettings.json',
	'appsettings.Development.json',
	'appsettings.Production.json'
)

$files = Get-ChildItem -Path $resolvedSourceRoot -Recurse -File | Where-Object {
	$_.Extension.ToLowerInvariant() -ne '.pdb' -and $_.Name -notin $excludedNames
}

$filesByDirectory = @{}
foreach ($file in $files) {
	$relativeDirectory = Get-RelativePath -BasePath $resolvedSourceRoot -TargetPath $file.DirectoryName
	$relativeDirectory = $relativeDirectory.Replace('/', [System.IO.Path]::DirectorySeparatorChar)
	if ($relativeDirectory -eq '.' -or $relativeDirectory -eq [System.IO.Path]::DirectorySeparatorChar.ToString()) {
		$relativeDirectory = ''
	}

	if (-not $filesByDirectory.ContainsKey($relativeDirectory)) {
		$filesByDirectory[$relativeDirectory] = New-Object System.Collections.Generic.List[System.IO.FileInfo]
	}

	$filesByDirectory[$relativeDirectory].Add($file)
}

$directories = Get-ChildItem -Path $resolvedSourceRoot -Directory -Recurse
$directoryIds = @{}
foreach ($directory in $directories) {
	$relativeDirectory = Get-RelativePath -BasePath $resolvedSourceRoot -TargetPath $directory.FullName
	if ($relativeDirectory -eq '.') {
		continue
	}

	$directoryIds[$relativeDirectory] = 'Dir_' + (ConvertTo-WixId $relativeDirectory.Replace([System.IO.Path]::DirectorySeparatorChar, '_').Replace([System.IO.Path]::AltDirectorySeparatorChar, '_'))
}

$directoriesByParent = @{}
foreach ($directory in $directories) {
	$relativeDirectory = Get-RelativePath -BasePath $resolvedSourceRoot -TargetPath $directory.FullName
	if ($relativeDirectory -eq '.') {
		continue
	}

	$parentDirectory = [System.IO.Path]::GetDirectoryName($relativeDirectory)
	if ([string]::IsNullOrWhiteSpace($parentDirectory)) {
		$parentDirectory = ''
	}

	if (-not $directoriesByParent.ContainsKey($parentDirectory)) {
		$directoriesByParent[$parentDirectory] = New-Object System.Collections.Generic.List[string]
	}

	$directoriesByParent[$parentDirectory].Add($relativeDirectory)
}

$componentIds = New-Object System.Collections.Generic.List[string]
$xml = New-Object System.Text.StringBuilder
[void]$xml.AppendLine('<?xml version="1.0" encoding="utf-8"?>')
[void]$xml.AppendLine('<Wix xmlns="http://wixtoolset.org/schemas/v4/wxs">')
[void]$xml.AppendLine('  <Fragment>')
[void]$xml.AppendLine('    <DirectoryRef Id="INSTALLFOLDER">')

$writeDirectoryContent = {
	param(
		[string]$RelativeDirectory,
		[string]$Indent
	)

	$relativeDirectoryKey = $RelativeDirectory
	if ([string]::IsNullOrWhiteSpace($relativeDirectoryKey)) {
		$relativeDirectoryKey = ''
	}

	$directoryFiles = @()
	if ($filesByDirectory.ContainsKey($relativeDirectoryKey)) {
		$directoryFiles = @($filesByDirectory[$relativeDirectoryKey] | Sort-Object Name)
	}

	foreach ($file in $directoryFiles) {
		$relativePath = Get-RelativePath -BasePath $projectDirectory -TargetPath $file.FullName
		$relativePath = $relativePath -replace '\\', '/'

		$relativeFilePath = Get-RelativePath -BasePath $resolvedSourceRoot -TargetPath $file.FullName
		$componentId = 'cmp_' + (ConvertTo-WixId ($relativeFilePath.Replace([System.IO.Path]::DirectorySeparatorChar, '_').Replace([System.IO.Path]::AltDirectorySeparatorChar, '_')))
		$componentIds.Add($componentId) | Out-Null
		$guid = [Guid]::NewGuid().ToString('B')

		[void]$xml.AppendLine(('{0}<Component Id="{1}" Guid="{2}">' -f $Indent, $componentId, $guid))
		[void]$xml.AppendLine(('{0}  <File Source="{1}" />' -f $Indent, $relativePath))
		[void]$xml.AppendLine(('{0}</Component>' -f $Indent))
	}

	$childDirectories = @()
	if ($directoriesByParent.ContainsKey($relativeDirectoryKey)) {
		$childDirectories = @($directoriesByParent[$relativeDirectoryKey] | Sort-Object)
	}

	foreach ($childDirectory in $childDirectories) {
		$childName = [System.IO.Path]::GetFileName($childDirectory)
		$childId = $directoryIds[$childDirectory]
		[void]$xml.AppendLine(('{0}<Directory Id="{1}" Name="{2}">' -f $Indent, $childId, (Escape-Xml $childName)))
		& $writeDirectoryContent $childDirectory ($Indent + '  ')
		[void]$xml.AppendLine(('{0}</Directory>' -f $Indent))
	}
}

& $writeDirectoryContent '' '      '

[void]$xml.AppendLine('    </DirectoryRef>')
[void]$xml.AppendLine('    <ComponentGroup Id="HarvestedFiles">')
foreach ($componentId in $componentIds) {
	[void]$xml.AppendLine(('      <ComponentRef Id="{0}" />' -f $componentId))
}
[void]$xml.AppendLine('    </ComponentGroup>')
[void]$xml.AppendLine('  </Fragment>')
[void]$xml.AppendLine('</Wix>')

Set-Content -Path $OutputFile -Value $xml.ToString() -Encoding UTF8
Write-Host "Harvested $($files.Count) files into $OutputFile"
