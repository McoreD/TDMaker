namespace TDMaker.Core.Models;

public sealed class MediaGeneralInfo
{
    public string? CompleteName { get; set; }
    public string? Format { get; set; }
    public string? FormatInfo { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? FileSizeDisplay { get; set; }
    public double? DurationMilliseconds { get; set; }
    public string? DurationDisplay { get; set; }
    public string? DurationDisplayShort { get; set; }
    public string? OverallBitRate { get; set; }
    public string? EncodedDate { get; set; }
    public string? EncodedApplication { get; set; }
    public string? WritingLibrary { get; set; }
    public string? Subtitles { get; set; }
    public Dictionary<string, string> Fields { get; } = new(StringComparer.OrdinalIgnoreCase);
}
