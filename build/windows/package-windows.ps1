param(
    [string]$RuntimeIdentifier = "win-x64"
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path (Join-Path $PSScriptRoot "..") "..")
$scriptPath = Join-Path (Join-Path $repoRoot "build") "package-release.ps1"

& $scriptPath -RuntimeIdentifier $RuntimeIdentifier
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
