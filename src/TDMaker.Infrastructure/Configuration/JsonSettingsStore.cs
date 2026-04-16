namespace TDMaker.Infrastructure.Configuration;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Models;

public sealed class JsonSettingsStore(
    IPlatformPaths platformPaths,
    ILogger<JsonSettingsStore> logger) : ISettingsStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public async Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default)
    {
        EnsureDirectories();

        if (!File.Exists(platformPaths.SettingsFilePath))
        {
            var fresh = new AppSettings();
            await SaveAsync(fresh, cancellationToken);
            return fresh;
        }

        await using var stream = File.OpenRead(platformPaths.SettingsFilePath);
        var loaded = await JsonSerializer.DeserializeAsync<AppSettings>(stream, SerializerOptions, cancellationToken)
            ?? new AppSettings();

        logger.LogInformation("Loaded settings from {SettingsFilePath}", platformPaths.SettingsFilePath);
        return loaded;
    }

    public async Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default)
    {
        EnsureDirectories();

        await using var stream = File.Create(platformPaths.SettingsFilePath);
        await JsonSerializer.SerializeAsync(stream, settings, SerializerOptions, cancellationToken);
        logger.LogInformation("Saved settings to {SettingsFilePath}", platformPaths.SettingsFilePath);
    }

    private void EnsureDirectories()
    {
        Directory.CreateDirectory(platformPaths.AppRoot);
        Directory.CreateDirectory(platformPaths.LogsDirectory);
        Directory.CreateDirectory(platformPaths.ToolsDirectory);
        Directory.CreateDirectory(platformPaths.DefaultWorkspaceDirectory);
    }
}
