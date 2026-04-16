namespace TDMaker.App.ViewModels;

using TDMaker.Core.Models;

public sealed class AssetSummaryViewModel
{
    public AssetSummaryViewModel(MediaAsset asset)
    {
        Asset = asset;
        FileName = asset.FileName;
        Format = asset.General.Format ?? asset.Video.Format ?? "Unknown";
        Duration = asset.General.DurationDisplay ?? "Unknown";
        Resolution = asset.Video.Resolution ?? "Audio only";
        AudioCount = asset.AudioTracks.Count;
        DetailedSummaryText = asset.SummaryTextComplete;
        ScreenshotCount = asset.Screenshots.Count;
    }

    public MediaAsset Asset { get; }
    public string FileName { get; }
    public string Format { get; }
    public string Duration { get; }
    public string Resolution { get; }
    public int AudioCount { get; }
    public int ScreenshotCount { get; }
    public string DetailedSummaryText { get; }
}
