namespace AvaloniaDesignTest.ViewModels;
public class SettingsWindowViewModel : ViewModelBase
{
    public MainWindowViewModel MainWindow { get; init; }
    public SettingsWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
    }
}