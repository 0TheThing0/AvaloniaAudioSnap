using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace AvaloniaDesignTest.ViewModels;

public class LibraryWindowViewModel : ViewModelBase
{ 
    public MainWindowViewModel MainWindow { get; init; }

    public ObservableCollection<MusicTrackViewModel> Tracks { get; } = new();
    
    private ReactiveCommand<Unit, Unit> TrackChoosed;
    public MusicTrackViewModel? CurrentMusicTrack { get; set; }
    
    public LibraryWindowViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
    }

    public void ShowChosenTrack()
    {
        int index = Tracks.IndexOf(CurrentMusicTrack);
        MainWindow.ShowResultWindow(CurrentMusicTrack);
        Tracks.Move(index,0);
    }
}