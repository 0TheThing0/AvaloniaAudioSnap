using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Converters;

public class TypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch ((MessageType)value)
        {
            case MessageType.Info:
                return new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 0, 0, 0));
            case MessageType.Error:
                return new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 168, 40, 57));
            case MessageType.Success:
                return new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 96, 207, 46));
        }
        return new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 0, 0, 0));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return true;
    }
}