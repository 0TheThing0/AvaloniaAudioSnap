using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        var view = this.DataContext as MainWindowViewModel;
        if (view != null)
        {
            view.Save();
        }
    }
}