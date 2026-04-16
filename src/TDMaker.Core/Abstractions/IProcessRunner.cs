namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IProcessRunner
{
    Task<ProcessResult> RunAsync(
        string executablePath,
        IReadOnlyList<string> arguments,
        string? workingDirectory = null,
        CancellationToken cancellationToken = default);
}
