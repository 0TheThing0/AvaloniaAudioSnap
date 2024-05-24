namespace AvaloniaDesignTest.ViewModels;

public class LibraryWindowViewModel : ViewModelBase
{ 
    public MainWindowViewModel MainWindow { get; init; }

    public LibraryWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
    }
}