﻿using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using AvaloniaDesignTest.Views;
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
    
    public string CurrentItem { get; set; }
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

    public async Task SearchFileOnline()
    {
        MainWindow.IsSearch = true;
        var message = await Track.SearchFileOnline();
        MainWindow.PopupMessage(message);
        MainWindow.IsSearch = false;
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

    public async void ShowUrl()
    {
        Process.Start(new ProcessStartInfo(CurrentItem) {UseShellExecute = true}) ;
    }
    
}