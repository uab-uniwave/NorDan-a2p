# Define the directory to scan
$rootDir = "C:/Repos/Nordan-Alu2Prefsuite"
$saveDir = "C:/Repos/Nordan-Alu2Prefsuite/Logs/FileTree"

# Ensure the save directory exists, create if not
if (-not (Test-Path -Path $saveDir)) {
  New-Item -ItemType Directory -Path $saveDir | Out-Null
  Write-Output "Created save directory: $saveDir"
}

# Get the timestamp and prepare the output file name
$timestamp = Get-Date -Format "yyyyMMddTHHmmssfffZ" # ISO 8601 format
$outputFile = Join-Path -Path $saveDir -ChildPath "treeMethods_$timestamp.md"
# 
$excludedFolders = @("Obj", "Output", ".vs", ".vscode", ".git", "Solution_Items", "Logs", "Packages", "lib", "Test_Cases", "XML___", "Setup1", "Properties", "Resources", "WixSharp.Setup", "CustomControls")

# Function to generate the file tree and extract methods
function Get-FileTreeWithMethods {
  param (
    [string]$Path,
    [string]$Prefix = ""
  )

  # Get all directories and files in the current path
  $items = Get-ChildItem -Path $Path -Force | Sort-Object -Property PSIsContainer, Name

  foreach ($item in $items) {
    # Skip excluded folders and their contents
    if ($item.PSIsContainer -and $excludedFolders -contains $item.Name) {
      continue
    }

    if ($item.PSIsContainer) {
      # Directory - Write to output file and show in console
      $line = "$Prefix├── $($item.Name)/"
      Write-Output $line
      $line | Out-File -Append -FilePath $outputFile

      # Recursively process the directory
      Get-FileTreeWithMethods -Path $item.FullName -Prefix "$Prefix│ "
    }
    elseif ($item.Extension -eq ".cs") {
      # File - Write to output file and show in console
      $line = "$Prefix├── $($item.Name)"
      Write-Output $line
      $line | Out-File -Append -FilePath $outputFile

      # Extract and write methods from the .cs file
      $methods = Get-Content -Path $item.FullName |
      Select-String -Pattern '^\s*(public|private|protected|internal|static)?\s*(\w+\s+)+(\w+)\s*\(.*\)\s*{?' |
      ForEach-Object { $_.Matches.Value }

      foreach ($method in $methods) {
        $methodLine = "$Prefix│ ├── Method: $method"
        Write-Output $methodLine
        $methodLine | Out-File -Append -FilePath $outputFile
      }
    }
  }
}

# Initialize the output file
Write-Output "$rootDir/" | Out-File -FilePath $outputFile
Write-Output "$rootDir/"

# Start the tree generation
Get-FileTreeWithMethods -Path $rootDir
