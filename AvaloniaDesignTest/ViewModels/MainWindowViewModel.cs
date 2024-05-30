using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using AvaloniaDesignTest.Models.Settings;

namespace AvaloniaDesignTest.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    private LibraryWindowViewModel _libraryViewModel;
    private SettingsWindowViewModel _settingsViewModel;
    private SearchWindowViewModel _searchViewModel;
    private ErrorWindowViewModel _errorViewModel;
    private ResultWindowViewModel _resultViewModel;
    private Window _window;
    private bool _isSearch;
    
    public bool IsSearch
    {
        get => _isSearch;
        set => this.RaiseAndSetIfChanged(ref _isSearch, value);
    } 
    
     
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public Window Window
    {
        set => this.RaiseAndSetIfChanged(ref _window, value);
    }
    
    public MainWindowViewModel()
    {
        //Creating base viewmodels
        _searchViewModel = new SearchWindowViewModel(this);
        _errorViewModel = new ErrorWindowViewModel(this);
        _libraryViewModel = new LibraryWindowViewModel(this);
        
        //Trying to read settings from appsettings file
        try
        {
            using (FileStream fs = new FileStream("appsettings.json", FileMode.OpenOrCreate))
            {
                var des = new DataContractJsonSerializer(typeof(Settings));
                var settings = des.ReadObject(fs) as Settings;
                if (settings!=null)
                    Settings.GlobalSettings = settings;
            }
            
        }
        catch
        {
        }
        Settings.GlobalSettings.Save();
        //Creating Settings viewmodel with new settings
        _settingsViewModel = new SettingsWindowViewModel(this);

        
        _currentViewModel = _searchViewModel;
        
        //Observing search command from different view models to choose file from other places
        Observable.Merge(
            _searchViewModel.SearchCommand,
            _errorViewModel.SearchCommand).Subscribe(
            _ =>
                ChooseFile());
        
        //Adding to Schedule task to load history
        RxApp.MainThreadScheduler.Schedule(LoadTracks);
    }
    
    /// <summary>
    /// Method to show choose file dialog and call search this file to new result view model
    /// </summary>
    public async void ChooseFile()
    {
        var files = await _window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open music file",
            AllowMultiple = false
        });

        if (files.Count == 1)
        {
            await SearchFile(files.First());
        }
        
    }

    /// <summary>
    ///  //Method to show choose directory dialog
    /// </summary>
    /// <returns></returns>
    public async Task<IStorageFolder?> ChooseDir()
    {
        var files = await  _window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Open folder",
            AllowMultiple = false
        });

        if (files.Count == 1)
        {
            return files.First();
        }

        return null;
    }
    
    /// <summary>
    /// Method to create new resultViewModel add count fingerprint, metadata from file
    /// </summary>
    /// <param name="file"></param>
    public async Task SearchFile(IStorageFile file)
    {
        IsSearch = true;

        //Creating new result view model for new track
        CreateResultWindow();
        await Task.Run(() => _resultViewModel.Search(file));
        
        //Adding this track to history
        //TODO: redo adding
        var viewmodel =
            _libraryViewModel.Tracks.FirstOrDefault(x => x.Track.Filepath == _resultViewModel.Track.Track.Filepath, null);
        var trackPos = _libraryViewModel.Tracks.IndexOf(viewmodel);
        if (trackPos != -1)
        {
            _resultViewModel.Track.Track.LoadedFrom = viewmodel.Track.LoadedFrom;
            _libraryViewModel.Tracks[trackPos] = _resultViewModel.Track;
        }
        else
        {
            _libraryViewModel.Tracks.Add(_resultViewModel.Track);
        }


        IsSearch = false;
    }

    /// <summary>
    /// Method to create new resultViewModel
    /// </summary>
    private void CreateResultWindow()
    {
        _resultViewModel?.Track.Track.SaveAsync();
        _resultViewModel = new ResultWindowViewModel(this);
        _resultViewModel.SearchCommand.Subscribe(_ => ChooseFile());
        _resultViewModel.WrongInputCommand.Subscribe(_ => ShowErrorWindow());
        _resultViewModel.ShowResultCommand.Subscribe(_ => ShowResultWindow());
    }
    
    /// <summary>
    /// Method to save current track in resultViewModel
    /// </summary>
    public void Save()
    {
        _resultViewModel?.Track.Track.SaveAsync();
    }
    
    /// <summary>
    /// Method to show errorViewModel 
    /// </summary>
    public void ShowErrorWindow()
    {
        CurrentViewModel = _errorViewModel;
    }
    
    /// <summary>
    /// Method to show resultViewModel
    /// </summary>
    public async void ShowResultWindow()
    {
        CurrentViewModel = _resultViewModel is not null ? _resultViewModel : _searchViewModel;
    }
    
    /// <summary>
    /// Method to show searchViewModel
    /// </summary>
    public void ShowSearchWindow()
    {
        CurrentViewModel = _searchViewModel;
    }

    /// <summary>
    /// Method to show settingsViewModel
    /// </summary>
    public void ShowSettingsWindow()
    {
        //_settingsViewModel.CurrentSettings = Settings.GlobalSettings;
        CurrentViewModel = _settingsViewModel;
    }

    /// <summary>
    /// Method to show libraryViewModel
    /// </summary>
    public void ShowLibraryWindow()
    {
        CurrentViewModel = _libraryViewModel;
    }

    /// <summary>
    /// Method to show resultViewModel based on track
    /// </summary>
    /// <param name="trackViewModel"></param>
    public void ShowResultWindow(MusicTrackViewModel? trackViewModel)
    {
        if (trackViewModel is not null)
        {
            CreateResultWindow();
            _resultViewModel.Track = trackViewModel;
        }
        ShowResultWindow();
    }
    
    /// <summary>
    /// Method to load tracks from history directory (see settings)
    /// </summary>
    private async void LoadTracks()
    {
        //TODO: add in correct order with amount
        var tracks = await MusicTrackViewModel.LoadCachedAsync();
        foreach (var track in tracks)
        {
            _libraryViewModel.Tracks.Add(track);
        }

        foreach (var track in _libraryViewModel.Tracks.ToList())
        {
            //TODO: make async
            await track.Track.ConfigureData();
            await track.LoadCoverFromFile();
        }
    }
}