namespace TDMaker.Core.Models;

public sealed class ReleaseProfile
{
    public const string DefaultMovieProfileId = "movies";
    public const string DefaultMusicProfileId = "music";

    public string Id { get; set; } = DefaultMovieProfileId;
    public string Name { get; set; } = "Movies";
    public string SourceLabel { get; set; } = "Blu-ray";
    public string PublishPreset { get; set; } = "Default";
    public string? CustomTemplateDirectory { get; set; }
    public string? OutputDirectory { get; set; }
    public string? PtpImgApiKey { get; set; }
    public bool CreateScreenshots { get; set; } = true;
    public bool UploadScreenshots { get; set; }
    public bool CombineScreenshots { get; set; }
    public bool CreateTorrent { get; set; } = true;
    public bool ExportXml { get; set; }
    public bool WritePublishText { get; set; } = true;
    public bool WrapPublishInPreBlock { get; set; }
    public bool CenterPublishText { get; set; }
    public bool UseFullSizeImages { get; set; } = true;
    public bool HidePrivatePaths { get; set; } = true;
    public bool RandomizeScreenshotFrames { get; set; } = true;
    public int ScreenshotCount { get; set; } = 6;
    public int ScreenshotColumns { get; set; } = 3;
    public int Heading1FontSize { get; set; } = 5;
    public int Heading2FontSize { get; set; } = 4;
    public int Heading3FontSize { get; set; } = 3;
    public int BodyFontSize { get; set; } = 2;
    public int FontSizeIncrement { get; set; } = 1;
    public int? TorrentPieceLengthKiB { get; set; }
    public List<TrackerDefinition> Trackers { get; set; } = [];
    public List<string> VideoExtensions { get; set; } =
    [
        ".mkv",
        ".mp4",
        ".avi",
        ".m2ts",
        ".ts",
        ".mov",
        ".vob",
        ".mpg",
        ".mpeg"
    ];

    public List<string> AudioExtensions { get; set; } =
    [
        ".flac",
        ".mp3",
        ".m4a",
        ".aac",
        ".ogg",
        ".opus",
        ".wav"
    ];

    public static ReleaseProfile CreateMovieProfile()
    {
        return new ReleaseProfile();
    }

    public static ReleaseProfile CreateMusicProfile()
    {
        return new ReleaseProfile
        {
            Id = DefaultMusicProfileId,
            Name = "Music Videos",
            SourceLabel = "WEB",
            PublishPreset = "MTN",
            ScreenshotCount = 16,
            ScreenshotColumns = 4,
            CombineScreenshots = true
        };
    }

    public ReleaseProfile Clone()
    {
        return new ReleaseProfile
        {
            Id = Id,
            Name = Name,
            SourceLabel = SourceLabel,
            PublishPreset = PublishPreset,
            CustomTemplateDirectory = CustomTemplateDirectory,
            OutputDirectory = OutputDirectory,
            PtpImgApiKey = PtpImgApiKey,
            CreateScreenshots = CreateScreenshots,
            UploadScreenshots = UploadScreenshots,
            CombineScreenshots = CombineScreenshots,
            CreateTorrent = CreateTorrent,
            ExportXml = ExportXml,
            WritePublishText = WritePublishText,
            WrapPublishInPreBlock = WrapPublishInPreBlock,
            CenterPublishText = CenterPublishText,
            UseFullSizeImages = UseFullSizeImages,
            HidePrivatePaths = HidePrivatePaths,
            RandomizeScreenshotFrames = RandomizeScreenshotFrames,
            ScreenshotCount = ScreenshotCount,
            ScreenshotColumns = ScreenshotColumns,
            Heading1FontSize = Heading1FontSize,
            Heading2FontSize = Heading2FontSize,
            Heading3FontSize = Heading3FontSize,
            BodyFontSize = BodyFontSize,
            FontSizeIncrement = FontSizeIncrement,
            TorrentPieceLengthKiB = TorrentPieceLengthKiB,
            Trackers = Trackers.Select(x => new TrackerDefinition
            {
                Name = x.Name,
                AnnounceUrl = x.AnnounceUrl,
                SourceFlag = x.SourceFlag,
                Enabled = x.Enabled
            }).ToList(),
            VideoExtensions = [.. VideoExtensions],
            AudioExtensions = [.. AudioExtensions]
        };
    }
}
