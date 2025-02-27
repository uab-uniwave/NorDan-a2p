param (
    [string]$sourcePath = ".\Output\Release\net8.0-windows",
    [string]$outputPath = ".\src\a2p.Installer"
)

# Resolve the paths to absolute paths
$sourcePath = (Resolve-Path -Path $sourcePath).Path
$outputPath = (Resolve-Path -Path $outputPath).Path

# Define the output file paths
$outputFilePath = Join-Path -Path $outputPath -ChildPath "Components.wxs"
$productFilePath = Join-Path -Path $outputPath -ChildPath "Product.wxs"
$fragmentFilePath = Join-Path -Path $outputPath -ChildPath "Fragment.wxs"
$directoryFilePath = Join-Path -Path $outputPath -ChildPath "Directory.wxs"

# Clear Host
Clear-Host

# Print the resolved paths to the console
Write-Host "Resolved source path: $sourcePath" -ForegroundColor Blue
Write-Host "Resolved output path: $outputPath" -ForegroundColor Blue  -InformationAction Continue
Write-Host "Output file saved path: $outputFilePath" -ForegroundColor Green

function Sanitize-Id {
    param (
        [string]$id
    )
    return $id -replace '[^a-zA-Z0-9_.]', '_'
}

function Remove-Prefix {
    param (
        [string]$id
    )
    return $id -replace '^[^_]+_', ''
}

function Generate-Component {
    param (
        [string]$folderPath,
        [string]$componentId
    )

    $files = Get-ChildItem -Path $folderPath -File
    $guid = [guid]::NewGuid().ToString()
    $componentId = Remove-Prefix -id (Sanitize-Id -id $componentId)
    $componentXml = "<Component Id='$componentId' Guid='$guid'>" + "`r`n"
    foreach ($file in $files) {
        $fileId = Remove-Prefix -id (Sanitize-Id -id ($componentId + '_' + $file.Name))
        $relativePath = $file.FullName.Substring($sourcePath.Length + 1) -replace '\\', '/'
        $componentXml += "    <File Id='$fileId' Source='$relativePath' />" + "`r`n"
    }
    $componentXml += "</Component>" + "`r`n"
    return $componentXml
}

function Generate-Directory {
    param (
        [string]$folderPath,
        [string]$componentId
    )

    $directories = Get-ChildItem -Path $folderPath -Directory
    $directoryXml = Generate-Component -folderPath $folderPath -componentId $componentId
    foreach ($directory in $directories) {
        $subComponentId = Remove-Prefix -id (Sanitize-Id -id ($componentId + '_' + $directory.Name))
        $directoryXml += Generate-Directory -folderPath $directory.FullName -componentId $subComponentId
    }
    return $directoryXml
}

function Generate-Fragments {
    param (
        [string]$folderPath,
        [string]$parentId = "TARGETDIR",
        [string]$componentIdPrefix = ""
    )

    $directories = Get-ChildItem -Path $folderPath -Directory
    $fragmentsXml = ""
    foreach ($directory in $directories) {
        $directoryId = Remove-Prefix -id (Sanitize-Id -id ($componentIdPrefix + '_' + $directory.Name))
        $fragmentsXml += "  <Fragment>" + "`r`n"
        $fragmentsXml += "    <DirectoryRef Id='$parentId'>" + "`r`n"
        $fragmentsXml += "      <Directory Id='$directoryId' Name='$directoryId'>" + "`r`n"
        $fragmentsXml += Generate-Directory -folderPath $directory.FullName -componentId $directoryId
        $fragmentsXml += "      </Directory>" + "`r`n"
        $fragmentsXml += "    </DirectoryRef>" + "`r`n"
        $fragmentsXml += "  </Fragment>" + "`r`n"
        $fragmentsXml += Generate-Fragments -folderPath $directory.FullName -parentId $directoryId -componentIdPrefix $directoryId
    }
    return $fragmentsXml
}

# Generate components for files in the source path first
$xmlContent = "<Wix xmlns='http://schemas.microsoft.com/wix/2006/wi'>" + "`r`n"
$xmlContent += "  <Fragment>" + "`r`n"
$xmlContent += "    <DirectoryRef Id='TARGETDIR'>" + "`r`n"
$xmlContent += "      <Directory Id='SOURCE' Name='Source'>" + "`r`n"
$xmlContent += Generate-Component -folderPath $sourcePath -componentId "SOURCE"
$xmlContent += "      </Directory>" + "`r`n"
$xmlContent += "    </DirectoryRef>" + "`r`n"
$xmlContent += "  </Fragment>" + "`r`n"

