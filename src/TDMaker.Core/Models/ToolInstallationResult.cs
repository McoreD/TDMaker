namespace TDMaker.Core.Models;

public sealed class ToolInstallationResult
{
    public required ToolKind Kind { get; init; }
    public required bool Success { get; init; }
    public required string Message { get; init; }
    public string? InstalledPath { get; init; }
    public string? DownloadUrl { get; init; }
}
