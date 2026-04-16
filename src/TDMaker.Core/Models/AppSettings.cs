namespace TDMaker.Core.Models;

public sealed class AppSettings
{
    public const int CurrentSchemaVersion = 1;

    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
    public string ActiveProfileId { get; set; } = ReleaseProfile.DefaultMovieProfileId;
    public string? CustomWorkspaceDirectory { get; set; }
    public ToolSettings Tools { get; set; } = new();
    public List<ReleaseProfile> Profiles { get; set; } =
    [
        ReleaseProfile.CreateMovieProfile(),
        ReleaseProfile.CreateMusicProfile()
    ];

    public ReleaseProfile GetActiveProfile()
    {
        return Profiles.FirstOrDefault(x => x.Id == ActiveProfileId)
            ?? Profiles.First();
    }
}