# Generate fragments for subfolders recursively
$xmlContent += Generate-Fragments -folderPath $sourcePath -componentIdPrefix "SOURCE"
$xmlContent += "</Wix>" + "`r`n"

Set-Content -Path $outputFilePath -Value $xmlContent -Encoding Ascii

# Extract ComponentRef IDs from the generated Components.wxs
$componentRefs = Select-String -Path $outputFilePath -Pattern '<Component Id=' | ForEach-Object {
    $_.Matches[0].Groups[1].Value
}

# Update Product.wxs with new ComponentRef IDs
$productXml = [xml](Get-Content -Path $productFilePath)
$namespaceManager = New-Object System.Xml.XmlNamespaceManager($productXml.NameTable)
$namespaceManager.AddNamespace("wix", "http://schemas.microsoft.com/wix/2006/wi")
$componentGroup = $productXml.SelectSingleNode("//wix:ComponentGroup[@Id='ProductComponents']", $namespaceManager)

if ($null -eq $componentGroup) {
    Write-Host "ComponentGroup with Id 'ProductComponents' not found in Product.wxs" -ForegroundColor Red
    exit 1
}

# Clear existing ComponentRef elements
$componentGroup.RemoveAll()

# Add new ComponentRef elements
foreach ($componentRef in $componentRefs) {
    $newComponentRef = $productXml.CreateElement("ComponentRef", "http://schemas.microsoft.com/wix/2006/wi")
    $newComponentRef.SetAttribute("Id", $componentRef)
    $componentGroup.AppendChild($newComponentRef) | Out-Null
}

# Save the updated Product.wxs
$productXml.Save($productFilePath)

Write-Host "Product.wxs updated with new ComponentRef IDs" -ForegroundColor Green

# Update Fragment.wxs with new ComponentRef IDs
$fragmentXml = [xml](Get-Content -Path $fragmentFilePath)
$componentGroup = $fragmentXml.SelectSingleNode("//wix:ComponentGroup[@Id='AllComponents']", $namespaceManager)

if ($null -eq $componentGroup) {
    Write-Host "ComponentGroup with Id 'AllComponents' not found in Fragment.wxs" -ForegroundColor Red
    exit 1
}

# Clear existing ComponentRef elements
$componentGroup.RemoveAll()

# Add new ComponentRef elements
foreach ($componentRef in $componentRefs) {
    $newComponentRef = $fragmentXml.CreateElement("ComponentRef", "http://schemas.microsoft.com/wix/2006/wi")
    $newComponentRef.SetAttribute("Id", $componentRef)
    $componentGroup.AppendChild($newComponentRef) | Out-Null
}

# Save the updated Fragment.wxs
$fragmentXml.Save($fragmentFilePath)

Write-Host "Fragment.wxs updated with new ComponentRef IDs" -ForegroundColor Green

# Update Directory.wxs with new ComponentRef IDs
$directoryXml = [xml](Get-Content -Path $directoryFilePath)
$componentGroup = $directoryXml.SelectSingleNode("//wix:DirectoryRef[@Id='ProgramFilesFolder']", $namespaceManager)

if ($null -eq $componentGroup) {
    Write-Host "DirectoryRef with Id 'ProgramFilesFolder' not found in Directory.wxs" -ForegroundColor Red
    exit 1
}

# Clear existing Component elements
$componentGroup.RemoveAll()

# Add new Component elements
foreach ($componentRef in $componentRefs) {
    $newComponent = $directoryXml.CreateElement("Component", "http://schemas.microsoft.com/wix/2006/wi")
    $newComponent.SetAttribute("Id", $componentRef)
    $newComponent.SetAttribute("Guid", [guid]::NewGuid().ToString())
    $componentGroup.AppendChild($newComponent) | Out-Null
}

# Save the updated Directory.wxs
$directoryXml.Save($directoryFilePath)

Write-Host "Directory.wxs updated with new Component IDs" -ForegroundColor Green
