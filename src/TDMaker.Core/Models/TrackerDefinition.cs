namespace TDMaker.Core.Models;

public sealed class TrackerDefinition
{
    public string Name { get; set; } = string.Empty;
    public string AnnounceUrl { get; set; } = string.Empty;
    public string? SourceFlag { get; set; }
    public bool Enabled { get; set; } = true;
}
