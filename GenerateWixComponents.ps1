$folderPath = "C:\Repos\Nordan-Alu2Prefsuite\Output\Release\net8.0-windows"
$components = @()

# Get all files recursively
Get-ChildItem -Path $folderPath -Recurse -File | ForEach-Object {
    $filePath = $_.FullName -replace '\\', '/'
    $fileName = $_.Name -replace '\.', '_'
    $guid = [guid]::NewGuid().ToString()

    # Generate WiX XML component
    $component = @"
      <Component Id="$fileName" Guid="$guid">
        <File Id="$fileName" Source="$filePath" />
      </Component>
"@
    $components += $component
}

# Generate the full WiX Fragment
$wixXml = @"
<Fragment>
    <ComponentGroup Id="AllComponents">
        $($components -join "`n")
    </ComponentGroup>
</Fragment>
"@

# Output the XML to a file
$wixXml | Out-File -Encoding utf8 -FilePath "C:\Repos\Nordan-Alu2Prefsuite\wix_components.xml"

Write-Host "WiX XML Components generated successfully!"
