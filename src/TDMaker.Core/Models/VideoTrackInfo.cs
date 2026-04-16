namespace TDMaker.Core.Models;

public sealed class VideoTrackInfo
{
    public string? Format { get; set; }
    public string? FormatVersion { get; set; }
    public string? Codec { get; set; }
    public string? BitRate { get; set; }
    public string? Standard { get; set; }
    public string? FrameRate { get; set; }
    public string? ScanType { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public string? Resolution { get; set; }
    public string? DisplayAspectRatio { get; set; }
    public string? EncodedLibrarySettings { get; set; }
    public Dictionary<string, string> Fields { get; } = new(StringComparer.OrdinalIgnoreCase);
}
