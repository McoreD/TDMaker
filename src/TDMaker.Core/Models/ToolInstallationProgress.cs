namespace TDMaker.Core.Models;

public sealed class ToolInstallationProgress
{
    public required ToolKind Kind { get; init; }
    public required string Status { get; init; }
    public double? Percentage { get; init; }
}
