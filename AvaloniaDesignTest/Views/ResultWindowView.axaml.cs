using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Views;

public partial class ResultWindowView : UserControl
{
    public ResultWindowView()
    {
        InitializeComponent();
    }

    public async void ShowUrl(string path)
    {
        Process.Start(new ProcessStartInfo(path) {UseShellExecute = true}) ;
    }
}