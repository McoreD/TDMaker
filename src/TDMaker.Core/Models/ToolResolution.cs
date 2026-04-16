namespace TDMaker.Core.Models;

public sealed class ToolResolution
{
    public required ToolKind Kind { get; init; }
    public required string DisplayName { get; init; }
    public string? Path { get; init; }
    public bool IsConfigured => !string.IsNullOrWhiteSpace(Path);
}
