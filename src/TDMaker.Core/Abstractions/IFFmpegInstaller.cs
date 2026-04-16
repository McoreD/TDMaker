namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IFFmpegInstaller
{
    Task<ToolInstallationResult> InstallLatestAsync(
        IProgress<ToolInstallationProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
