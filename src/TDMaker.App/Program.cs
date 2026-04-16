using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TDMaker.App.ViewModels;
using TDMaker.Infrastructure.DependencyInjection;

namespace TDMaker.App;

internal static class Program
{
    public static IHost AppHost { get; private set; } = null!;

    [STAThread]
    public static void Main(string[] args)
    {
        AppHost = CreateHost(args);
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        AppHost.Dispose();
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();

    private static IHost CreateHost(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddDebug();
        builder.Logging.AddSimpleConsole();
        builder.Services.AddTDMakerInfrastructure();
        builder.Services.AddSingleton<MainWindowViewModel>();
        return builder.Build();
    }
}
