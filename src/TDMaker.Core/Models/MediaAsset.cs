namespace TDMaker.Core.Models;

public sealed class MediaAsset
{
    public required string FilePath { get; init; }
    public required string RelativePath { get; init; }
    public required string FileName { get; init; }
    public string FileExtension { get; init; } = string.Empty;
    public string SummaryText { get; set; } = string.Empty;
    public string SummaryTextComplete { get; set; } = string.Empty;
    public MediaGeneralInfo General { get; set; } = new();
    public VideoTrackInfo Video { get; set; } = new();
    public List<AudioTrackInfo> AudioTracks { get; } = [];
    public List<ScreenshotArtifact> Screenshots { get; } = [];
    public List<string> SubtitleLanguages { get; } = [];

    public bool HasVideo => !string.IsNullOrWhiteSpace(Video.Format) || Video.Fields.Count > 0;
    public bool HasAudio => AudioTracks.Count > 0;
    public bool IsAudioOnly => HasAudio && !HasVideo;
}
