using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using AvaloniaDesignTest.Views;
using Chromaprint;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

public class ResultWindowViewModel : ViewModelBase
{
    private MusicTrackViewModel _trackViewModel;

    public MusicTrackViewModel TrackViewModel
    {
        get => _trackViewModel;
        set => this.RaiseAndSetIfChanged(ref _trackViewModel, value);
    }
    
    public MainWindowViewModel MainWindow { get; init; }
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowResultCommand { get; }
    public ReactiveCommand<Unit, Unit> WrongInputCommand { get; }
    
    public ResultWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
        WrongInputCommand = ReactiveCommand.Create(() => {});
        ShowResultCommand = ReactiveCommand.Create(() => {});
        SearchCommand = ReactiveCommand.Create(() => { });
    }

    public async void ApplyChanges()
    {
        TrackViewModel.ApplyChanges();
    }
    public async void Search(IStorageFile file)
    {
        FileChromaContext chromaContext = new FileChromaContext();
        if (!chromaContext.ComputeFingerprint(file.Path.LocalPath))
        {
            WrongInputCommand.Execute().Subscribe();
            return;
        }
        else
        {
            _trackViewModel = new MusicTrackViewModel(file.Path.LocalPath, chromaContext.GetFingerprint());
            ShowResultCommand.Execute().Subscribe();
            await _trackViewModel.Search();
        }
    }
}