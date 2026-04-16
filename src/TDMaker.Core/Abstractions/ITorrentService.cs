namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface ITorrentService
{
    Task<IReadOnlyList<string>> CreateAsync(
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        string outputDirectory,
        CancellationToken cancellationToken = default);
}
