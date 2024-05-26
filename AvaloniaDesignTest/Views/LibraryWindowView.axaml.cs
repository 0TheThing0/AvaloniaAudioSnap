using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaDesignTest.ViewModels;
using ReactiveUI;

namespace AvaloniaDesignTest.Views;

public partial class LibraryWindowView : UserControl
{
    public LibraryWindowView()
    {
        InitializeComponent();
    }
    
    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        var vm = this.DataContext as LibraryWindowViewModel;
        if (vm != null)
        {
            vm.ShowChosenTrack();
        }
    }
}