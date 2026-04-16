namespace TDMaker.App.Services;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using TDMaker.Core.Models;

public sealed class AvaloniaPathPickerService : IPathPickerService
{
    public async Task<IReadOnlyList<string>> PickInputFilesAsync(string? initialPath = null)
    {
        var storage = GetStorageProvider();
        EnsureCanOpenFiles(storage);

        var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select media files",
            AllowMultiple = true,
            SuggestedStartLocation = await TryResolveStartFolderAsync(storage, initialPath),
            FileTypeFilter =
            [
                new FilePickerFileType("Common media files")
                {
                    Patterns = ["*.mkv", "*.mp4", "*.avi", "*.m2ts", "*.ts", "*.mov", "*.mpg", "*.mpeg", "*.wmv", "*.iso"],
                    AppleUniformTypeIdentifiers = ["public.movie", "public.video"],
                    MimeTypes = ["video/*"]
                },
                FilePickerFileTypes.All
            ]
        });

        return files.Select(TryGetRequiredLocalPath).ToArray();
    }

    public async Task<string?> PickInputDirectoryAsync(string? initialPath = null)
    {
        return await PickDirectoryAsync("Select an input folder", initialPath);
    }

    public async Task<string?> PickOutputDirectoryAsync(string? initialPath = null)
    {
        return await PickDirectoryAsync("Select an output folder", initialPath);
    }

    public async Task<string?> PickToolPathAsync(ToolKind toolKind, string? initialPath = null)
    {
        var storage = GetStorageProvider();
        EnsureCanOpenFiles(storage);

        var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = toolKind == ToolKind.FFmpeg ? "Select the FFmpeg executable" : "Select the MediaInfo executable",
            AllowMultiple = false,
            SuggestedStartLocation = await TryResolveStartFolderAsync(storage, initialPath),
            FileTypeFilter = [BuildToolPickerFileType(toolKind), FilePickerFileTypes.All]
        });

        return files.Count == 0 ? null : TryGetRequiredLocalPath(files[0]);
    }

    private static async Task<string?> PickDirectoryAsync(string title, string? initialPath)
    {
        var storage = GetStorageProvider();
        EnsureCanPickFolders(storage);

        var folders = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            SuggestedStartLocation = await TryResolveStartFolderAsync(storage, initialPath)
        });

        return folders.Count == 0 ? null : TryGetRequiredLocalPath(folders[0]);
    }

    private static IStorageProvider GetStorageProvider()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow is null)
        {
            throw new InvalidOperationException("The main desktop window is not ready yet.");
        }

        return desktop.MainWindow.StorageProvider;
    }

    private static void EnsureCanOpenFiles(IStorageProvider storageProvider)
    {
        if (!storageProvider.CanOpen)
        {
            throw new InvalidOperationException("This platform does not currently expose a file picker.");
        }
    }

    private static void EnsureCanPickFolders(IStorageProvider storageProvider)
    {
        if (!storageProvider.CanPickFolder)
        {
            throw new InvalidOperationException("This platform does not currently expose a folder picker.");
        }
    }

    private static FilePickerFileType BuildToolPickerFileType(ToolKind toolKind)
    {
        if (toolKind == ToolKind.FFmpeg)
        {
            return OperatingSystem.IsWindows()
                ? new FilePickerFileType("FFmpeg executable") { Patterns = ["ffmpeg*.exe"] }
                : new FilePickerFileType("FFmpeg executable") { Patterns = ["ffmpeg*"] };
        }

        return OperatingSystem.IsWindows()
            ? new FilePickerFileType("MediaInfo executable") { Patterns = ["mediainfo*.exe"] }
            : new FilePickerFileType("MediaInfo executable") { Patterns = ["mediainfo*"] };
    }

    private static async Task<IStorageFolder?> TryResolveStartFolderAsync(IStorageProvider storageProvider, string? initialPath)
    {
        var folderPath = NormalizeStartFolder(initialPath);
        if (folderPath is null)
        {
            return null;
        }

        return await storageProvider.TryGetFolderFromPathAsync(folderPath);
    }

    private static string? NormalizeStartFolder(string? initialPath)
    {
        if (string.IsNullOrWhiteSpace(initialPath))
        {
            return null;
        }

        try
        {
            var candidate = initialPath.Trim();
            if (Directory.Exists(candidate))
            {
                return Path.GetFullPath(candidate);
            }

            var fullPath = Path.GetFullPath(candidate);
            if (File.Exists(fullPath))
            {
                return Path.GetDirectoryName(fullPath);
            }

            var parent = Path.GetDirectoryName(fullPath);
            return parent is not null && Directory.Exists(parent) ? parent : null;
        }
        catch
        {
            return null;
        }
    }

    private static string TryGetRequiredLocalPath(IStorageItem item)
    {
        return item.TryGetLocalPath()
            ?? throw new InvalidOperationException("The selected item did not resolve to a local file system path.");
    }
}
