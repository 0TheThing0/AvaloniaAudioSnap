using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;
using Microsoft.Extensions.Configuration;
using System.Reactive.Concurrency;
using System.Text.Json;
using AvaloniaDesignTest.Models.Settings;
using Chromaprint;
using TagLib;

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
    
    public MainWindowViewModel()
    {
        _searchViewModel = new SearchWindowViewModel(this);
        _errorViewModel = new ErrorWindowViewModel(this);
        _libraryViewModel = new LibraryWindowViewModel(this);

        try
        {
            using (FileStream fs = new FileStream("appsettings.json", FileMode.OpenOrCreate))
            {
                var settings = JsonSerializer.Deserialize<Settings>(fs);
                if (settings!=null)
                    Settings.GlobalSettings = settings;
            }
            
        }
        catch
        {
            
        }
        _settingsViewModel = new SettingsWindowViewModel(this);

        _currentViewModel = _searchViewModel;
        
        Observable.Merge(
            _searchViewModel.SearchCommand,
            _errorViewModel.SearchCommand).Subscribe(
            _ =>
                ChooseFile());
        RxApp.MainThreadScheduler.Schedule(LoadTracks);
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

    public async Task<IStorageFolder> ChooseDir()
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
    #region Change Window
    
    public async Task SearchFile(IStorageFile file)
    {
        IsSearch = true;
        
        CreateResultWindow();
        if (_resultViewModel is not null)
        {
            await Task.Run(() => _resultViewModel.Search(file));
            var track = _libraryViewModel.Tracks.FirstOrDefault(x => x.Filepath == _resultViewModel.Track.Filepath, null);
            var trackPos = _libraryViewModel.Tracks.IndexOf(track);
            if (trackPos != -1)
            {
                _resultViewModel.Track.LoadedFrom = track.LoadedFrom;
                _libraryViewModel.Tracks[trackPos] = _resultViewModel.Track;
            }
            else
            {
                _libraryViewModel.Tracks.Add(_resultViewModel.Track);
            }
        }

        IsSearch = false;
    }

    private void CreateResultWindow()
    {
        _resultViewModel?.Track.SaveAsync();
        _resultViewModel = new ResultWindowViewModel(this);
        _resultViewModel.SearchCommand.Subscribe(_ => ChooseFile());
        _resultViewModel.WrongInputCommand.Subscribe(_ => ShowErrorWindow());
        _resultViewModel.ShowResultCommand.Subscribe(_ => ShowResultWindow());
    }
    
    public void Save()
    {
        _resultViewModel?.Track.SaveAsync();
    }
    
    public void ShowErrorWindow()
    {
        CurrentViewModel = _errorViewModel;
    }
    
    public async void ShowResultWindow()
    {
        if (_resultViewModel != null)
        {
            CurrentViewModel = _resultViewModel;
        }
        else
        {
            CurrentViewModel = _searchViewModel;
        }
    }
    
    public void ShowSearchWindow()
    {
        CurrentViewModel = _searchViewModel;
    }

    public void ShowSettingsWindow()
    {
        //_settingsViewModel.CurrentSettings = Settings.GlobalSettings;
        CurrentViewModel = _settingsViewModel;
    }

    public void ShowLibraryWindow()
    {
        CurrentViewModel = _libraryViewModel;
    }

    public void ShowResultWindow(MusicTrackViewModel? trackViewModel)
    {
        if (trackViewModel is not null)
        {
            CreateResultWindow();
            _resultViewModel.Track = trackViewModel;
        }
        ShowResultWindow();
    }
    #endregion
    
    private async void LoadTracks()
    {
        var tracks = await MusicTrackViewModel.LoadCachedAsync();
        foreach (var track in tracks)
        {
            _libraryViewModel.Tracks.Add(track);
        }

        foreach (var track in _libraryViewModel.Tracks.ToList())
        {
            //TODO: make async
            await track.ConfigureData();
            await track.LoadCoverFromFile();
        }
    }
}