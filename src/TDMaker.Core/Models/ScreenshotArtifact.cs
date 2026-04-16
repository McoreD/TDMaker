namespace TDMaker.Core.Models;

public sealed class ScreenshotArtifact
{
    public required string LocalPath { get; init; }
    public required TimeSpan Timestamp { get; init; }
    public string? RemoteUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
}
