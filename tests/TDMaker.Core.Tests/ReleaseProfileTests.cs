using TDMaker.Core.Models;

namespace TDMaker.Core.Tests;

public sealed class ReleaseProfileTests
{
    [Fact]
    public void CloneCreatesIndependentCollections()
    {
        var original = new ReleaseProfile
        {
            PtpImgApiKey = "alpha",
            Trackers =
            [
                new TrackerDefinition
                {
                    Name = "Main",
                    AnnounceUrl = "https://tracker.example/announce",
                    SourceFlag = "TDM",
                    Enabled = true
                }
            ],
            VideoExtensions = [".mkv"],
            AudioExtensions = [".flac"]
        };

        var clone = original.Clone();
        clone.PtpImgApiKey = "beta";
        clone.Trackers[0].AnnounceUrl = "https://other.example/announce";
        clone.VideoExtensions.Add(".mp4");
        clone.AudioExtensions.Add(".aac");

        Assert.Equal("alpha", original.PtpImgApiKey);
        Assert.Equal("https://tracker.example/announce", original.Trackers[0].AnnounceUrl);
        Assert.Single(original.VideoExtensions);
        Assert.Single(original.AudioExtensions);
    }

    [Fact]
    public void CreateMusicProfileUsesMusicFriendlyDefaults()
    {
        var profile = ReleaseProfile.CreateMusicProfile();

        Assert.Equal(ReleaseProfile.DefaultMusicProfileId, profile.Id);
        Assert.Equal("WEB", profile.SourceLabel);
        Assert.Equal("MTN", profile.PublishPreset);
        Assert.True(profile.CombineScreenshots);
        Assert.Equal(16, profile.ScreenshotCount);
        Assert.Equal(4, profile.ScreenshotColumns);
    }
}
