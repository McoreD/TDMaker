param(
    [Parameter(Mandatory)]
    [string]$RuntimeIdentifier,

    [string]$Configuration = "Release",

    [string]$Version = "",

    [string]$OutputDirectory = "",

    [switch]$SkipClean
)

$ErrorActionPreference = "Stop"

function Resolve-Version {
    param([string]$PropsFile)

    if (!(Test-Path $PropsFile)) {
        throw "Version file not found: $PropsFile"
    }

    [xml]$xml = Get-Content -Path $PropsFile
    $node = $xml.SelectSingleNode("//Version")
    if ($null -eq $node -or [string]::IsNullOrWhiteSpace($node.InnerText)) {
        throw "Could not resolve <Version> from $PropsFile"
    }

    return $node.InnerText.Trim()
}

function Get-PlatformMetadata {
    param([string]$Rid)

    if ($Rid.StartsWith("win-", [StringComparison]::OrdinalIgnoreCase)) {
        return @{
            OsName = "windows"
            ArchiveExtension = ".zip"
        }
    }

    if ($Rid.StartsWith("linux-", [StringComparison]::OrdinalIgnoreCase)) {
        return @{
            OsName = "linux"
            ArchiveExtension = ".tar.gz"
        }
    }

    if ($Rid.StartsWith("osx-", [StringComparison]::OrdinalIgnoreCase)) {
        return @{
            OsName = "macos"
            ArchiveExtension = ".tar.gz"
        }
    }

    throw "Unsupported runtime identifier '$Rid'. Expected win-*, linux-*, or osx-*."
}

function Publish-Project {
    param(
        [string]$ProjectPath,
        [string]$Rid,
        [string]$ConfigurationName,
        [string]$PublishDirectory
    )

    $arguments = @(
        "publish",
        $ProjectPath,
        "-c", $ConfigurationName,
        "-r", $Rid,
        "--self-contained", "true",
        "--disable-build-servers",
        "-p:PublishSingleFile=false",
        "-p:DebugType=None",
        "-o", $PublishDirectory
    )

    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed for $ProjectPath ($Rid)"
    }
}

function Write-ReleaseManifest {
    param(
        [string]$ManifestPath,
        [string]$VersionString,
        [string]$Rid,
        [string]$ArchiveName
    )

    $content = @"
TDMaker Release Package

Version: $VersionString
Runtime: $Rid
Archive: $ArchiveName

Contents:
- desktop/: Avalonia desktop application
- cli/: command-line application
- README.md: project documentation
- LICENSE.txt: project license
"@

    Set-Content -Path $ManifestPath -Value $content -Encoding UTF8
}

function New-ZipArchive {
    param(
        [string]$SourceDirectory,
        [string]$DestinationPath
    )

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    if (Test-Path $DestinationPath) {
        Remove-Item -Path $DestinationPath -Force
    }

    [System.IO.Compression.ZipFile]::CreateFromDirectory($SourceDirectory, $DestinationPath, [System.IO.Compression.CompressionLevel]::Optimal, $true)
}

function New-TarArchive {
    param(
        [string]$ParentDirectory,
        [string]$RootDirectoryName,
        [string]$DestinationPath
    )

    if (Test-Path $DestinationPath) {
        Remove-Item -Path $DestinationPath -Force
    }

    & tar -czf $DestinationPath -C $ParentDirectory $RootDirectoryName
    if ($LASTEXITCODE -ne 0) {
        throw "tar failed while creating $DestinationPath"
    }
}

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$propsFile = Join-Path $repoRoot "Directory.Build.props"
$appProject = Join-Path (Join-Path $repoRoot "src") "TDMaker.App\TDMaker.App.csproj"
$cliProject = Join-Path (Join-Path $repoRoot "src") "TDMaker.Cli\TDMaker.Cli.csproj"

if ([string]::IsNullOrWhiteSpace($Version)) {
    $Version = Resolve-Version -PropsFile $propsFile
}

if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $repoRoot "dist"
}

$platform = Get-PlatformMetadata -Rid $RuntimeIdentifier
$architecture = $RuntimeIdentifier.Substring($RuntimeIdentifier.IndexOf('-') + 1)
$assetBaseName = "TDMaker-$Version-$($platform.OsName)-$architecture"
$stageRoot = Join-Path $repoRoot "artifacts"
$stageDirectory = Join-Path (Join-Path (Join-Path $stageRoot "release") $RuntimeIdentifier) $assetBaseName
$desktopDirectory = Join-Path $stageDirectory "desktop"
$cliDirectory = Join-Path $stageDirectory "cli"
$assetPath = Join-Path $OutputDirectory ($assetBaseName + $platform.ArchiveExtension)

if (!$SkipClean -and (Test-Path $stageDirectory)) {
    Remove-Item -LiteralPath $stageDirectory -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $desktopDirectory | Out-Null
New-Item -ItemType Directory -Force -Path $cliDirectory | Out-Null
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

Write-Host "Packaging TDMaker $Version for $RuntimeIdentifier..."

Publish-Project -ProjectPath $appProject -Rid $RuntimeIdentifier -ConfigurationName $Configuration -PublishDirectory $desktopDirectory
Publish-Project -ProjectPath $cliProject -Rid $RuntimeIdentifier -ConfigurationName $Configuration -PublishDirectory $cliDirectory

Copy-Item -Path (Join-Path $repoRoot "README.md") -Destination (Join-Path $stageDirectory "README.md") -Force
Copy-Item -Path (Join-Path $repoRoot "LICENSE.txt") -Destination (Join-Path $stageDirectory "LICENSE.txt") -Force
Write-ReleaseManifest -ManifestPath (Join-Path $stageDirectory "RELEASE-CONTENTS.txt") -VersionString $Version -Rid $RuntimeIdentifier -ArchiveName (Split-Path $assetPath -Leaf)

if ($platform.ArchiveExtension -eq ".zip") {
    New-ZipArchive -SourceDirectory $stageDirectory -DestinationPath $assetPath
}
else {
    New-TarArchive -ParentDirectory (Split-Path $stageDirectory -Parent) -RootDirectoryName (Split-Path $stageDirectory -Leaf) -DestinationPath $assetPath
}

Write-Host "Created release asset: $assetPath"
