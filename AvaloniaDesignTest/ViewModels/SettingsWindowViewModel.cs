using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using AvaloniaDesignTest.Models.Settings;
using DynamicData.Binding;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

//That is so BAD
public class SettingsWindowViewModel : ViewModelBase
{
    private bool _isDirty;

    public bool IsDirty
    {
        get => _isDirty;
        set => this.RaiseAndSetIfChanged(ref _isDirty, value);
    }
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
        CurrentSettings.WhenAnyPropertyChanged(nameof(Settings.GlobalSettings));

        Observable.Merge(
            Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => _settings.RequestSettings.PropertyChanged += handler,
                handler => _settings.RequestSettings.PropertyChanged -= handler
            ),
            Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => _settings.RequestSettings.ReleaseFormatSettings.PropertyChanged += handler,
                handler => _settings.RequestSettings.ReleaseFormatSettings.PropertyChanged -= handler
            ),
            Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => _settings.GeneralSettings.PropertyChanged += handler,
                handler => _settings.GeneralSettings.PropertyChanged -= handler
            )

        ).Subscribe(_ => SettingsChange());
    }

    public void SettingsChange()
    {
        IsDirty = true;
    }
    
    public async void ApplyChanges()
    {
        try
        {
            Settings.GlobalSettings = _settings.Clone() as Settings;
            await Settings.GlobalSettings?.Save();
            IsDirty = false;
            MainWindow.PopupMessage(MessageType.Success, "Settings applied");
        }
        catch
        {
            MainWindow.PopupMessage(MessageType.Error, "Error in applying changes");
        }
        
        
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
        SettingsChange();
    }
    
    public void RemoveMetadata(int index)
    {
        try
        {
            CurrentSettings.RequestSettings.ObservingMetadata.RemoveAt(index);
            SettingsChange();
        }
        catch
        {
            MainWindow.PopupMessage(MessageType.Error,"Element was not added");
        }
    }
}