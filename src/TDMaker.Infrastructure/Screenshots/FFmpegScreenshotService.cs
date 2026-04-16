namespace TDMaker.Infrastructure.Screenshots;

using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;
using TDMaker.Infrastructure.Support;

public sealed class FFmpegScreenshotService(
    IExternalToolLocator toolLocator,
    IProcessRunner processRunner,
    ILogger<FFmpegScreenshotService> logger) : IScreenshotService
{
    public async Task<IReadOnlyList<ScreenshotArtifact>> CreateAsync(
        MediaAsset asset,
        ReleaseProfile profile,
        string outputDirectory,
        ToolSettings tools,
        CancellationToken cancellationToken = default)
    {
        if (!asset.HasVideo || asset.General.DurationMilliseconds is null or <= 0)
        {
            return [];
        }

        var ffmpeg = toolLocator.Resolve(ToolKind.FFmpeg, tools);
        if (!ffmpeg.IsConfigured)
        {
            throw new InvalidOperationException("FFmpeg could not be found. Configure ffmpeg before requesting screenshots.");
        }

        var screenshotDirectory = Path.Combine(outputDirectory, "screenshots");
        Directory.CreateDirectory(screenshotDirectory);

        var timestamps = CreateTimestamps(asset.General.DurationMilliseconds.Value, profile);
        var screenshots = new List<ScreenshotArtifact>(timestamps.Count);

        for (var index = 0; index < timestamps.Count; index++)
        {
            var timestamp = timestamps[index];
            var outputPath = Path.Combine(
                screenshotDirectory,
                $"{Path.GetFileNameWithoutExtension(asset.FileName)}-{index + 1:000}.png");

            var result = await processRunner.RunAsync(
                ffmpeg.Path!,
                [
                    "-hide_banner",
                    "-loglevel",
                    "error",
                    "-ss",
                    timestamp.ToString(@"hh\:mm\:ss\.fff"),
                    "-i",
                    asset.FilePath,
                    "-frames:v",
                    "1",
                    "-y",
                    outputPath
                ],
                cancellationToken: cancellationToken);

            if (result.ExitCode == 0 && File.Exists(outputPath))
            {
                screenshots.Add(new ScreenshotArtifact
                {
                    LocalPath = outputPath,
                    Timestamp = timestamp
                });
            }
            else
            {
                logger.LogWarning(
                    "FFmpeg could not create screenshot {Index} for {File}: {Error}",
                    index + 1,
                    asset.FileName,
                    result.StandardError);
            }
        }

        if (profile.CombineScreenshots && screenshots.Count > 1)
        {
            var contactSheet = await CreateContactSheetAsync(asset, screenshots, profile, screenshotDirectory, cancellationToken);
            return [contactSheet];
        }

        return screenshots;
    }

    private static List<TimeSpan> CreateTimestamps(double durationMilliseconds, ReleaseProfile profile)
    {
        var duration = TimeSpan.FromMilliseconds(durationMilliseconds);
        var sliceSize = duration.TotalSeconds / (profile.ScreenshotCount + 1);
        var results = new List<TimeSpan>(profile.ScreenshotCount);
        var random = Random.Shared;

        for (var index = 0; index < profile.ScreenshotCount; index++)
        {
            var segmentStart = sliceSize * (index + 1);
            var seconds = profile.RandomizeScreenshotFrames
                ? segmentStart + random.NextDouble() * Math.Max(sliceSize - 1, 1)
                : segmentStart;

            results.Add(TimeSpan.FromSeconds(seconds));
        }

        return results;
    }

    private static async Task<ScreenshotArtifact> CreateContactSheetAsync(
        MediaAsset asset,
        IReadOnlyList<ScreenshotArtifact> screenshots,
        ReleaseProfile profile,
        string screenshotDirectory,
        CancellationToken cancellationToken)
    {
        using var firstImage = await Image.LoadAsync<Rgba32>(screenshots[0].LocalPath, cancellationToken);

        const int padding = 16;
        const int spacing = 10;
        var columns = Math.Max(profile.ScreenshotColumns, 1);
        var rows = (int)Math.Ceiling(screenshots.Count / (double)columns);
        var width = padding * 2 + firstImage.Width * columns + spacing * (columns - 1);
        var height = padding * 2 + firstImage.Height * rows + spacing * (rows - 1);

        using var canvas = new Image<Rgba32>(width, height, Color.ParseHex("#101826"));
        for (var index = 0; index < screenshots.Count; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var image = await Image.LoadAsync<Rgba32>(screenshots[index].LocalPath, cancellationToken);
            var column = index % columns;
            var row = index / columns;
            var point = new Point(
                padding + column * (firstImage.Width + spacing),
                padding + row * (firstImage.Height + spacing));

            canvas.Mutate(ctx => ctx.DrawImage(image, point, 1f));
        }

        var outputPath = Path.Combine(
            screenshotDirectory,
            $"{Path.GetFileNameWithoutExtension(asset.FileName)}-sheet.jpg");

        await canvas.SaveAsJpegAsync(outputPath, cancellationToken);

        return new ScreenshotArtifact
        {
            LocalPath = outputPath,
            Timestamp = screenshots[0].Timestamp
        };
    }
}
