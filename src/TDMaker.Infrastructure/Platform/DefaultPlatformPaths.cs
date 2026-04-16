namespace TDMaker.Infrastructure.Platform;

using TDMaker.Core.Abstractions;

public sealed class DefaultPlatformPaths : IPlatformPaths
{
    public string AppRoot { get; } = ResolveRoot();
    public string SettingsFilePath => Path.Combine(AppRoot, "settings.json");
    public string LogsDirectory => Path.Combine(AppRoot, "logs");
    public string ToolsDirectory => Path.Combine(AppRoot, "tools");
    public string DefaultWorkspaceDirectory => Path.Combine(AppRoot, "workspace");
    public string BundledTemplatesDirectory => Path.Combine(AppContext.BaseDirectory, "Templates");

    private static string ResolveRoot()
    {
        var overrideRoot = Environment.GetEnvironmentVariable("TDMAKER_HOME");
        if (!string.IsNullOrWhiteSpace(overrideRoot))
        {
            return overrideRoot;
        }

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, "TDMaker");
    }
}
