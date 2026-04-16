namespace TDMaker.Core.Abstractions;

using TDMaker.Core.Models;

public interface IExternalToolLocator
{
    ToolResolution Resolve(ToolKind toolKind, ToolSettings settings);
    IReadOnlyList<ToolResolution> ResolveAll(ToolSettings settings);
}
