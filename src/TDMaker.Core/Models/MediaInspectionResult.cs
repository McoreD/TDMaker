namespace TDMaker.Core.Models;

public sealed class MediaInspectionResult
{
    public required IReadOnlyList<string> Inputs { get; init; }
    public required MediaInputKind InputKind { get; init; }
    public required string Title { get; set; }
    public required string SourceLabel { get; set; }
    public required string OutputName { get; set; }
    public string? DiscLabel { get; set; }
    public string? TemplateDirectory { get; set; }
    public MediaAsset PrimaryAsset { get; set; } = null!;
    public List<MediaAsset> Assets { get; } = [];
    public List<string> Warnings { get; } = [];

    public IEnumerable<ScreenshotArtifact> AllScreenshots => Assets.SelectMany(x => x.Screenshots);
}
