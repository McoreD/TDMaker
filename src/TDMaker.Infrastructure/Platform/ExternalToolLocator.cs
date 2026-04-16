namespace TDMaker.Infrastructure.Platform;

using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed class ExternalToolLocator(IPlatformPaths platformPaths) : IExternalToolLocator
{
    public IReadOnlyList<ToolResolution> ResolveAll(ToolSettings settings)
    {
        return
        [
            Resolve(ToolKind.FFmpeg, settings),
            Resolve(ToolKind.MediaInfo, settings)
        ];
    }

    public ToolResolution Resolve(ToolKind toolKind, ToolSettings settings)
    {
        var configured = toolKind switch
        {
            ToolKind.FFmpeg => settings.FFmpegPath,
            ToolKind.MediaInfo => settings.MediaInfoPath,
            _ => null
        };

        var candidate = ResolvePath(configured, GetCandidateNames(toolKind));

        return new ToolResolution
        {
            Kind = toolKind,
            DisplayName = toolKind.ToString(),
            Path = candidate
        };
    }

    private string? ResolvePath(string? configuredPath, IReadOnlyList<string> candidateNames)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
        {
            return configuredPath;
        }

        foreach (var candidateName in candidateNames)
        {
            var bundled = Path.Combine(platformPaths.ToolsDirectory, candidateName);
            if (File.Exists(bundled))
            {
                return bundled;
            }

            var local = Path.Combine(AppContext.BaseDirectory, candidateName);
            if (File.Exists(local))
            {
                return local;
            }
        }

        var pathEntries = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var entry in pathEntries)
        {
            foreach (var candidateName in candidateNames)
            {
                var probe = Path.Combine(entry, candidateName);
                if (File.Exists(probe))
                {
                    return probe;
                }
            }
        }

        return null;
    }

    private static IReadOnlyList<string> GetCandidateNames(ToolKind toolKind)
    {
        if (OperatingSystem.IsWindows())
        {
            return toolKind switch
            {
                ToolKind.FFmpeg => ["ffmpeg.exe", "ffmpeg.cmd", "ffmpeg.bat"],
                ToolKind.MediaInfo => ["mediainfo.exe", "mediainfo.cmd", "mediainfo.bat"],
                _ => []
            };
        }

        return toolKind switch
        {
            ToolKind.FFmpeg => ["ffmpeg"],
            ToolKind.MediaInfo => ["mediainfo"],
            _ => []
        };
    }
}
