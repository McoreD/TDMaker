namespace TDMaker.Core.Models;

public sealed class AudioTrackInfo
{
    public int Index { get; set; }
    public string? Format { get; set; }
    public string? FormatVersion { get; set; }
    public string? FormatProfile { get; set; }
    public string? CodecId { get; set; }
    public string? BitRate { get; set; }
    public string? BitRateMode { get; set; }
    public string? Channels { get; set; }
    public string? SamplingRate { get; set; }
    public string? Resolution { get; set; }
    public Dictionary<string, string> Fields { get; } = new(StringComparer.OrdinalIgnoreCase);
}
