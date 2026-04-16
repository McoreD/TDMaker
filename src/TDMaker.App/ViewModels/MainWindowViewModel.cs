namespace TDMaker.App.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDMaker.App.Services;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public partial class MainWindowViewModel : ViewModelBase
{
    private static readonly StringComparer PathComparer =
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    private static readonly StringComparison PathComparison =
        OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    private readonly ISettingsStore _settingsStore;
    private readonly IExternalToolLocator _toolLocator;
    private readonly IPathPickerService _pathPickerService;
    private readonly IFFmpegInstaller _ffmpegInstaller;
    private readonly IMediaInfoInstaller _mediaInfoInstaller;
    private readonly IPlatformPaths _platformPaths;
    private readonly IMediaInspector _mediaInspector;
    private readonly IPublishService _publishService;
    private readonly IReleaseWorkflow _releaseWorkflow;
    private AppSettings? _settings;

    public MainWindowViewModel(
        ISettingsStore settingsStore,
        IExternalToolLocator toolLocator,
        IPathPickerService pathPickerService,
        IFFmpegInstaller ffmpegInstaller,
        IMediaInfoInstaller mediaInfoInstaller,
        IPlatformPaths platformPaths,
        IMediaInspector mediaInspector,
        IPublishService publishService,
        IReleaseWorkflow releaseWorkflow)
    {
        _settingsStore = settingsStore;
        _toolLocator = toolLocator;
        _pathPickerService = pathPickerService;
        _ffmpegInstaller = ffmpegInstaller;
        _mediaInfoInstaller = mediaInfoInstaller;
        _platformPaths = platformPaths;
        _mediaInspector = mediaInspector;
        _publishService = publishService;
        _releaseWorkflow = releaseWorkflow;

        InputPaths.CollectionChanged += (_, _) => RefreshCommands();
        _ = LoadSettingsAsync();
    }

    public ObservableCollection<string> InputPaths { get; } = [];
    public ObservableCollection<AssetSummaryViewModel> Assets { get; } = [];
    public ObservableCollection<string> Warnings { get; } = [];
    public ObservableCollection<ToolStatusViewModel> ToolStatuses { get; } = [];
    public ObservableCollection<ProfileChoiceViewModel> Profiles { get; } = [];
    public ObservableCollection<string> Presets { get; } = ["Default", "MTN", "Minimal", "BTN"];

    [ObservableProperty] private string draftInputPath = string.Empty;
    [ObservableProperty] private string? selectedInput;
    [ObservableProperty] private AssetSummaryViewModel? selectedAsset;
    [ObservableProperty] private ProfileChoiceViewModel? selectedProfile;
    [ObservableProperty] private string titleOverride = string.Empty;
    [ObservableProperty] private string sourceOverride = string.Empty;
    [ObservableProperty] private string publishText = string.Empty;
    [ObservableProperty] private string selectedAssetSummary = string.Empty;
    [ObservableProperty] private string outputDirectory = string.Empty;
    [ObservableProperty] private string statusMessage = "Loading settings...";
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string ffmpegPath = string.Empty;
    [ObservableProperty] private string mediaInfoPath = string.Empty;
    [ObservableProperty] private string profileOutputDirectory = string.Empty;
    [ObservableProperty] private string publishPreset = "Default";
    [ObservableProperty] private string ptpImgApiKey = string.Empty;
    [ObservableProperty] private bool createScreenshots = true;
    [ObservableProperty] private bool uploadScreenshots;
    [ObservableProperty] private bool combineScreenshots;
    [ObservableProperty] private bool createTorrent = true;
    [ObservableProperty] private bool exportXml;
    [ObservableProperty] private bool wrapPublishInPreBlock;
    [ObservableProperty] private bool centerPublishText;
    [ObservableProperty] private bool useFullSizeImages = true;
    [ObservableProperty] private bool hidePrivatePaths = true;
    [ObservableProperty] private bool randomizeScreenshotFrames = true;
    [ObservableProperty] private int screenshotCount = 6;
    [ObservableProperty] private int screenshotColumns = 3;
    [ObservableProperty] private bool isDownloadingFfmpeg;
    [ObservableProperty] private double ffmpegDownloadProgress;
    [ObservableProperty] private string ffmpegDownloadStatus = "FFmpeg is resolved from settings, TDMAKER_HOME/tools, the app directory, or PATH.";
    [ObservableProperty] private bool isFfmpegAvailable;
    [ObservableProperty] private bool isDownloadingMediaInfo;
    [ObservableProperty] private double mediaInfoDownloadProgress;
    [ObservableProperty] private string mediaInfoDownloadStatus = "MediaInfo is resolved from settings, TDMAKER_HOME/tools, the app directory, or PATH.";
    [ObservableProperty] private bool isMediaInfoAvailable;

    partial void OnSelectedAssetChanged(AssetSummaryViewModel? value)
    {
        SelectedAssetSummary = value?.DetailedSummaryText ?? string.Empty;
    }

    partial void OnDraftInputPathChanged(string value)
    {
        AddInputCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedInputChanged(string? value)
    {
        RemoveSelectedInputCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedProfileChanged(ProfileChoiceViewModel? value)
    {
        if (_settings is null || value is null)
        {
            return;
        }

        LoadProfileEditor(_settings.Profiles.First(x => x.Id == value.Id));
        RefreshCommands();
    }

    partial void OnFfmpegPathChanged(string value)
    {
        RefreshToolStatusesPreview();
    }

    partial void OnMediaInfoPathChanged(string value)
    {
        RefreshToolStatusesPreview();
    }

    [RelayCommand(CanExecute = nameof(CanAddInput))]
    private void AddInput()
    {
        var added = QueueInputs([DraftInputPath]);
        if (added == 0)
        {
            return;
        }

        DraftInputPath = string.Empty;
        StatusMessage = $"Queued {InputPaths.Count} input path(s).";
    }

    [RelayCommand(CanExecute = nameof(CanBrowsePaths))]
    private async Task BrowseInputFilesAsync()
    {
        try
        {
            var paths = await _pathPickerService.PickInputFilesAsync(NullIfEmpty(DraftInputPath) ?? SelectedInput);
            var added = QueueInputs(paths);
            if (added > 0)
            {
                DraftInputPath = string.Empty;
                StatusMessage = $"Queued {added} new input path(s).";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(CanBrowsePaths))]
    private async Task BrowseInputDirectoryAsync()
    {
        try
        {
            var path = await _pathPickerService.PickInputDirectoryAsync(NullIfEmpty(DraftInputPath) ?? SelectedInput);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            var added = QueueInputs([path]);
            if (added > 0)
            {
                DraftInputPath = string.Empty;
                StatusMessage = $"Queued {path}.";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(CanRemoveSelectedInput))]
    private void RemoveSelectedInput()
    {
        if (SelectedInput is null)
        {
            return;
        }

        InputPaths.Remove(SelectedInput);
        SelectedInput = InputPaths.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ReloadSettingsAsync() => await LoadSettingsAsync();

    [RelayCommand(CanExecute = nameof(CanDownloadFfmpeg))]
    private async Task DownloadFfmpegAsync()
    {
        if (_settings is null)
        {
            StatusMessage = "Settings are still loading.";
            return;
        }

        try
        {
            IsDownloadingFfmpeg = true;
            FfmpegDownloadProgress = 0;
            FfmpegDownloadStatus = "Preparing managed FFmpeg download...";
            StatusMessage = "Downloading managed FFmpeg...";
            RefreshCommands();

            var progress = new Progress<ToolInstallationProgress>(update =>
            {
                if (update.Kind != ToolKind.FFmpeg)
                {
                    return;
                }

                if (update.Percentage.HasValue)
                {
                    FfmpegDownloadProgress = Math.Clamp(update.Percentage.Value, 0, 100);
                }

                FfmpegDownloadStatus = update.Status;
            });

            var result = await _ffmpegInstaller.InstallLatestAsync(progress);
            RefreshToolStatuses(_settings.Tools);

            FfmpegDownloadStatus = result.Message;
            if (result.Success)
            {
                FfmpegDownloadProgress = 100;
                StatusMessage = "Managed FFmpeg installed.";
            }
            else
            {
                StatusMessage = result.Message;
            }
        }
        catch (Exception ex)
        {
            FfmpegDownloadStatus = ex.Message;
            StatusMessage = ex.Message;
        }
        finally
        {
            IsDownloadingFfmpeg = false;
            RefreshCommands();
        }
    }

    [RelayCommand(CanExecute = nameof(CanDownloadMediaInfo))]
    private async Task DownloadMediaInfoAsync()
    {
        if (_settings is null)
        {
            StatusMessage = "Settings are still loading.";
            return;
        }

        try
        {
            IsDownloadingMediaInfo = true;
            MediaInfoDownloadProgress = 0;
            MediaInfoDownloadStatus = "Preparing managed MediaInfo download...";
            StatusMessage = "Downloading managed MediaInfo...";
            RefreshCommands();

            var progress = new Progress<ToolInstallationProgress>(update =>
            {
                if (update.Kind != ToolKind.MediaInfo)
                {
                    return;
                }

                if (update.Percentage.HasValue)
                {
                    MediaInfoDownloadProgress = Math.Clamp(update.Percentage.Value, 0, 100);
                }

                MediaInfoDownloadStatus = update.Status;
            });

            var result = await _mediaInfoInstaller.InstallLatestAsync(progress);
            RefreshToolStatuses(_settings.Tools);

            MediaInfoDownloadStatus = result.Message;
            if (result.Success)
            {
                MediaInfoDownloadProgress = 100;
                StatusMessage = "Managed MediaInfo installed.";
            }
            else
            {
                StatusMessage = result.Message;
            }
        }
        catch (Exception ex)
        {
            MediaInfoDownloadStatus = ex.Message;
            StatusMessage = ex.Message;
        }
        finally
        {
            IsDownloadingMediaInfo = false;
            RefreshCommands();
        }
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        if (_settings is null || SelectedProfile is null)
        {
            return;
        }

        _settings.ActiveProfileId = SelectedProfile.Id;
        _settings.Tools.FFmpegPath = NullIfEmpty(FfmpegPath);
        _settings.Tools.MediaInfoPath = NullIfEmpty(MediaInfoPath);

        ApplyEditor(_settings.Profiles.First(x => x.Id == SelectedProfile.Id));
        await _settingsStore.SaveAsync(_settings);
        RefreshToolStatuses(_settings.Tools);
        StatusMessage = "Settings saved.";
    }

    [RelayCommand(CanExecute = nameof(CanBrowsePaths))]
    private async Task BrowseProfileOutputDirectoryAsync()
    {
        try
        {
            var path = await _pathPickerService.PickOutputDirectoryAsync(ProfileOutputDirectory);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            ProfileOutputDirectory = path;
            StatusMessage = $"Output directory set to {path}.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(CanBrowsePaths))]
    private async Task BrowseFfmpegPathAsync()
    {
        try
        {
            var path = await _pathPickerService.PickToolPathAsync(ToolKind.FFmpeg, FfmpegPath);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            FfmpegPath = path;
            StatusMessage = "FFmpeg path updated.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(CanBrowsePaths))]
    private async Task BrowseMediaInfoPathAsync()
    {
        try
        {
            var path = await _pathPickerService.PickToolPathAsync(ToolKind.MediaInfo, MediaInfoPath);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            MediaInfoPath = path;
            StatusMessage = "MediaInfo path updated.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(CanInspect))]
    private async Task InspectAsync()
    {
        var request = BuildRequest();
        if (request is null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            RefreshCommands();
            StatusMessage = "Inspecting media with MediaInfo...";
            var inspection = await _mediaInspector.InspectAsync(request);
            ApplyInspection(inspection);
            PublishText = await _publishService.RenderAsync(inspection, request.Profile);
            StatusMessage = "Inspection completed.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            RefreshCommands();
        }
    }

    [RelayCommand(CanExecute = nameof(CanInspect))]
    private async Task RunWorkflowAsync()
    {
        var request = BuildRequest();
        if (request is null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            RefreshCommands();
            StatusMessage = "Running shared workflow...";
            var result = await _releaseWorkflow.RunAsync(request);
            ApplyInspection(result.Inspection);
            PublishText = result.PublishText;
            OutputDirectory = result.OutputDirectory;
            Warnings.Clear();
            foreach (var warning in result.Warnings)
            {
                Warnings.Add(warning);
            }

            StatusMessage = "Workflow completed.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            RefreshCommands();
        }
    }

    [RelayCommand]
    private void ClearSession()
    {
        InputPaths.Clear();
        Assets.Clear();
        Warnings.Clear();
        PublishText = string.Empty;
        SelectedAssetSummary = string.Empty;
        OutputDirectory = string.Empty;
        StatusMessage = "Session cleared.";
    }

    private bool CanAddInput() => !string.IsNullOrWhiteSpace(DraftInputPath);
    private bool CanBrowsePaths() => !IsBusy && !IsDownloadingFfmpeg && !IsDownloadingMediaInfo;
    private bool CanRemoveSelectedInput() => SelectedInput is not null;
    private bool CanInspect() => !IsBusy && !IsDownloadingFfmpeg && !IsDownloadingMediaInfo && InputPaths.Count > 0 && SelectedProfile is not null;
    private bool CanDownloadFfmpeg() => !IsBusy && !IsDownloadingFfmpeg && !IsDownloadingMediaInfo && !IsFfmpegAvailable;
    private bool CanDownloadMediaInfo() => !IsBusy && !IsDownloadingMediaInfo && !IsDownloadingFfmpeg && !IsMediaInfoAvailable;

    private async Task LoadSettingsAsync()
    {
        try
        {
            _settings = await _settingsStore.LoadAsync();
            Profiles.Clear();
            foreach (var profile in _settings.Profiles)
            {
                Profiles.Add(new ProfileChoiceViewModel { Id = profile.Id, Name = profile.Name });
            }

            FfmpegPath = _settings.Tools.FFmpegPath ?? string.Empty;
            MediaInfoPath = _settings.Tools.MediaInfoPath ?? string.Empty;
            RefreshToolStatuses(_settings.Tools);

            SelectedProfile = Profiles.FirstOrDefault(x => x.Id == _settings.ActiveProfileId) ?? Profiles.FirstOrDefault();
            if (SelectedProfile is not null)
            {
                LoadProfileEditor(_settings.Profiles.First(x => x.Id == SelectedProfile.Id));
            }

            StatusMessage = "Ready.";
            RefreshCommands();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void LoadProfileEditor(ReleaseProfile profile)
    {
        ProfileOutputDirectory = profile.OutputDirectory ?? string.Empty;
        PublishPreset = profile.PublishPreset;
        PtpImgApiKey = profile.PtpImgApiKey ?? string.Empty;
        CreateScreenshots = profile.CreateScreenshots;
        UploadScreenshots = profile.UploadScreenshots;
        CombineScreenshots = profile.CombineScreenshots;
        CreateTorrent = profile.CreateTorrent;
        ExportXml = profile.ExportXml;
        WrapPublishInPreBlock = profile.WrapPublishInPreBlock;
        CenterPublishText = profile.CenterPublishText;
        UseFullSizeImages = profile.UseFullSizeImages;
        HidePrivatePaths = profile.HidePrivatePaths;
        RandomizeScreenshotFrames = profile.RandomizeScreenshotFrames;
        ScreenshotCount = profile.ScreenshotCount;
        ScreenshotColumns = profile.ScreenshotColumns;
    }

    private void ApplyEditor(ReleaseProfile profile)
    {
        profile.OutputDirectory = NullIfEmpty(ProfileOutputDirectory);
        profile.PublishPreset = PublishPreset;
        profile.PtpImgApiKey = NullIfEmpty(PtpImgApiKey);
        profile.CreateScreenshots = CreateScreenshots;
        profile.UploadScreenshots = UploadScreenshots;
        profile.CombineScreenshots = CombineScreenshots;
        profile.CreateTorrent = CreateTorrent;
        profile.ExportXml = ExportXml;
        profile.WrapPublishInPreBlock = WrapPublishInPreBlock;
        profile.CenterPublishText = CenterPublishText;
        profile.UseFullSizeImages = UseFullSizeImages;
        profile.HidePrivatePaths = HidePrivatePaths;
        profile.RandomizeScreenshotFrames = RandomizeScreenshotFrames;
        profile.ScreenshotCount = ScreenshotCount;
        profile.ScreenshotColumns = ScreenshotColumns;
    }

    private void RefreshToolStatuses(ToolSettings tools)
    {
        ToolStatuses.Clear();
        var resolvedTools = _toolLocator.ResolveAll(tools);

        foreach (var tool in resolvedTools)
        {
            ToolStatuses.Add(new ToolStatusViewModel
            {
                DisplayName = tool.DisplayName,
                Status = tool.IsConfigured ? "Ready" : "Missing",
                Path = tool.Path ?? GetMissingToolMessage(tool.Kind)
            });
        }

        var ffmpeg = resolvedTools.First(x => x.Kind == ToolKind.FFmpeg);
        IsFfmpegAvailable = ffmpeg.IsConfigured;
        UpdateFfmpegDownloadState(ffmpeg);

        var mediaInfo = resolvedTools.First(x => x.Kind == ToolKind.MediaInfo);
        IsMediaInfoAvailable = mediaInfo.IsConfigured;
        UpdateMediaInfoDownloadState(mediaInfo);
    }

    private ReleaseRequest? BuildRequest()
    {
        if (_settings is null || SelectedProfile is null)
        {
            StatusMessage = "Settings are still loading.";
            return null;
        }

        var profile = _settings.Profiles.First(x => x.Id == SelectedProfile.Id).Clone();
        ApplyEditor(profile);

        return new ReleaseRequest
        {
            Inputs = InputPaths.ToArray(),
            Profile = profile,
            Tools = new ToolSettings
            {
                FFmpegPath = NullIfEmpty(FfmpegPath),
                MediaInfoPath = NullIfEmpty(MediaInfoPath)
            },
            TitleOverride = NullIfEmpty(TitleOverride),
            SourceOverride = NullIfEmpty(SourceOverride)
        };
    }

    private void ApplyInspection(MediaInspectionResult inspection)
    {
        Assets.Clear();
        foreach (var asset in inspection.Assets)
        {
            Assets.Add(new AssetSummaryViewModel(asset));
        }

        SelectedAsset = Assets.FirstOrDefault();
        OutputDirectory = inspection.OutputName;
        Warnings.Clear();
        foreach (var warning in inspection.Warnings)
        {
            Warnings.Add(warning);
        }
    }

    private void RefreshCommands()
    {
        AddInputCommand.NotifyCanExecuteChanged();
        BrowseInputFilesCommand.NotifyCanExecuteChanged();
        BrowseInputDirectoryCommand.NotifyCanExecuteChanged();
        BrowseProfileOutputDirectoryCommand.NotifyCanExecuteChanged();
        BrowseFfmpegPathCommand.NotifyCanExecuteChanged();
        BrowseMediaInfoPathCommand.NotifyCanExecuteChanged();
        RemoveSelectedInputCommand.NotifyCanExecuteChanged();
        DownloadFfmpegCommand.NotifyCanExecuteChanged();
        DownloadMediaInfoCommand.NotifyCanExecuteChanged();
        InspectCommand.NotifyCanExecuteChanged();
        RunWorkflowCommand.NotifyCanExecuteChanged();
    }

    private static string? NullIfEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private void UpdateFfmpegDownloadState(ToolResolution ffmpeg)
    {
        if (IsDownloadingFfmpeg)
        {
            return;
        }

        if (ffmpeg.IsConfigured)
        {
            FfmpegDownloadProgress = 100;
            FfmpegDownloadStatus = IsManagedToolPath(ffmpeg.Path)
                ? $"Managed FFmpeg ready: {ffmpeg.Path}"
                : $"FFmpeg already available: {ffmpeg.Path}";
            return;
        }

        FfmpegDownloadProgress = 0;
        FfmpegDownloadStatus = "FFmpeg not found. Download a managed build into TDMAKER_HOME/tools or configure a custom path.";
    }

    private void UpdateMediaInfoDownloadState(ToolResolution mediaInfo)
    {
        if (IsDownloadingMediaInfo)
        {
            return;
        }

        if (mediaInfo.IsConfigured)
        {
            MediaInfoDownloadProgress = 100;
            MediaInfoDownloadStatus = IsManagedToolPath(mediaInfo.Path)
                ? $"Managed MediaInfo ready: {mediaInfo.Path}"
                : $"MediaInfo already available: {mediaInfo.Path}";
            return;
        }

        MediaInfoDownloadProgress = 0;
        MediaInfoDownloadStatus = "MediaInfo not found. Download a managed build into TDMAKER_HOME/tools or configure a custom path.";
    }

    private static string GetMissingToolMessage(ToolKind toolKind)
    {
        return toolKind switch
        {
            ToolKind.FFmpeg => "Install on PATH, configure an absolute path, or download a managed FFmpeg build.",
            ToolKind.MediaInfo => "Install on PATH, configure an absolute path, or download a managed MediaInfo build.",
            _ => "Install on PATH or configure an absolute path."
        };
    }

    private int QueueInputs(IEnumerable<string> paths)
    {
        var added = 0;

        foreach (var path in paths
                     .Select(NullIfEmpty)
                     .Where(path => path is not null)
                     .Select(path => path!))
        {
            if (InputPaths.Contains(path, PathComparer))
            {
                continue;
            }

            InputPaths.Add(path);
            added++;
        }

        if (SelectedInput is null)
        {
            SelectedInput = InputPaths.FirstOrDefault();
        }

        return added;
    }

    private void RefreshToolStatusesPreview()
    {
        if (_settings is null)
        {
            return;
        }

        RefreshToolStatuses(new ToolSettings
        {
            FFmpegPath = NullIfEmpty(FfmpegPath),
            MediaInfoPath = NullIfEmpty(MediaInfoPath)
        });
    }

    private bool IsManagedToolPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var managedToolsRoot = Path.GetFullPath(_platformPaths.ToolsDirectory);

        if (!managedToolsRoot.EndsWith(Path.DirectorySeparatorChar))
        {
            managedToolsRoot += Path.DirectorySeparatorChar;
        }

        var fullPath = Path.GetFullPath(path);
        return fullPath.StartsWith(managedToolsRoot, PathComparison);
    }
}
