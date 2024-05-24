using System.Reactive;
using AvaloniaDesignTest.Views;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

public class ErrorWindowViewModel : ViewModelBase
{
    public MainWindowViewModel MainWindow { get; init; }
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    public ErrorWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
        SearchCommand = ReactiveCommand.Create(() => { });
    }
}