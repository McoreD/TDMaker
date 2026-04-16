namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IImageUploadService
{
    Task<IReadOnlyList<ScreenshotArtifact>> UploadAsync(
        IReadOnlyList<ScreenshotArtifact> screenshots,
        ReleaseProfile profile,
        CancellationToken cancellationToken = default);
}
