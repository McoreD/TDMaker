namespace TDMaker.Core.Models;

public sealed class ReleaseRequest
{
    public required IReadOnlyList<string> Inputs { get; init; }
    public required ReleaseProfile Profile { get; init; }
    public required ToolSettings Tools { get; init; }
    public string? TitleOverride { get; init; }
    public string? SourceOverride { get; init; }
    public string? OutputDirectoryOverride { get; init; }
}
