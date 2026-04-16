namespace TDMaker.Infrastructure.Platform;

using System.IO.Compression;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed partial class FFmpegInstaller(
    IHttpClientFactory httpClientFactory,
    IPlatformPaths platformPaths,
    IProcessRunner processRunner,
    ILogger<FFmpegInstaller> logger) : IFFmpegInstaller
{
    private const string GyanWindowsReleaseUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

    public async Task<ToolInstallationResult> InstallLatestAsync(
        IProgress<ToolInstallationProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        FFmpegDownloadPlan plan;

        try
        {
            plan = ResolvePlan();
        }
        catch (PlatformNotSupportedException ex)
        {
            LogUnsupportedPlatform(logger, ex, ex.Message);
            return Failure(ex.Message);
        }

        Directory.CreateDirectory(platformPaths.ToolsDirectory);

        var archivePath = Path.Combine(Path.GetTempPath(), $"tdmaker-ffmpeg-{Guid.NewGuid():N}.zip");

        try
        {
            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.FFmpeg,
                Status = $"Downloading FFmpeg from {plan.ProviderName}...",
                Percentage = 0
            });

            await DownloadArchiveAsync(plan.DownloadUrl, archivePath, progress, cancellationToken);

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.FFmpeg,
                Status = "Extracting FFmpeg into the managed tools directory...",
                Percentage = 92
            });

            var installedPath = ExtractExecutable(archivePath, plan);
            EnsureExecutable(installedPath);

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.FFmpeg,
                Status = "Validating managed FFmpeg...",
                Percentage = 97
            });

            var validation = await processRunner.RunAsync(
                installedPath,
                ["-version"],
                cancellationToken: cancellationToken);

            if (validation.ExitCode != 0)
            {
                LogValidationFailed(logger, validation.ExitCode, validation.StandardError);

                return Failure(
                    "FFmpeg was downloaded but did not start successfully on this machine.",
                    plan.DownloadUrl,
                    installedPath);
            }

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.FFmpeg,
                Status = $"Managed FFmpeg installed at {installedPath}",
                Percentage = 100
            });

            return new ToolInstallationResult
            {
                Kind = ToolKind.FFmpeg,
                Success = true,
                Message = $"Managed FFmpeg installed at {installedPath}.",
                InstalledPath = installedPath,
                DownloadUrl = plan.DownloadUrl
            };
        }
        catch (OperationCanceledException)
        {
            LogCanceled(logger);
            return Failure("FFmpeg installation was canceled.", plan.DownloadUrl);
        }
        catch (Exception ex)
        {
            LogInstallFailed(logger, ex);
            return Failure($"FFmpeg installation failed: {ex.Message}", plan.DownloadUrl);
        }
        finally
        {
            TryDeleteFile(archivePath);
        }
    }

    private async Task DownloadArchiveAsync(
        string downloadUrl,
        string archivePath,
        IProgress<ToolInstallationProgress>? progress,
        CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TDMaker", "5.0.0"));

        using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var contentLength = response.Content.Headers.ContentLength;

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using var destinationStream = File.Create(archivePath);

        var buffer = new byte[81920];
        long downloadedBytes = 0;
        int bytesRead;

        while ((bytesRead = await responseStream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await destinationStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            downloadedBytes += bytesRead;

            double? percentage = contentLength.HasValue && contentLength.Value > 0
                ? Math.Clamp(downloadedBytes * 90d / contentLength.Value, 0, 90)
                : null;

            progress?.Report(new ToolInstallationProgress
            {
                Kind = ToolKind.FFmpeg,
                Status = contentLength.HasValue && contentLength.Value > 0
                    ? $"Downloading FFmpeg... {downloadedBytes / 1024d / 1024d:0.0} / {contentLength.Value / 1024d / 1024d:0.0} MB"
                    : $"Downloading FFmpeg... {downloadedBytes / 1024d / 1024d:0.0} MB",
                Percentage = percentage
            });
        }
    }

    private string ExtractExecutable(string archivePath, FFmpegDownloadPlan plan)
    {
        using var archive = ZipFile.OpenRead(archivePath);
        var executableEntry = archive.Entries.FirstOrDefault(entry =>
            !string.IsNullOrWhiteSpace(entry.Name) &&
            entry.Name.Equals(plan.ExecutableName, StringComparison.OrdinalIgnoreCase));

        if (executableEntry is null)
        {
            throw new InvalidOperationException(
                $"Downloaded FFmpeg archive did not contain {plan.ExecutableName}.");
        }

        var destinationPath = Path.Combine(platformPaths.ToolsDirectory, plan.ExecutableName);
        executableEntry.ExtractToFile(destinationPath, overwrite: true);
        return destinationPath;
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

    private static FFmpegDownloadPlan ResolvePlan()
    {
        if (OperatingSystem.IsWindows())
        {
            return new FFmpegDownloadPlan(
                GyanWindowsReleaseUrl,
                "ffmpeg.exe",
                RuntimeInformation.OSArchitecture == Architecture.Arm64
                    ? "gyan.dev release essentials (x64 fallback for Windows ARM64)"
                    : "gyan.dev release essentials");
        }

        if (OperatingSystem.IsLinux())
        {
            return RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => new FFmpegDownloadPlan(
                    "https://ffmpeg.martin-riedl.de/redirect/latest/linux/amd64/release/ffmpeg.zip",
                    "ffmpeg",
                    "Martin Riedl release build"),
                Architecture.Arm64 => new FFmpegDownloadPlan(
                    "https://ffmpeg.martin-riedl.de/redirect/latest/linux/arm64/release/ffmpeg.zip",
                    "ffmpeg",
                    "Martin Riedl release build"),
                _ => throw new PlatformNotSupportedException(
                    $"Automatic FFmpeg download is not supported for Linux architecture '{RuntimeInformation.OSArchitecture}'.")
            };
        }

        if (OperatingSystem.IsMacOS())
        {
            return RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => new FFmpegDownloadPlan(
                    "https://ffmpeg.martin-riedl.de/redirect/latest/macos/amd64/release/ffmpeg.zip",
                    "ffmpeg",
                    "Martin Riedl notarized release build"),
                Architecture.Arm64 => new FFmpegDownloadPlan(
                    "https://ffmpeg.martin-riedl.de/redirect/latest/macos/arm64/release/ffmpeg.zip",
                    "ffmpeg",
                    "Martin Riedl notarized release build"),
                _ => throw new PlatformNotSupportedException(
                    $"Automatic FFmpeg download is not supported for macOS architecture '{RuntimeInformation.OSArchitecture}'.")
            };
        }

        throw new PlatformNotSupportedException("Automatic FFmpeg download is only supported on Windows, Linux, and macOS.");
    }

    private static ToolInstallationResult Failure(
        string message,
        string? downloadUrl = null,
        string? installedPath = null)
    {
        return new ToolInstallationResult
        {
            Kind = ToolKind.FFmpeg,
            Success = false,
            Message = message,
            InstalledPath = installedPath,
            DownloadUrl = downloadUrl
        };
    }

    [LoggerMessage(
        EventId = 3101,
        Level = LogLevel.Warning,
        Message = "FFmpeg auto-install is not supported on this platform. {Message}")]
    private static partial void LogUnsupportedPlatform(ILogger logger, Exception ex, string message);

    [LoggerMessage(
        EventId = 3102,
        Level = LogLevel.Warning,
        Message = "Managed FFmpeg validation failed with exit code {ExitCode}. stderr: {StandardError}")]
    private static partial void LogValidationFailed(ILogger logger, int exitCode, string standardError);

    [LoggerMessage(
        EventId = 3103,
        Level = LogLevel.Information,
        Message = "FFmpeg installation was canceled.")]
    private static partial void LogCanceled(ILogger logger);

    [LoggerMessage(
        EventId = 3104,
        Level = LogLevel.Error,
        Message = "FFmpeg installation failed.")]
    private static partial void LogInstallFailed(ILogger logger, Exception ex);

    private sealed record FFmpegDownloadPlan(
        string DownloadUrl,
        string ExecutableName,
        string ProviderName);
}
