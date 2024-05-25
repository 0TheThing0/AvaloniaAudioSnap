using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;
using Microsoft.Extensions.Configuration;
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
        _settingsViewModel = new SettingsWindowViewModel(this);
        _libraryViewModel = new LibraryWindowViewModel(this);
        
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        var res=config.GetRequiredSection("Hello").Get<MetadataSettings>();

        _currentViewModel = _searchViewModel;
        
        Observable.Merge(
            _searchViewModel.SearchCommand,
            _errorViewModel.SearchCommand).Subscribe(
            _ =>
                ChooseFile());

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

        if (files.Count >= 1)
        {
            await SearchFile(files.First());
        }
    }

    #region Change Window
    
    public async Task SearchFile(IStorageFile file)
    {
        IsSearch = true;
        
        _resultViewModel = new ResultWindowViewModel(this);
        _resultViewModel.SearchCommand.Subscribe(_ => ChooseFile());
        _resultViewModel.WrongInputCommand.Subscribe(_ => ShowErrorWindow());
        _resultViewModel.ShowResultCommand.Subscribe(_ => ShowResultWindow());
        
        var r = Task.Run(() => _resultViewModel.Search(file));
        await r;
        IsSearch = false;
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
        CurrentViewModel = _settingsViewModel;
    }

    public void ShowLibraryWindow()
    {
        CurrentViewModel = _libraryViewModel;
    }
    #endregion
}