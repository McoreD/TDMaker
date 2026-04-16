namespace TDMaker.Core.Models;

public sealed class ReleaseResult
{
    public required string OutputDirectory { get; init; }
    public required MediaInspectionResult Inspection { get; init; }
    public string PublishText { get; set; } = string.Empty;
    public string? PublishFilePath { get; set; }
    public string? XmlFilePath { get; set; }
    public List<string> TorrentFiles { get; } = [];
    public List<string> Warnings { get; } = [];
}
