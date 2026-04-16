namespace TDMaker.Infrastructure.Publishing;

using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;
using TDMaker.Infrastructure.Support;

public sealed partial class LegacyTemplatePublishService(
    IPlatformPaths platformPaths,
    ILogger<LegacyTemplatePublishService> logger) : IPublishService
{
    public Task<string> RenderAsync(
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var templateRoot = ResolveTemplateRoot(profile);
        var content = inspection.InputKind == MediaInputKind.Disc
            ? RenderDisc(inspection, profile, templateRoot)
            : RenderFiles(inspection, profile, templateRoot);

        if (profile.HidePrivatePaths)
        {
            content = CompleteNameRegex().Replace(content, match => Path.GetFileName(match.Groups["value"].Value));
        }

        content = RemoveUnresolvedTokens(content).Trim();
        logger.LogInformation("Rendered publish text using preset {Preset}", profile.PublishPreset);
        return Task.FromResult(content);
    }

    public async Task<string> WriteXmlAsync(
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        string outputDirectory,
        CancellationToken cancellationToken = default)
    {
        var publishText = await RenderAsync(inspection, profile, cancellationToken);
        var primary = inspection.PrimaryAsset;
        var filePath = Path.Combine(outputDirectory, $"{inspection.OutputName}.xml");

        var document = new XDocument(
            new XElement("TorrentInfo",
                new XElement("Format", GuessFormat(inspection)),
                new XElement("Resolution", GuessResolution(primary)),
                new XElement("Width", primary.Video.Width ?? string.Empty),
                new XElement("Height", primary.Video.Height ?? string.Empty),
                new XElement("Media", inspection.SourceLabel),
                new XElement("FileType", primary.FileExtension.TrimStart('.').ToUpperInvariant()),
                new XElement("ReleaseDescription", publishText),
                new XElement("MediaInfoSummary", primary.SummaryText),
                new XElement("Screenshots",
                    inspection.AllScreenshots
                        .Select(x => x.RemoteUrl ?? x.ThumbnailUrl)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(url => new XElement("Screenshot", url)))));

        Directory.CreateDirectory(outputDirectory);
        await using var stream = File.Create(filePath);
        await document.SaveAsync(stream, SaveOptions.None, cancellationToken);
        return filePath;
    }

    private string RenderDisc(MediaInspectionResult inspection, ReleaseProfile profile, string templateRoot)
    {
        var asset = inspection.PrimaryAsset;
        var parser = new LegacySummaryFieldParser(asset.SummaryText);

        var template = File.ReadAllText(Path.Combine(templateRoot, "Disc.txt"));
        template = template.Replace("%General_Info%", BuildGeneralInfo(asset, templateRoot, parser), StringComparison.OrdinalIgnoreCase);
        template = template.Replace("%Video_Info%", BuildVideoInfo(asset, templateRoot, parser, true), StringComparison.OrdinalIgnoreCase);
        template = template.Replace("%Audio_Info%", BuildAudioInfo(asset, templateRoot, parser, true), StringComparison.OrdinalIgnoreCase);
        template = ReplaceCommonTokens(template, inspection, profile, asset, parser);
        return ApplyPublishWrappers(template, profile);
    }

    private string RenderFiles(MediaInspectionResult inspection, ReleaseProfile profile, string templateRoot)
    {
        var builder = new StringBuilder();
        foreach (var asset in inspection.Assets)
        {
            var parser = new LegacySummaryFieldParser(asset.SummaryText);
            var template = File.ReadAllText(Path.Combine(templateRoot, "File.txt"));
            template = template.Replace("%General_Info%", BuildGeneralInfo(asset, templateRoot, parser), StringComparison.OrdinalIgnoreCase);
            template = template.Replace("%Video_Info%", BuildVideoInfo(asset, templateRoot, parser, false), StringComparison.OrdinalIgnoreCase);
            template = template.Replace("%Audio_Info%", BuildAudioInfo(asset, templateRoot, parser, false), StringComparison.OrdinalIgnoreCase);
            template = ReplaceCommonTokens(template, inspection, profile, asset, parser);
            builder.AppendLine(ApplyPublishWrappers(template, profile));
            builder.AppendLine();
        }

        return builder.ToString();
    }

    private static string BuildGeneralInfo(MediaAsset asset, string templateRoot, LegacySummaryFieldParser parser)
    {
        var template = File.ReadAllText(Path.Combine(templateRoot, "GeneralInfo.txt"));
        template = ReplaceGeneralTokens(template, asset);
        return parser.ReplaceGeneral(template);
    }

    private static string BuildVideoInfo(MediaAsset asset, string templateRoot, LegacySummaryFieldParser parser, bool disc)
    {
        var fileName = disc ? "DiscVideoInfo.txt" : "FileVideoInfo.txt";
        var template = File.ReadAllText(Path.Combine(templateRoot, fileName));
        template = ReplaceVideoTokens(template, asset);
        return parser.ReplaceVideo(template);
    }

    private static string BuildAudioInfo(MediaAsset asset, string templateRoot, LegacySummaryFieldParser parser, bool disc)
    {
        var fileName = disc ? "DiscAudioInfo.txt" : "FileAudioInfo.txt";
        var template = File.ReadAllText(Path.Combine(templateRoot, fileName));
        template = ReplaceAudioTokens(template, asset);
        return parser.ReplaceAudio(template);
    }

    private static string ReplaceCommonTokens(
        string template,
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        MediaAsset asset,
        LegacySummaryFieldParser parser)
    {
        template = parser.ReplaceGeneral(parser.ReplaceVideo(parser.ReplaceAudio(template)));
        template = ReplaceGeneralTokens(template, asset);
        template = ReplaceVideoTokens(template, asset);
        template = ReplaceAudioTokens(template, asset);

        template = template
            .Replace("%Title%", inspection.Title, StringComparison.OrdinalIgnoreCase)
            .Replace("%Source%", inspection.SourceLabel, StringComparison.OrdinalIgnoreCase)
            .Replace("%FileName%", asset.FileName, StringComparison.OrdinalIgnoreCase)
            .Replace("%Disc_Menu%", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Disc_Extras%", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Disc_Authoring%", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%WebLink%", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%ScreenshotFull%", BuildScreenshotMarkup(asset, profile.UseFullSizeImages), StringComparison.OrdinalIgnoreCase)
            .Replace("%ScreenshotForums%", BuildScreenshotMarkup(asset, false), StringComparison.OrdinalIgnoreCase)
            .Replace("%FontSize_Heading1%", profile.Heading1FontSize.ToString(), StringComparison.OrdinalIgnoreCase)
            .Replace("%FontSize_Heading2%", profile.Heading2FontSize.ToString(), StringComparison.OrdinalIgnoreCase)
            .Replace("%FontSize_Heading3%", profile.Heading3FontSize.ToString(), StringComparison.OrdinalIgnoreCase)
            .Replace("%FontSize_Body%", profile.BodyFontSize.ToString(), StringComparison.OrdinalIgnoreCase)
            .Replace("%NewLine%", Environment.NewLine, StringComparison.OrdinalIgnoreCase);

        return template;
    }

    private static string ReplaceGeneralTokens(string template, MediaAsset asset)
    {
        return template
            .Replace("%Format%", asset.General.Format ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Bitrate%", asset.General.OverallBitRate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%FileSize%", asset.General.FileSizeDisplay ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Subtitles%", asset.General.Subtitles ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Duration%", asset.General.DurationDisplay ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%EncodedApplication%", asset.General.EncodedApplication ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%EncodedDate%", asset.General.EncodedDate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%General_FileSize%", asset.General.FileSizeDisplay ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%General_Format%", asset.General.Format ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%General_Duration%", asset.General.DurationDisplay ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%General_OverallBitRate%", asset.General.OverallBitRate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%General_WritingApplication%", asset.General.EncodedApplication ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%General_WritingLibrary%", asset.General.WritingLibrary ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    private static string ReplaceVideoTokens(string template, MediaAsset asset)
    {
        return template
            .Replace("%Video_Codec%", asset.Video.Codec ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_Format%", asset.Video.Format ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_Bitrate%", asset.Video.BitRate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_Standard%", asset.Video.Standard ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_FrameRate%", asset.Video.FrameRate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_ScanType%", asset.Video.ScanType ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_DisplayAspectRatio%", asset.Video.DisplayAspectRatio ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_Width%", asset.Video.Width ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_Height%", asset.Video.Height ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_Resolution%", asset.Video.Resolution ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Video_EncodedLibrarySettings%", asset.Video.EncodedLibrarySettings ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    private static string ReplaceAudioTokens(string template, MediaAsset asset)
    {
        var audio = asset.AudioTracks.FirstOrDefault();
        if (audio is null)
        {
            return template;
        }

        return template
            .Replace("%Audio_Format%", audio.Format ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Audio_Bitrate%", audio.BitRate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Audio_BitrateMode%", audio.BitRateMode ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Audio_Channels%", audio.Channels ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Audio_SamplingRate%", audio.SamplingRate ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("%Audio_Resolution%", audio.Resolution ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildScreenshotMarkup(MediaAsset asset, bool fullSize)
    {
        var urls = asset.Screenshots
            .Select(x => fullSize ? x.RemoteUrl ?? x.LocalPath : x.ThumbnailUrl ?? x.RemoteUrl ?? x.LocalPath)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(url => $"[img]{url}[/img]");

        return string.Join(Environment.NewLine + Environment.NewLine, urls);
    }

    private static string ApplyPublishWrappers(string template, ReleaseProfile profile)
    {
        var result = template;
        if (profile.CenterPublishText)
        {
            result = $"[align=center]{result}[/align]";
        }

        if (profile.WrapPublishInPreBlock)
        {
            result = $"[pre]{result}[/pre]";
        }

        return result;
    }

    private string ResolveTemplateRoot(ReleaseProfile profile)
    {
        if (!string.IsNullOrWhiteSpace(profile.CustomTemplateDirectory) && Directory.Exists(profile.CustomTemplateDirectory))
        {
            return profile.CustomTemplateDirectory;
        }

        var candidate = Path.Combine(platformPaths.BundledTemplatesDirectory, profile.PublishPreset);
        return Directory.Exists(candidate)
            ? candidate
            : Path.Combine(platformPaths.BundledTemplatesDirectory, "Default");
    }

    private static string RemoveUnresolvedTokens(string template)
    {
        return UnresolvedTokenRegex().Replace(template, string.Empty);
    }

    private static string GuessFormat(MediaInspectionResult inspection)
    {
        if (inspection.InputKind == MediaInputKind.Disc)
        {
            return inspection.SourceLabel;
        }

        var codec = inspection.PrimaryAsset.Video.Codec ?? inspection.PrimaryAsset.Video.Format ?? string.Empty;
        if (codec.Contains("x264", StringComparison.OrdinalIgnoreCase)
            || codec.Contains("avc", StringComparison.OrdinalIgnoreCase))
        {
            return "H.264";
        }

        if (codec.Contains("x265", StringComparison.OrdinalIgnoreCase)
            || codec.Contains("hevc", StringComparison.OrdinalIgnoreCase))
        {
            return "H.265";
        }

        return codec;
    }

    private static string GuessResolution(MediaAsset asset)
    {
        if (int.TryParse(asset.Video.Width, out var width))
        {
            return width switch
            {
                >= 1900 => "1080p",
                >= 1200 => "720p",
                >= 700 => "480p",
                _ => asset.Video.Resolution ?? string.Empty
            };
        }

        return asset.Video.Resolution ?? string.Empty;
    }

    [GeneratedRegex("(?<prefix>Complete name\\s*: )(?<value>.+?)(?=\\r?\\n)")]
    private static partial Regex CompleteNameRegex();

    [GeneratedRegex("%[A-Za-z0-9_]+%")]
    private static partial Regex UnresolvedTokenRegex();
}
