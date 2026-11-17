using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using OOP_lr2.Services;

namespace OOP_lr2;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if WINDOWS
        // Підтвердження виходу також при закритті вікна хрестиком.
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(windows => windows.OnWindowCreated(window =>
            {
                var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow.Closing += async (sender, args) =>
                {
                    if (ExitConfirmation.IsConfirmed)
                    {
                        return;
                    }

                    args.Cancel = true;
                    var page = Application.Current?.Windows.FirstOrDefault()?.Page;
                    if (page != null && await ExitConfirmation.AskAsync(page))
                    {
                        Application.Current?.Quit();
                    }
                };
            }));
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
