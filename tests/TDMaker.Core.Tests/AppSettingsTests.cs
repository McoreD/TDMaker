using TDMaker.Core.Models;

namespace TDMaker.Core.Tests;

public sealed class AppSettingsTests
{
    [Fact]
    public void GetActiveProfileReturnsMatchingProfileWhenPresent()
    {
        var settings = new AppSettings
        {
            ActiveProfileId = ReleaseProfile.DefaultMusicProfileId
        };

        var active = settings.GetActiveProfile();

        Assert.Equal(ReleaseProfile.DefaultMusicProfileId, active.Id);
    }

    [Fact]
    public void GetActiveProfileFallsBackToFirstProfileWhenConfiguredIdIsMissing()
    {
        var settings = new AppSettings
        {
            ActiveProfileId = "missing"
        };

        var active = settings.GetActiveProfile();

        Assert.Equal(settings.Profiles[0].Id, active.Id);
    }
}
