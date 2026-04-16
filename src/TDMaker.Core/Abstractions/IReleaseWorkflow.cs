namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IReleaseWorkflow
{
    Task<ReleaseResult> RunAsync(
        ReleaseRequest request,
        CancellationToken cancellationToken = default);
}
