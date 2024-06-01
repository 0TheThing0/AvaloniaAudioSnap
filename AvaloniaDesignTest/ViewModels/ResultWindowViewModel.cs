using System;
using System.Reactive;
using Avalonia.Platform.Storage;
using Chromaprint;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

public class ResultWindowViewModel : ViewModelBase
{
    private MusicTrackViewModel _trackViewModel;

    public MusicTrackViewModel Track
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
        var res = await Track.ApplyChanges().ConfigureAwait(false);
        if (res)
        {
            MainWindow.PopupMessage(MessageType.Error,"Error in applying changes");
        }
        else
        {
            MainWindow.PopupMessage(MessageType.Success,"Changes applied");
        }
    }

    public async void SearchFileOnline()
    {
        await Track.SearchFileOnline();
    }
    
    public async void Search(IStorageFile file)
    {
        FileChromaContext chromaContext = new FileChromaContext();
        if (!chromaContext.ComputeFingerprint(file.Path.LocalPath, 1000))
        {
            WrongInputCommand.Execute().Subscribe();
        }
        else
        {
            _trackViewModel = new MusicTrackViewModel(file.Path.LocalPath, chromaContext.GetFingerprint());
            ShowResultCommand.Execute().Subscribe();
            await _trackViewModel.Analyze();
        }
    }
}