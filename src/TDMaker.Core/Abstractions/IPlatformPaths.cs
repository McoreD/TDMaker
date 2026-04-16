namespace TDMaker.Core.Abstractions;

public interface IPlatformPaths
{
    string AppRoot { get; }
    string SettingsFilePath { get; }
    string LogsDirectory { get; }
    string ToolsDirectory { get; }
    string DefaultWorkspaceDirectory { get; }
    string BundledTemplatesDirectory { get; }
}
