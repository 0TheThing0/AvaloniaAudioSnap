using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Views;

public partial class SettingsWindowView : UserControl
{
    public SettingsWindowView()
    {
        InitializeComponent();
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        (this.DataContext as SettingsWindowViewModel).AddMetadata(Metadata.Text);
    }

    private void Remove_OnClick(object? sender, RoutedEventArgs e)
    {
        (this.DataContext as SettingsWindowViewModel).RemoveMetadata(ListBox.SelectedIndex);
    }
    
}