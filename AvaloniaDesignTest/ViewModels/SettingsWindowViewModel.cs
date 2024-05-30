using AvaloniaDesignTest.Models.Settings;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

//That is so BAD
public class SettingsWindowViewModel : ViewModelBase
{
    public Settings CurrentSettings
    {
        get => _settings;
        set => this.RaiseAndSetIfChanged(ref _settings, value);
    }
    
    public string CoverPath
    {
        get => _coverPath;
        set
        {
            this.RaiseAndSetIfChanged(ref _coverPath, value);
            CurrentSettings.GeneralSettings.CoverPath = _coverPath;
        }
    }
    
    public string HistoryPath
    {
        get => _historyPath;
        set
        {
            this.RaiseAndSetIfChanged(ref _historyPath, value);
            CurrentSettings.GeneralSettings.HistoryPath = _historyPath;
        }
    }

    public MainWindowViewModel MainWindow { get; init; }
    
    private Settings _settings;
    private string _coverPath;
    private string _historyPath;
    
    public SettingsWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
        _settings = Settings.GlobalSettings.Clone() as Settings;
        _coverPath = Settings.GlobalSettings.GeneralSettings.CoverPath;
        _historyPath = Settings.GlobalSettings.GeneralSettings.HistoryPath;
    }
    
    public void ApplyChanges()
    {
        Settings.GlobalSettings = _settings.Clone() as Settings;
    }

    public async void ChooseCoverPath()
    {
        var result = await MainWindow.ChooseDir();
        if (result != null)
        {
            CoverPath = result.Path.LocalPath;
        }
    }
    
    public async void ChooseHistoryPath()
    {
        var result = await MainWindow.ChooseDir();
        if (result != null)
        {
            HistoryPath = result.Path.LocalPath;
        }
    }

    public void AddMetadata(string value)
    {
        if (!CurrentSettings.RequestSettings.ObservingMetadata.Contains(value))
        {
            CurrentSettings.RequestSettings.ObservingMetadata.Add(value);
        }
    }
    
    public void RemoveMetadata(int index)
    {
        try
        {
            CurrentSettings.RequestSettings.ObservingMetadata.RemoveAt(index);
        }
        catch
        {
            
        }
    }
}