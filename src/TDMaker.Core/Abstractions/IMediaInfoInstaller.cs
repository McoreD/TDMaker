namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IMediaInfoInstaller
{
    Task<ToolInstallationResult> InstallLatestAsync(
        IProgress<ToolInstallationProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
