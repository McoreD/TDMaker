namespace TDMaker.Infrastructure.Media;

using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;
using TDMaker.Infrastructure.Support;

public sealed class MediaInfoCliInspector(
    IExternalToolLocator toolLocator,
    IProcessRunner processRunner,
    ILogger<MediaInfoCliInspector> logger) : IMediaInspector
{
    public async Task<MediaInspectionResult> InspectAsync(
        ReleaseRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var mediaInfo = toolLocator.Resolve(ToolKind.MediaInfo, request.Tools);
        if (!mediaInfo.IsConfigured)
        {
            throw new InvalidOperationException("MediaInfo CLI could not be found. Configure mediainfo before running a release.");
        }

        var inputs = request.Inputs.Select(Path.GetFullPath).ToArray();
        var inputKind = DetermineInputKind(inputs, request.Profile);
        var files = EnumerateFiles(inputs, inputKind, request.Profile).ToArray();

        if (files.Length == 0)
        {
            throw new InvalidOperationException("No supported media files were found in the supplied input.");
        }

        var assets = new List<MediaAsset>(files.Length);
        foreach (var file in files)
        {
            assets.Add(await InspectFileAsync(mediaInfo.Path!, file, files, cancellationToken));
        }

        var primaryAsset = assets
            .OrderByDescending(x => x.General.FileSizeBytes ?? 0)
            .First();

        var title = request.TitleOverride ?? GuessTitle(inputs, inputKind, primaryAsset);
        var source = request.SourceOverride ?? GuessSource(inputs, title, inputKind, request.Profile.SourceLabel);

        var inspection = new MediaInspectionResult
        {
            Inputs = inputs,
            InputKind = inputKind,
            Title = title,
            SourceLabel = source,
            OutputName = StringUtilities.SanitizeFileName(title),
            PrimaryAsset = primaryAsset
        };

        inspection.Assets.AddRange(assets);

        if (inputKind == MediaInputKind.Disc)
        {
            inspection.DiscLabel = source;
        }

        logger.LogInformation(
            "Inspected {Count} media assets as {InputKind} using MediaInfo",
            inspection.Assets.Count,
            inspection.InputKind);

        return inspection;
    }

    private async Task<MediaAsset> InspectFileAsync(
        string mediaInfoPath,
        string filePath,
        IReadOnlyList<string> allFiles,
        CancellationToken cancellationToken)
    {
        var jsonResult = await processRunner.RunAsync(
            mediaInfoPath,
            ["--Output=JSON", filePath],
            cancellationToken: cancellationToken);

        if (jsonResult.ExitCode != 0)
        {
            throw new InvalidOperationException($"MediaInfo JSON inspection failed for '{filePath}': {jsonResult.StandardError}");
        }

        var summaryResult = await processRunner.RunAsync(
            mediaInfoPath,
            [filePath],
            cancellationToken: cancellationToken);

        var fullSummaryResult = await processRunner.RunAsync(
            mediaInfoPath,
            ["--Full", filePath],
            cancellationToken: cancellationToken);

        return ParseAsset(
            filePath,
            Path.GetRelativePath(Path.GetDirectoryName(allFiles[0]) ?? Environment.CurrentDirectory, filePath),
            jsonResult.StandardOutput,
            summaryResult.StandardOutput,
            fullSummaryResult.StandardOutput);
    }

    private static MediaAsset ParseAsset(
        string filePath,
        string relativePath,
        string json,
        string summary,
        string fullSummary)
    {
        using var document = JsonDocument.Parse(json);
        var tracks = document.RootElement.GetProperty("media").GetProperty("track").EnumerateArray().ToArray();

        var generalTrack = tracks.FirstOrDefault(x => GetTrackType(x) == "General");
        var videoTrack = tracks.FirstOrDefault(x => GetTrackType(x) == "Video");
        var audioTracks = tracks.Where(x => GetTrackType(x) == "Audio").ToArray();
        var textTracks = tracks.Where(x => GetTrackType(x) == "Text").ToArray();

        var asset = new MediaAsset
        {
            FilePath = filePath,
            RelativePath = relativePath,
            FileName = Path.GetFileName(filePath),
            FileExtension = Path.GetExtension(filePath),
            SummaryText = summary.Trim(),
            SummaryTextComplete = fullSummary.Trim()
        };

        asset.General = CreateGeneralInfo(generalTrack);
        asset.Video = CreateVideoInfo(videoTrack);

        foreach (var audioTrack in audioTracks.Select(CreateAudioInfo))
        {
            asset.AudioTracks.Add(audioTrack);
        }

        foreach (var subtitle in textTracks
                     .Select(x => GetValue(x, "Language_String") ?? GetValue(x, "Language"))
                     .Where(x => !string.IsNullOrWhiteSpace(x))
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            asset.SubtitleLanguages.Add(subtitle!);
        }

        asset.General.Subtitles = asset.SubtitleLanguages.Count == 0
            ? "None"
            : string.Join(", ", asset.SubtitleLanguages);

        return asset;
    }

    private static MediaGeneralInfo CreateGeneralInfo(JsonElement track)
    {
        if (track.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return new MediaGeneralInfo();
        }

        var info = new MediaGeneralInfo
        {
            CompleteName = GetValue(track, "CompleteName"),
            Format = GetValue(track, "Format"),
            FormatInfo = GetValue(track, "Format_Info"),
            OverallBitRate = GetValue(track, "OverallBitRate_String") ?? GetValue(track, "OverallBitRate"),
            EncodedDate = GetValue(track, "Encoded_Date"),
            EncodedApplication = GetValue(track, "Encoded_Application"),
            WritingLibrary = GetValue(track, "Encoded_Library")
        };

        foreach (var property in track.EnumerateObject())
        {
            info.Fields[property.Name] = property.Value.ToString();
        }

        if (long.TryParse(GetValue(track, "FileSize"), NumberStyles.Any, CultureInfo.InvariantCulture, out var fileSize))
        {
            info.FileSizeBytes = fileSize;
            info.FileSizeDisplay = StringUtilities.HumanizeBytes(fileSize);
        }

        if (double.TryParse(GetValue(track, "Duration"), NumberStyles.Any, CultureInfo.InvariantCulture, out var duration))
        {
            info.DurationMilliseconds = duration;
            info.DurationDisplay = StringUtilities.HumanizeDuration(duration);
            info.DurationDisplayShort = StringUtilities.HumanizeDuration(duration);
        }

        return info;
    }

    private static VideoTrackInfo CreateVideoInfo(JsonElement track)
    {
        if (track.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return new VideoTrackInfo();
        }

        var info = new VideoTrackInfo
        {
            Format = GetValue(track, "Format"),
            FormatVersion = GetValue(track, "Format_Version"),
            Codec = GetValue(track, "Encoded_Library") ?? GetValue(track, "CodecID_Hint") ?? GetValue(track, "CodecID"),
            BitRate = GetValue(track, "BitRate_String") ?? GetValue(track, "BitRate"),
            Standard = GetValue(track, "Standard"),
            FrameRate = GetValue(track, "FrameRate_String") ?? GetValue(track, "FrameRate"),
            ScanType = GetValue(track, "ScanType_String") ?? GetValue(track, "ScanType"),
            Width = GetValue(track, "Width"),
            Height = GetValue(track, "Height"),
            DisplayAspectRatio = GetValue(track, "DisplayAspectRatio_String") ?? GetValue(track, "DisplayAspectRatio"),
            EncodedLibrarySettings = GetValue(track, "Encoded_Library_Settings")
        };

        if (!string.IsNullOrWhiteSpace(info.Width) && !string.IsNullOrWhiteSpace(info.Height))
        {
            info.Resolution = $"{info.Width}x{info.Height}";
        }

        foreach (var property in track.EnumerateObject())
        {
            info.Fields[property.Name] = property.Value.ToString();
        }

        return info;
    }

    private static AudioTrackInfo CreateAudioInfo(JsonElement track)
    {
        if (track.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return new AudioTrackInfo();
        }

        var info = new AudioTrackInfo
        {
            Index = int.TryParse(GetValue(track, "@typeorder"), out var index) ? Math.Max(index - 1, 0) : 0,
            Format = GetValue(track, "Format"),
            FormatVersion = GetValue(track, "Format_Version"),
            FormatProfile = GetValue(track, "Format_Profile"),
            CodecId = GetValue(track, "CodecID_Hint") ?? GetValue(track, "CodecID"),
            BitRate = GetValue(track, "BitRate_String") ?? GetValue(track, "BitRate"),
            BitRateMode = GetValue(track, "BitRate_Mode_String") ?? GetValue(track, "BitRate_Mode"),
            Channels = GetValue(track, "Channel_s__String") ?? GetValue(track, "Channel(s)_String"),
            SamplingRate = GetValue(track, "SamplingRate_String") ?? GetValue(track, "SamplingRate"),
            Resolution = GetValue(track, "Resolution_String") ?? GetValue(track, "BitDepth_String")
        };

        foreach (var property in track.EnumerateObject())
        {
            info.Fields[property.Name] = property.Value.ToString();
        }

        return info;
    }

    private static IEnumerable<string> EnumerateFiles(
        IReadOnlyList<string> inputs,
        MediaInputKind inputKind,
        ReleaseProfile profile)
    {
        var allowed = profile.VideoExtensions
            .Concat(profile.AudioExtensions)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (inputKind == MediaInputKind.SingleFile)
        {
            return inputs;
        }

        var files = new List<string>();

        foreach (var input in inputs)
        {
            if (File.Exists(input))
            {
                files.Add(input);
                continue;
            }

            if (!Directory.Exists(input))
            {
                continue;
            }

            if (inputKind == MediaInputKind.Disc)
            {
                files.AddRange(EnumerateDiscFiles(input));
            }
            else
            {
                files.AddRange(Directory.EnumerateFiles(input, "*.*", SearchOption.AllDirectories)
                    .Where(path => allowed.Contains(Path.GetExtension(path))));
            }
        }

        return files
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
    }

    private static IEnumerable<string> EnumerateDiscFiles(string root)
    {
        if (Directory.Exists(Path.Combine(root, "VIDEO_TS")))
        {
            return Directory.EnumerateFiles(root, "*.vob", SearchOption.AllDirectories);
        }

        if (Directory.Exists(Path.Combine(root, "BDMV")) || root.EndsWith("BDMV", StringComparison.OrdinalIgnoreCase))
        {
            return Directory.EnumerateFiles(root, "*.m2ts", SearchOption.AllDirectories);
        }

        return Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
            .Where(path => new[] { ".m2ts", ".mpls", ".vob", ".ifo" }.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase));
    }

    private static MediaInputKind DetermineInputKind(IReadOnlyList<string> inputs, ReleaseProfile profile)
    {
        if (inputs.Count == 1 && File.Exists(inputs[0]))
        {
            return MediaInputKind.SingleFile;
        }

        if (inputs.Count == 1 && Directory.Exists(inputs[0]))
        {
            if (IsDisc(inputs[0]))
            {
                return MediaInputKind.Disc;
            }

            var files = Directory.EnumerateFiles(inputs[0], "*.*", SearchOption.AllDirectories)
                .Where(path => profile.VideoExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase)
                               || profile.AudioExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
                .ToArray();

            if (files.Length > 0 && files.All(path => profile.AudioExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase)))
            {
                return MediaInputKind.AudioAlbum;
            }
        }

        if (inputs.Count > 0 && inputs.All(File.Exists))
        {
            return inputs.All(path => profile.AudioExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
                ? MediaInputKind.AudioAlbum
                : MediaInputKind.FileCollection;
        }

        return MediaInputKind.FileCollection;
    }

    private static bool IsDisc(string path)
    {
        return Directory.Exists(Path.Combine(path, "VIDEO_TS"))
               || Directory.Exists(Path.Combine(path, "BDMV"))
               || path.EndsWith("VIDEO_TS", StringComparison.OrdinalIgnoreCase)
               || path.EndsWith("BDMV", StringComparison.OrdinalIgnoreCase);
    }

    private static string GuessTitle(IReadOnlyList<string> inputs, MediaInputKind inputKind, MediaAsset primaryAsset)
    {
        if (inputKind == MediaInputKind.SingleFile)
        {
            return Path.GetFileNameWithoutExtension(inputs[0]);
        }

        if (inputKind == MediaInputKind.Disc)
        {
            var directory = Directory.Exists(inputs[0]) ? inputs[0] : Path.GetDirectoryName(inputs[0]) ?? inputs[0];
            var name = new DirectoryInfo(directory).Name;
            return string.Equals(name, "VIDEO_TS", StringComparison.OrdinalIgnoreCase)
                ? Directory.GetParent(directory)?.Name ?? name
                : name;
        }

        if (inputs.Count == 1 && Directory.Exists(inputs[0]))
        {
            return new DirectoryInfo(inputs[0]).Name;
        }

        return Path.GetFileNameWithoutExtension(primaryAsset.FileName);
    }

    private static string GuessSource(
        IReadOnlyList<string> inputs,
        string title,
        MediaInputKind inputKind,
        string fallback)
    {
        var haystack = string.Join(' ', inputs.Append(title));
        if (inputKind == MediaInputKind.Disc && haystack.Contains("VIDEO_TS", StringComparison.OrdinalIgnoreCase))
        {
            return "DVD";
        }

        if (inputKind == MediaInputKind.Disc && haystack.Contains("BDMV", StringComparison.OrdinalIgnoreCase))
        {
            return "Blu-ray";
        }

        if (haystack.Contains("BDRip", StringComparison.OrdinalIgnoreCase)
            || haystack.Contains("Blu", StringComparison.OrdinalIgnoreCase))
        {
            return "Blu-ray";
        }

        if (haystack.Contains("DVD", StringComparison.OrdinalIgnoreCase))
        {
            return "DVD";
        }

        if (haystack.Contains("HDTV", StringComparison.OrdinalIgnoreCase))
        {
            return "HDTV";
        }

        if (haystack.Contains("WEB", StringComparison.OrdinalIgnoreCase))
        {
            return "WEB";
        }

        if (haystack.Contains("TV", StringComparison.OrdinalIgnoreCase))
        {
            return "TV";
        }

        return fallback;
    }

    private static string? GetValue(JsonElement element, string propertyName)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                return property.Value.ToString();
            }
        }

        return null;
    }

    private static string? GetTrackType(JsonElement track)
    {
        return GetValue(track, "@type");
    }
}
