namespace TDMaker.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using TDMaker.Core.Abstractions;
using TDMaker.Core.Workflows;
using TDMaker.Infrastructure.Configuration;
using TDMaker.Infrastructure.Media;
using TDMaker.Infrastructure.Platform;
using TDMaker.Infrastructure.Publishing;
using TDMaker.Infrastructure.Screenshots;
using TDMaker.Infrastructure.Torrents;
using TDMaker.Infrastructure.Uploads;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTDMakerInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IPlatformPaths, DefaultPlatformPaths>();
        services.AddSingleton<ISettingsStore, JsonSettingsStore>();
        services.AddSingleton<IProcessRunner, ProcessRunner>();
        services.AddSingleton<IExternalToolLocator, ExternalToolLocator>();
        services.AddSingleton<IMediaInspector, MediaInfoCliInspector>();
        services.AddSingleton<IScreenshotService, FFmpegScreenshotService>();
        services.AddSingleton<IImageUploadService, PtpImgUploadService>();
        services.AddSingleton<IPublishService, LegacyTemplatePublishService>();
        services.AddSingleton<ITorrentService, MonoTorrentService>();
        services.AddSingleton<IReleaseWorkflow, ReleaseWorkflow>();
        return services;
    }
}
