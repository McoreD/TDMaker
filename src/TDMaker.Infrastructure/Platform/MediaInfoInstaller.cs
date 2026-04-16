namespace TDMaker.Infrastructure.Platform;

using System.IO.Compression;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed partial class MediaInfoInstaller(
    IHttpClientFactory httpClientFactory,
    IPlatformPaths platformPaths,
    IProcessRunner processRunner,
    ILogger<MediaInfoInstaller> logger) : IMediaInfoInstaller
{
    private const string MediaInfoVersion = "26.01";
    private const string LibZenVersion = "0.4.41";
    private const string WindowsX64Url = "https://mediaarea.net/download/binary/mediainfo/26.01/MediaInfo_CLI_26.01_Windows_x64.zip";
    private const string WindowsArm64Url = "https://mediaarea.net/download/binary/mediainfo/26.01/MediaInfo_CLI_26.01_Windows_ARM64.zip";
    private const string MacUrl = "https://mediaarea.net/download/binary/mediainfo/26.01/MediaInfo_CLI_26.01_Mac.dmg";
    private const string LinuxAmd64CliUrl = "https://mediaarea.net/download/binary/mediainfo/26.01/mediainfo_26.01-1_amd64.Ubuntu_24.04.deb";
    private const string LinuxAmd64LibMediaInfoUrl = "https://mediaarea.net/download/binary/libmediainfo0/26.01/libmediainfo0v5_26.01-1_amd64.Ubuntu_24.04.deb";
    private const string LinuxAmd64LibZenUrl = "https://mediaarea.net/download/binary/libzen0/0.4.41/libzen0v5_0.4.41-1_amd64.Ubuntu_24.04.deb";
    private const string LinuxArm64CliUrl = "https://mediaarea.net/download/binary/mediainfo/26.01/mediainfo_26.01-1_arm64.Ubuntu_24.04.deb";
    private const string LinuxArm64LibMediaInfoUrl = "https://mediaarea.net/download/binary/libmediainfo0/26.01/libmediainfo0v5_26.01-1_arm64.Ubuntu_24.04.deb";
    private const string LinuxArm64LibZenUrl = "https://mediaarea.net/download/binary/libzen0/0.4.41/libzen0v5_0.4.41-1_arm64.Ubuntu_24.04.deb";

    public async Task<ToolInstallationResult> InstallLatestAsync(
        IProgress<ToolInstallationProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        MediaInfoInstallPlan plan;
        string? primaryDownloadUrl = null;

        try
        {
            plan = ResolvePlan();
            primaryDownloadUrl = plan.Artifacts[0].DownloadUrl;
        }
        catch (PlatformNotSupportedException ex)
        {
            LogUnsupportedPlatform(logger, ex, ex.Message);
            return Failure(ex.Message);
        }

        Directory.CreateDirectory(platformPaths.ToolsDirectory);

        var tempRoot = Path.Combine(Path.GetTempPath(), $"tdmaker-mediainfo-{Guid.NewGuid():N}");
        var downloadsRoot = Path.Combine(tempRoot, "downloads");
        var stagingToolsRoot = Path.Combine(tempRoot, "tools");

        Directory.CreateDirectory(downloadsRoot);
        Directory.CreateDirectory(stagingToolsRoot);

        try
        {
            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.MediaInfo,
                Status = $"Downloading MediaInfo {MediaInfoVersion} from {plan.ProviderName}...",
                Percentage = 0
            });

            var downloadedArtifacts = await DownloadArtifactsAsync(plan, downloadsRoot, progress, cancellationToken);
            var validationPath = await StageInstallationAsync(plan, downloadedArtifacts, stagingToolsRoot, progress, cancellationToken);

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.MediaInfo,
                Status = "Validating managed MediaInfo...",
                Percentage = 96
            });

            var validation = await processRunner.RunAsync(
                validationPath,
                ["--Version"],
                cancellationToken: cancellationToken);

            if (validation.ExitCode != 0)
            {
                LogValidationFailed(logger, validation.ExitCode, validation.StandardError);
                return Failure(
                    "MediaInfo was downloaded but did not start successfully on this machine.",
                    primaryDownloadUrl,
                    validationPath);
            }

            CommitStagedTools(stagingToolsRoot);

            var installedPath = Path.Combine(
                platformPaths.ToolsDirectory,
                Path.GetRelativePath(stagingToolsRoot, validationPath));

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.MediaInfo,
                Status = $"Managed MediaInfo installed at {installedPath}",
                Percentage = 100
            });

            return new ToolInstallationResult
            {
                Kind = ToolKind.MediaInfo,
                Success = true,
                Message = $"Managed MediaInfo installed at {installedPath}.",
                InstalledPath = installedPath,
                DownloadUrl = primaryDownloadUrl
            };
        }
        catch (OperationCanceledException)
        {
            LogCanceled(logger);
            return Failure("MediaInfo installation was canceled.", primaryDownloadUrl);
        }
        catch (Exception ex)
        {
            LogInstallFailed(logger, ex);
            return Failure($"MediaInfo installation failed: {ex.Message}", primaryDownloadUrl);
        }
        finally
        {
            TryDeleteDirectory(tempRoot);
        }
    }

    private async Task<IReadOnlyList<DownloadedArtifact>> DownloadArtifactsAsync(
        MediaInfoInstallPlan plan,
        string downloadsRoot,
        IProgress<ToolInstallationProgress>? progress,
        CancellationToken cancellationToken)
    {
        var downloads = new List<DownloadedArtifact>(plan.Artifacts.Count);

        for (var index = 0; index < plan.Artifacts.Count; index++)
        {
            var artifact = plan.Artifacts[index];
            var targetPath = Path.Combine(downloadsRoot, artifact.FileName);

            await DownloadFileAsync(
                artifact,
                targetPath,
                index,
                plan.Artifacts.Count,
                progress,
                cancellationToken);

            downloads.Add(new DownloadedArtifact(artifact, targetPath));
        }

        return downloads;
    }

    private async Task DownloadFileAsync(
        MediaInfoArtifact artifact,
        string targetPath,
        int artifactIndex,
        int artifactCount,
        IProgress<ToolInstallationProgress>? progress,
        CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TDMaker", "5.0.0"));

        using var response = await client.GetAsync(
            artifact.DownloadUrl,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var contentLength = response.Content.Headers.ContentLength;

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using var destinationStream = File.Create(targetPath);

        var buffer = new byte[81920];
        long downloadedBytes = 0;
        int bytesRead;

        while ((bytesRead = await responseStream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            downloadedBytes += bytesRead;

            double? percentage = null;
            if (contentLength.HasValue && contentLength.Value > 0)
            {
                var phaseBase = artifactIndex * 72d / artifactCount;
                var phaseSpan = 72d / artifactCount;
                percentage = Math.Clamp(
                    phaseBase + downloadedBytes * phaseSpan / contentLength.Value,
                    0,
                    72);
            }

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.MediaInfo,
                Status = contentLength.HasValue && contentLength.Value > 0
                    ? $"Downloading {artifact.DisplayName}... {downloadedBytes / 1024d / 1024d:0.0} / {contentLength.Value / 1024d / 1024d:0.0} MB"
                    : $"Downloading {artifact.DisplayName}... {downloadedBytes / 1024d / 1024d:0.0} MB",
                Percentage = percentage
            });
        }
    }

    private async Task<string> StageInstallationAsync(
        MediaInfoInstallPlan plan,
        IReadOnlyList<DownloadedArtifact> downloads,
        string stagingToolsRoot,
        IProgress<ToolInstallationProgress>? progress,
        CancellationToken cancellationToken)
    {
        return plan.PackageKind switch
        {
            MediaInfoPackageKind.WindowsZip => StageWindows(downloads[0], stagingToolsRoot, progress),
            MediaInfoPackageKind.LinuxDebBundle => await StageLinuxAsync(downloads, stagingToolsRoot, progress, cancellationToken),
            MediaInfoPackageKind.MacDmg => await StageMacAsync(downloads[0], stagingToolsRoot, progress, cancellationToken),
            _ => throw new InvalidOperationException($"Unsupported MediaInfo package kind '{plan.PackageKind}'.")
        };
    }

    private static string StageWindows(
        DownloadedArtifact archive,
        string stagingToolsRoot,
        IProgress<ToolInstallationProgress>? progress)
    {
        progress?.Report(new ToolInstallationProgress
        {
            Kind = ToolKind.MediaInfo,
            Status = "Extracting MediaInfo into the managed tools directory...",
            Percentage = 84
        });

        using var zip = ZipFile.OpenRead(archive.LocalPath);
        foreach (var entry in zip.Entries)
        {
            var destinationPath = Path.Combine(stagingToolsRoot, entry.FullName);
            if (string.IsNullOrEmpty(entry.Name))
            {
                Directory.CreateDirectory(destinationPath);
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
            entry.ExtractToFile(destinationPath, overwrite: true);
        }

        var executablePath = FindFile(
            stagingToolsRoot,
            path => path.EndsWith("MediaInfo.exe", StringComparison.OrdinalIgnoreCase));

        if (executablePath is null)
        {
            throw new InvalidOperationException("Downloaded MediaInfo archive did not contain MediaInfo.exe.");
        }

        return executablePath;
    }

    private async Task<string> StageLinuxAsync(
        IReadOnlyList<DownloadedArtifact> downloads,
        string stagingToolsRoot,
        IProgress<ToolInstallationProgress>? progress,
        CancellationToken cancellationToken)
    {
        progress?.Report(new ToolInstallationProgress
        {
            Kind = ToolKind.MediaInfo,
            Status = "Extracting MediaInfo Linux packages into the managed tools directory...",
            Percentage = 82
        });

        var managedRoot = Path.Combine(stagingToolsRoot, "mediainfo-managed");
        Directory.CreateDirectory(managedRoot);

        foreach (var download in downloads)
        {
            await ExtractDebianPayloadAsync(download.LocalPath, managedRoot, cancellationToken);
        }

        var executablePath = FindFile(
            managedRoot,
            path => path.EndsWith($"{Path.DirectorySeparatorChar}usr{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}mediainfo", StringComparison.Ordinal));

        if (executablePath is null)
        {
            throw new InvalidOperationException("Downloaded MediaInfo Linux packages did not contain usr/bin/mediainfo.");
        }

        var libraryDirectories = Directory
            .EnumerateFiles(managedRoot, "libmediainfo.so*", SearchOption.AllDirectories)
            .Concat(Directory.EnumerateFiles(managedRoot, "libzen.so*", SearchOption.AllDirectories))
            .Select(Path.GetDirectoryName)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var wrapperPath = Path.Combine(stagingToolsRoot, "mediainfo");
        await File.WriteAllTextAsync(
            wrapperPath,
            BuildUnixWrapper(
                wrapperPath,
                executablePath,
                libraryDirectories!,
                "LD_LIBRARY_PATH"),
            cancellationToken);

        EnsureExecutable(wrapperPath);
        return wrapperPath;
    }

    private async Task<string> StageMacAsync(
        DownloadedArtifact dmg,
        string stagingToolsRoot,
        IProgress<ToolInstallationProgress>? progress,
        CancellationToken cancellationToken)
    {
        progress?.Report(new ToolInstallationProgress
        {
            Kind = ToolKind.MediaInfo,
            Status = "Mounting the MediaInfo macOS image...",
            Percentage = 80
        });

        var managedRoot = Path.Combine(stagingToolsRoot, "mediainfo-managed");
        var mountPoint = Path.Combine(Path.GetTempPath(), $"tdmaker-mediainfo-mount-{Guid.NewGuid():N}");
        Directory.CreateDirectory(managedRoot);
        Directory.CreateDirectory(mountPoint);

        try
        {
            await RunRequiredCommandAsync(
                "hdiutil",
                ["attach", "-nobrowse", "-readonly", "-mountpoint", mountPoint, dmg.LocalPath],
                cancellationToken);

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.MediaInfo,
                Status = "Copying MediaInfo CLI from the mounted disk image...",
                Percentage = 88
            });

            await RunRequiredCommandAsync("ditto", [mountPoint, managedRoot], cancellationToken);
        }
        finally
        {
            try
            {
                await RunRequiredCommandAsync("hdiutil", ["detach", mountPoint, "-force"], cancellationToken);
            }
            catch
            {
            }

            TryDeleteDirectory(mountPoint);
        }

        var executablePath = FindMacExecutable(managedRoot);
        if (executablePath is null)
        {
            throw new InvalidOperationException("Downloaded MediaInfo disk image did not contain a runnable CLI executable.");
        }

        var libraryDirectories = Directory
            .EnumerateFiles(managedRoot, "*.dylib", SearchOption.AllDirectories)
            .Select(Path.GetDirectoryName)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Append(Path.GetDirectoryName(executablePath))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var wrapperPath = Path.Combine(stagingToolsRoot, "mediainfo");
        await File.WriteAllTextAsync(
            wrapperPath,
            BuildUnixWrapper(
                wrapperPath,
                executablePath,
                libraryDirectories!,
                "DYLD_LIBRARY_PATH"),
            cancellationToken);

        EnsureExecutable(wrapperPath);
        return wrapperPath;
    }

    private async Task ExtractDebianPayloadAsync(
        string debPackagePath,
        string destinationRoot,
        CancellationToken cancellationToken)
    {
        var extractRoot = Path.Combine(Path.GetTempPath(), $"tdmaker-mediainfo-deb-{Guid.NewGuid():N}");
        Directory.CreateDirectory(extractRoot);

        try
        {
            var extracted = await TryExtractDebWithToolAsync(
                "bsdtar",
                ["-xf", debPackagePath, "-C", extractRoot],
                cancellationToken);

            if (!extracted)
            {
                extracted = await TryExtractDebWithToolAsync(
                    "tar",
                    ["-xf", debPackagePath, "-C", extractRoot],
                    cancellationToken);
            }

            if (!extracted)
            {
                extracted = await TryExtractDebWithArAsync(debPackagePath, extractRoot, cancellationToken);
            }

            if (!extracted)
            {
                throw new InvalidOperationException(
                    "Could not unpack the official MediaInfo .deb payload. Install 'bsdtar', 'tar', or 'ar' on this machine.");
            }

            var dataArchive = Directory
                .EnumerateFiles(extractRoot, "data.tar.*", SearchOption.TopDirectoryOnly)
                .OrderBy(path => path, StringComparer.Ordinal)
                .FirstOrDefault();

            if (dataArchive is null)
            {
                throw new InvalidOperationException("The downloaded MediaInfo package did not contain a data.tar payload.");
            }

            await ExtractTarArchiveAsync(dataArchive, destinationRoot, cancellationToken);
        }
        finally
        {
            TryDeleteDirectory(extractRoot);
        }
    }

    private async Task ExtractTarArchiveAsync(
        string archivePath,
        string destinationRoot,
        CancellationToken cancellationToken)
    {
        await RunRequiredCommandAsync("tar", ["-xf", archivePath, "-C", destinationRoot], cancellationToken);
    }

    private async Task<bool> TryExtractDebWithArAsync(
        string debPackagePath,
        string extractRoot,
        CancellationToken cancellationToken)
    {
        var result = await TryRunCommandAsync("ar", ["x", debPackagePath], extractRoot, cancellationToken);
        return result?.ExitCode == 0;
    }

    private async Task<bool> TryExtractDebWithToolAsync(
        string toolName,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var result = await TryRunCommandAsync(toolName, arguments, null, cancellationToken);
        return result?.ExitCode == 0;
    }

    private async Task<ProcessResult?> TryRunCommandAsync(
        string executable,
        IReadOnlyList<string> arguments,
        string? workingDirectory,
        CancellationToken cancellationToken)
    {
        try
        {
            return await processRunner.RunAsync(
                executable,
                arguments,
                workingDirectory,
                cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    private async Task RunRequiredCommandAsync(
        string executable,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var result = await processRunner.RunAsync(executable, arguments, cancellationToken: cancellationToken);
        if (result.ExitCode == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            $"{executable} failed with exit code {result.ExitCode}: {result.StandardError}".Trim());
    }

    private void CommitStagedTools(string stagingToolsRoot)
    {
        foreach (var entry in Directory.EnumerateFileSystemEntries(stagingToolsRoot))
        {
            var destinationPath = Path.Combine(platformPaths.ToolsDirectory, Path.GetFileName(entry));

            if (Directory.Exists(entry))
            {
                TryDeleteDirectory(destinationPath);
                CopyDirectory(entry, destinationPath);
                continue;
            }

            TryDeleteFile(destinationPath);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
            File.Copy(entry, destinationPath, overwrite: true);
        }
    }

    private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        Directory.CreateDirectory(destinationDirectory);

        foreach (var directory in Directory.EnumerateDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceDirectory, directory);
            Directory.CreateDirectory(Path.Combine(destinationDirectory, relativePath));
        }

        foreach (var file in Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceDirectory, file);
            var destinationPath = Path.Combine(destinationDirectory, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
            File.Copy(file, destinationPath, overwrite: true);
        }
    }

    private static string BuildUnixWrapper(
        string wrapperPath,
        string executablePath,
        IReadOnlyList<string> libraryDirectories,
        string environmentVariable)
    {
        var scriptDirectoryReference = "$SCRIPT_DIR";
        var wrapperDirectory = Path.GetDirectoryName(wrapperPath)
            ?? throw new InvalidOperationException("Wrapper path did not resolve to a parent directory.");
        var executableReference = $"{scriptDirectoryReference}/{ToUnixPath(Path.GetRelativePath(wrapperDirectory, executablePath))}";

        var libraryLines = libraryDirectories
            .Distinct(StringComparer.Ordinal)
            .Select(path => $"  \"{scriptDirectoryReference}/{ToUnixPath(Path.GetRelativePath(wrapperDirectory, path))}\"")
            .ToArray();

        var lines = new List<string>
        {
            "#!/usr/bin/env bash",
            "set -euo pipefail",
            "SCRIPT_DIR=\"$(cd \"$(dirname \"${BASH_SOURCE[0]}\")\" && pwd)\"",
            $"EXECUTABLE=\"{executableReference}\""
        };

        if (libraryLines.Length > 0)
        {
            lines.Add("LIB_DIRS=(");
            lines.AddRange(libraryLines);
            lines.Add(")");
            lines.Add("LIB_PATH=\"$(IFS=:; echo \"${LIB_DIRS[*]}\")\"");
            lines.Add($"if [ -n \"${{{environmentVariable}:-}}\" ]; then");
            lines.Add($"  export {environmentVariable}=\"$LIB_PATH:${{{environmentVariable}}}\"");
            lines.Add("else");
            lines.Add($"  export {environmentVariable}=\"$LIB_PATH\"");
            lines.Add("fi");
        }

        lines.Add("exec \"$EXECUTABLE\" \"$@\"");
        return string.Join('\n', lines) + "\n";
    }

    private static string? FindMacExecutable(string managedRoot)
    {
        var candidates = Directory
            .EnumerateFiles(managedRoot, "*", SearchOption.AllDirectories)
            .Where(path =>
            {
                var fileName = Path.GetFileName(path);
                return fileName.Equals("mediainfo", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Equals("MediaInfo", StringComparison.OrdinalIgnoreCase);
            })
            .OrderBy(path => ScoreMacExecutable(path))
            .ToArray();

        return candidates.FirstOrDefault();
    }

    private static int ScoreMacExecutable(string path)
    {
        var unixPath = ToUnixPath(path);
        if (unixPath.Contains("/Contents/MacOS/", StringComparison.Ordinal))
        {
            return 0;
        }

        if (unixPath.EndsWith("/mediainfo", StringComparison.OrdinalIgnoreCase))
        {
            return 1;
        }

        return 2;
    }

    private static string? FindFile(string root, Func<string, bool> predicate)
    {
        return Directory
            .EnumerateFiles(root, "*", SearchOption.AllDirectories)
            .FirstOrDefault(predicate);
    }

    private static string ToUnixPath(string path)
    {
        return path.Replace('\\', '/');
    }

    private static void EnsureExecutable(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        File.SetUnixFileMode(
            path,
            UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
            UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
            UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
    }

    private static void TryDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
        }
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch
        {
        }
    }

    private static MediaInfoInstallPlan ResolvePlan()
    {
        if (OperatingSystem.IsWindows())
        {
            return RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => new MediaInfoInstallPlan(
                    MediaInfoPackageKind.WindowsZip,
                    "MediaArea Windows CLI ZIP",
                    [new MediaInfoArtifact("MediaInfo CLI for Windows x64", WindowsX64Url, Path.GetFileName(new Uri(WindowsX64Url).LocalPath))]),
                Architecture.Arm64 => new MediaInfoInstallPlan(
                    MediaInfoPackageKind.WindowsZip,
                    "MediaArea Windows CLI ZIP",
                    [new MediaInfoArtifact("MediaInfo CLI for Windows ARM64", WindowsArm64Url, Path.GetFileName(new Uri(WindowsArm64Url).LocalPath))]),
                _ => throw new PlatformNotSupportedException(
                    $"Automatic MediaInfo download is not supported for Windows architecture '{RuntimeInformation.OSArchitecture}'.")
            };
        }

        if (OperatingSystem.IsLinux())
        {
            return RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => new MediaInfoInstallPlan(
                    MediaInfoPackageKind.LinuxDebBundle,
                    "MediaArea Ubuntu 24.04 CLI packages",
                    [
                        new MediaInfoArtifact("MediaInfo CLI", LinuxAmd64CliUrl, Path.GetFileName(new Uri(LinuxAmd64CliUrl).LocalPath)),
                        new MediaInfoArtifact("libmediainfo", LinuxAmd64LibMediaInfoUrl, Path.GetFileName(new Uri(LinuxAmd64LibMediaInfoUrl).LocalPath)),
                        new MediaInfoArtifact("libzen", LinuxAmd64LibZenUrl, Path.GetFileName(new Uri(LinuxAmd64LibZenUrl).LocalPath))
                    ]),
                Architecture.Arm64 => new MediaInfoInstallPlan(
                    MediaInfoPackageKind.LinuxDebBundle,
                    "MediaArea Ubuntu 24.04 CLI packages",
                    [
                        new MediaInfoArtifact("MediaInfo CLI", LinuxArm64CliUrl, Path.GetFileName(new Uri(LinuxArm64CliUrl).LocalPath)),
                        new MediaInfoArtifact("libmediainfo", LinuxArm64LibMediaInfoUrl, Path.GetFileName(new Uri(LinuxArm64LibMediaInfoUrl).LocalPath)),
                        new MediaInfoArtifact("libzen", LinuxArm64LibZenUrl, Path.GetFileName(new Uri(LinuxArm64LibZenUrl).LocalPath))
                    ]),
                _ => throw new PlatformNotSupportedException(
                    $"Automatic MediaInfo download is not supported for Linux architecture '{RuntimeInformation.OSArchitecture}'.")
            };
        }

        if (OperatingSystem.IsMacOS())
        {
            return new MediaInfoInstallPlan(
                MediaInfoPackageKind.MacDmg,
                "MediaArea macOS CLI disk image",
                [new MediaInfoArtifact("MediaInfo CLI for macOS", MacUrl, Path.GetFileName(new Uri(MacUrl).LocalPath))]);
        }

        throw new PlatformNotSupportedException("Automatic MediaInfo download is only supported on Windows, Linux, and macOS.");
    }

    private static ToolInstallationResult Failure(
        string message,
        string? downloadUrl = null,
        string? installedPath = null)
    {
        return new ToolInstallationResult
        {
            Kind = ToolKind.MediaInfo,
            Success = false,
            Message = message,
            InstalledPath = installedPath,
            DownloadUrl = downloadUrl
        };
    }

    [LoggerMessage(
        EventId = 3201,
        Level = LogLevel.Warning,
        Message = "MediaInfo auto-install is not supported on this platform. {Message}")]
    private static partial void LogUnsupportedPlatform(ILogger logger, Exception ex, string message);

    [LoggerMessage(
        EventId = 3202,
        Level = LogLevel.Warning,
        Message = "Managed MediaInfo validation failed with exit code {ExitCode}. stderr: {StandardError}")]
    private static partial void LogValidationFailed(ILogger logger, int exitCode, string standardError);

    [LoggerMessage(
        EventId = 3203,
        Level = LogLevel.Information,
        Message = "MediaInfo installation was canceled.")]
    private static partial void LogCanceled(ILogger logger);

    [LoggerMessage(
        EventId = 3204,
        Level = LogLevel.Error,
        Message = "MediaInfo installation failed.")]
    private static partial void LogInstallFailed(ILogger logger, Exception ex);

    private sealed record MediaInfoInstallPlan(
        MediaInfoPackageKind PackageKind,
        string ProviderName,
        IReadOnlyList<MediaInfoArtifact> Artifacts);

    private sealed record MediaInfoArtifact(
        string DisplayName,
        string DownloadUrl,
        string FileName);

    private sealed record DownloadedArtifact(MediaInfoArtifact Artifact, string LocalPath);

    private enum MediaInfoPackageKind
    {
        WindowsZip,
        LinuxDebBundle,
        MacDmg
    }
}
