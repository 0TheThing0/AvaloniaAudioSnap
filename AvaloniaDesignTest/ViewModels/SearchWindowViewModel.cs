using System.Reactive;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

public class SearchWindowViewModel : ViewModelBase
{
    public MainWindowViewModel MainWindow { get; init; }
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    
    public SearchWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
        SearchCommand = ReactiveCommand.Create(() => { });
    }

}