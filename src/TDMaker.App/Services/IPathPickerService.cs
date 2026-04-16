namespace TDMaker.App.Services;

using TDMaker.Core.Models;

public interface IPathPickerService
{
    Task<IReadOnlyList<string>> PickInputFilesAsync(string? initialPath = null);

    Task<string?> PickInputDirectoryAsync(string? initialPath = null);

    Task<string?> PickOutputDirectoryAsync(string? initialPath = null);

    Task<string?> PickToolPathAsync(ToolKind toolKind, string? initialPath = null);
}
