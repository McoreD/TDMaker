namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IScreenshotService
{
    Task<IReadOnlyList<ScreenshotArtifact>> CreateAsync(
        MediaAsset asset,
        ReleaseProfile profile,
        string outputDirectory,
        ToolSettings tools,
        CancellationToken cancellationToken = default);
}
