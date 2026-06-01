param(
    [string]$Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"

$RepoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$ProjectPath = Join-Path $RepoRoot "password-generator\password-generator.csproj"
$PublishDir = Join-Path $RepoRoot "artifacts\publish\$Runtime"
$InstallerDir = Join-Path $RepoRoot "artifacts\installer"
$InstallerProject = Join-Path $RepoRoot "installer\PasswordGenerator.wixproj"

dotnet publish $ProjectPath `
    -c Release `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:DebugType=None `
    -p:DebugSymbols=false `
    -o $PublishDir

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

New-Item -ItemType Directory -Force -Path $InstallerDir | Out-Null

dotnet build $InstallerProject -c Release

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Installer created under:"
Write-Host "  $(Join-Path $RepoRoot 'artifacts\installer')"
