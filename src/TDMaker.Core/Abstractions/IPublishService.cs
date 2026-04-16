namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IPublishService
{
    Task<string> RenderAsync(
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        CancellationToken cancellationToken = default);

    Task<string> WriteXmlAsync(
        MediaInspectionResult inspection,
        ReleaseProfile profile,
        string outputDirectory,
        CancellationToken cancellationToken = default);
}
