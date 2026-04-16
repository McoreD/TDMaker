namespace TDMaker.App.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ISettingsStore _settingsStore;
    private readonly IExternalToolLocator _toolLocator;
    private readonly IMediaInspector _mediaInspector;
    private readonly IPublishService _publishService;
    private readonly IReleaseWorkflow _releaseWorkflow;
    private AppSettings? _settings;

    public MainWindowViewModel(
        ISettingsStore settingsStore,
        IExternalToolLocator toolLocator,
        IMediaInspector mediaInspector,
        IPublishService publishService,
        IReleaseWorkflow releaseWorkflow)
    {
        _settingsStore = settingsStore;
        _toolLocator = toolLocator;
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

    partial void OnSelectedAssetChanged(AssetSummaryViewModel? value)
    {
        SelectedAssetSummary = value?.DetailedSummaryText ?? string.Empty;
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

    [RelayCommand(CanExecute = nameof(CanAddInput))]
    private void AddInput()
    {
        var candidate = DraftInputPath.Trim();
        if (string.IsNullOrWhiteSpace(candidate))
        {
            return;
        }

        if (!InputPaths.Contains(candidate, StringComparer.OrdinalIgnoreCase))
        {
            InputPaths.Add(candidate);
        }

        DraftInputPath = string.Empty;
        StatusMessage = $"Queued {InputPaths.Count} input path(s).";
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
    private bool CanRemoveSelectedInput() => SelectedInput is not null;
    private bool CanInspect() => !IsBusy && InputPaths.Count > 0 && SelectedProfile is not null;

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
        foreach (var tool in _toolLocator.ResolveAll(tools))
        {
            ToolStatuses.Add(new ToolStatusViewModel
            {
                DisplayName = tool.DisplayName,
                Status = tool.IsConfigured ? "Ready" : "Missing",
                Path = tool.Path ?? "Install on PATH or configure an absolute path."
            });
        }
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
        RemoveSelectedInputCommand.NotifyCanExecuteChanged();
        InspectCommand.NotifyCanExecuteChanged();
        RunWorkflowCommand.NotifyCanExecuteChanged();
    }

    private static string? NullIfEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
