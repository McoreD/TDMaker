namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IMediaInspector
{
    Task<MediaInspectionResult> InspectAsync(
        ReleaseRequest request,
        CancellationToken cancellationToken = default);
}
