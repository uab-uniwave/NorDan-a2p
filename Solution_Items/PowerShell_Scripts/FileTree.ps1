# Define the directory to scan
$rootDir = "R:/Any2Pref"
$saveDir = "R:/Any2Pref/Logs/FileTree"

# Ensure the save directory exists, create if not
if (-not (Test-Path -Path $saveDir)) {
 New-Item -ItemType Directory -Path $saveDir | Out-Null
 Write-Output "Created save directory: $saveDir"
}

# Get the timestamp and prepare the output file name
$timestamp = Get-Date -Format "yyyyMMddTHHmmssfffZ" # ISO 8601 format
$outputFile = Join-Path -Path $saveDir -ChildPath "tree_$timestamp.md"

# List of folders to exclude (case-insensitive)
$excludedFolders = @("Obj", "Output", ".vs", ".vscode", ".git", "Solution_Items", "Logs", "Packages")

# Function to generate the file tree
function Get-FileTree {
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
   Get-FileTree -Path $item.FullName -Prefix "$Prefix│ "
  }
  else {
   # File - Write to output file and show in console
   $line = "$Prefix├── $($item.Name)"
   Write-Output $line
   $line | Out-File -Append -FilePath $outputFile
  }
 }
}

# Initialize the output file
Write-Output "$saveDir/" | Out-File -FilePath $outputFile
Write-Output "$saveDir/"

# Start the tree generation
Get-FileTree -Path $rootDir

Write-Output "File tree saved to: $outputFile"
