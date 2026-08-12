<#
    .SYNOPSIS
    Publishes pg.exe as a self-contained, single-file Windows executable and
    packages it into a graphical MSI installer using WiX Toolset v7.

    .PARAMETER Configuration
    Build configuration to publish. Defaults to Release.

    .PARAMETER Runtime
    Target runtime identifier. Defaults to win-x64.
#>
param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"

$repoRoot        = Split-Path -Parent $PSScriptRoot
$projectPath     = Join-Path $repoRoot "password-generator\password-generator.csproj"
$installerProject = Join-Path $repoRoot "installer\installer.wixproj"
$publishDir      = Join-Path $repoRoot "artifacts\publish"
$installerOutDir = Join-Path $repoRoot "artifacts\installer"

# --- Read the app version out of the csproj so the MSI always matches ------
[xml]$csprojXml = Get-Content $projectPath
$version = $csprojXml.Project.PropertyGroup.Version | Select-Object -First 1
if ([string]::IsNullOrWhiteSpace($version)) {
    $version = "1.0.0"
}

Write-Host "Password Generator installer build" -ForegroundColor Cyan
Write-Host "  Version:       $version"
Write-Host "  Configuration: $Configuration"
Write-Host "  Runtime:       $Runtime"
Write-Host ""

# --- 1. Publish a self-contained, single-file pg.exe ------------------------
Write-Host "Publishing pg.exe..." -ForegroundColor Cyan

if (Test-Path $publishDir) {
    Remove-Item $publishDir -Recurse -Force
}

dotnet publish $projectPath `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -p:Version=$version `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE."
}

if (-not (Test-Path (Join-Path $publishDir "pg.exe"))) {
    throw "Publish succeeded but pg.exe was not found in $publishDir."
}

# --- 2. Build the MSI with WiX -----------------------------------------------
Write-Host "Building MSI installer..." -ForegroundColor Cyan

if (-not (Test-Path $installerOutDir)) {
    New-Item -ItemType Directory -Path $installerOutDir | Out-Null
}

dotnet build $installerProject `
    -c $Configuration `
    -p:Version=$version `
    -p:PublishDir=$publishDir `
    -p:OutputPath="$installerOutDir\" `
    -p:BaseOutputPath="$installerOutDir\"

if ($LASTEXITCODE -ne 0) {
    throw "Installer build failed with exit code $LASTEXITCODE."
}

$msiPath = Join-Path $installerOutDir "pg-setup-$version.msi"

if (Test-Path $msiPath) {
    Write-Host ""
    Write-Host "Installer created: $msiPath" -ForegroundColor Green
}
else {
    Write-Warning "Build finished, but $msiPath was not found. Check the output above for the actual file name/location."
}