namespace TDMaker.Core.Workflows;

using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed class ReleaseWorkflow(
    IMediaInspector mediaInspector,
    IScreenshotService screenshotService,
    IImageUploadService imageUploadService,
    IPublishService publishService,
    ITorrentService torrentService,
    ILogger<ReleaseWorkflow> logger) : IReleaseWorkflow
{
    public async Task<ReleaseResult> RunAsync(
        ReleaseRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await mediaInspector.InspectAsync(request, cancellationToken);
        var outputDirectory = ResolveOutputDirectory(request, inspection);

        Directory.CreateDirectory(outputDirectory);

        foreach (var asset in GetScreenshotTargets(inspection))
        {
            if (!request.Profile.CreateScreenshots)
            {
                continue;
            }

            var screenshots = await screenshotService.CreateAsync(
                asset,
                request.Profile,
                outputDirectory,
                request.Tools,
                cancellationToken);

            asset.Screenshots.Clear();
            asset.Screenshots.AddRange(screenshots);
        }

        if (request.Profile.UploadScreenshots)
        {
            foreach (var asset in inspection.Assets.Where(x => x.Screenshots.Count > 0))
            {
                var uploaded = await imageUploadService.UploadAsync(
                    asset.Screenshots,
                    request.Profile,
                    cancellationToken);

                asset.Screenshots.Clear();
                asset.Screenshots.AddRange(uploaded);
            }
        }

        var publishText = await publishService.RenderAsync(inspection, request.Profile, cancellationToken);
        string? publishFilePath = null;

        if (request.Profile.WritePublishText)
        {
            publishFilePath = Path.Combine(outputDirectory, $"{inspection.OutputName}_publish.txt");
            await File.WriteAllTextAsync(publishFilePath, publishText, cancellationToken);
        }

        var result = new ReleaseResult
        {
            Inspection = inspection,
            OutputDirectory = outputDirectory,
            PublishText = publishText,
            PublishFilePath = publishFilePath
        };

        if (request.Profile.CreateTorrent)
        {
            var torrentFiles = await torrentService.CreateAsync(
                inspection,
                request.Profile,
                outputDirectory,
                cancellationToken);

            result.TorrentFiles.AddRange(torrentFiles);
        }

        if (request.Profile.ExportXml)
        {
            result.XmlFilePath = await publishService.WriteXmlAsync(
                inspection,
                request.Profile,
                outputDirectory,
                cancellationToken);
        }

        result.Warnings.AddRange(inspection.Warnings);
        logger.LogInformation("Release workflow completed for {Title}", inspection.Title);

        return result;
    }

    private static IEnumerable<MediaAsset> GetScreenshotTargets(MediaInspectionResult inspection)
    {
        return inspection.InputKind switch
        {
            MediaInputKind.SingleFile => inspection.Assets.Take(1),
            MediaInputKind.Disc => [inspection.PrimaryAsset],
            _ => inspection.Assets
        };
    }

    private static string ResolveOutputDirectory(ReleaseRequest request, MediaInspectionResult inspection)
    {
        if (!string.IsNullOrWhiteSpace(request.OutputDirectoryOverride))
        {
            return request.OutputDirectoryOverride;
        }

        if (!string.IsNullOrWhiteSpace(request.Profile.OutputDirectory))
        {
            return Path.Combine(request.Profile.OutputDirectory, inspection.OutputName);
        }

        var sourceRoot = request.Inputs.Count == 1 && File.Exists(request.Inputs[0])
            ? Path.GetDirectoryName(request.Inputs[0]) ?? Environment.CurrentDirectory
            : request.Inputs.FirstOrDefault() ?? Environment.CurrentDirectory;

        return Path.Combine(sourceRoot, $"{inspection.OutputName}.tdmaker");
    }
}
