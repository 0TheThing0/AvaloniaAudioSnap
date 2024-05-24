using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaDesignTest.ViewModels;
using AvaloniaDesignTest.Views;

namespace AvaloniaDesignTest;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            (desktop.MainWindow.DataContext as MainWindowViewModel).Window = desktop.MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}