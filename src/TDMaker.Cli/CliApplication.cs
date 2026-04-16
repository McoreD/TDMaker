namespace TDMaker.Cli;

using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed class CliApplication(
    ISettingsStore settingsStore,
    IExternalToolLocator toolLocator,
    IMediaInspector mediaInspector,
    IReleaseWorkflow releaseWorkflow)
{
    public async Task<int> ShowToolsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await settingsStore.LoadAsync(cancellationToken);
        var tools = toolLocator.ResolveAll(settings.Tools);

        foreach (var tool in tools)
        {
            Console.WriteLine($"{tool.DisplayName,-10} {(tool.IsConfigured ? "ready" : "missing"),-8} {tool.Path ?? "(not found)"}");
        }

        return 0;
    }

    public async Task<int> InspectAsync(
        IReadOnlyList<string> inputs,
        string? profileId,
        string? title,
        string? source,
        CancellationToken cancellationToken = default)
    {
        var (settings, profile) = await LoadProfileAsync(profileId, cancellationToken);

        var inspection = await mediaInspector.InspectAsync(
            new ReleaseRequest
            {
                Inputs = inputs,
                Profile = profile,
                Tools = settings.Tools,
                TitleOverride = title,
                SourceOverride = source
            },
            cancellationToken);

        Console.WriteLine($"Title      : {inspection.Title}");
        Console.WriteLine($"Source     : {inspection.SourceLabel}");
        Console.WriteLine($"Input kind : {inspection.InputKind}");
        Console.WriteLine($"Assets     : {inspection.Assets.Count}");
        Console.WriteLine($"Primary    : {inspection.PrimaryAsset.FilePath}");
        Console.WriteLine();

        foreach (var asset in inspection.Assets)
        {
            Console.WriteLine(asset.FileName);
            Console.WriteLine($"  Format   : {asset.General.Format}");
            Console.WriteLine($"  Duration : {asset.General.DurationDisplay}");
            Console.WriteLine($"  Video    : {asset.Video.Format} {asset.Video.Resolution}");
            Console.WriteLine($"  Audio    : {asset.AudioTracks.Count}");
        }

        return 0;
    }

    public async Task<int> RunAsync(
        IReadOnlyList<string> inputs,
        string? profileId,
        string? title,
        string? source,
        string? output,
        bool? screenshots,
        bool? upload,
        bool? torrent,
        bool xml,
        CancellationToken cancellationToken = default)
    {
        var (settings, profile) = await LoadProfileAsync(profileId, cancellationToken);

        if (screenshots.HasValue)
        {
            profile.CreateScreenshots = screenshots.Value;
        }

        if (upload.HasValue)
        {
            profile.UploadScreenshots = upload.Value;
        }

        if (torrent.HasValue)
        {
            profile.CreateTorrent = torrent.Value;
        }

        if (xml)
        {
            profile.ExportXml = true;
        }

        var result = await releaseWorkflow.RunAsync(
            new ReleaseRequest
            {
                Inputs = inputs,
                Profile = profile,
                Tools = settings.Tools,
                TitleOverride = title,
                SourceOverride = source,
                OutputDirectoryOverride = output
            },
            cancellationToken);

        Console.WriteLine($"Output directory : {result.OutputDirectory}");
        Console.WriteLine($"Publish file     : {result.PublishFilePath ?? "(not written)"}");
        Console.WriteLine($"XML file         : {result.XmlFilePath ?? "(not written)"}");
        Console.WriteLine($"Torrent files    : {(result.TorrentFiles.Count == 0 ? "(none)" : string.Join(", ", result.TorrentFiles))}");
        Console.WriteLine($"Screenshots      : {result.Inspection.AllScreenshots.Count()}");

        foreach (var warning in result.Warnings.Distinct())
        {
            Console.WriteLine($"Warning          : {warning}");
        }

        return 0;
    }

    private async Task<(AppSettings settings, ReleaseProfile profile)> LoadProfileAsync(
        string? profileId,
        CancellationToken cancellationToken)
    {
        var settings = await settingsStore.LoadAsync(cancellationToken);
        var selected = string.IsNullOrWhiteSpace(profileId)
            ? settings.GetActiveProfile()
            : settings.Profiles.FirstOrDefault(x => x.Id.Equals(profileId, StringComparison.OrdinalIgnoreCase))
              ?? throw new InvalidOperationException($"Profile '{profileId}' was not found.");

        return (settings, selected.Clone());
    }
}
