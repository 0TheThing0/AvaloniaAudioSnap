using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaDesignTest.ViewModels;

namespace AvaloniaDesignTest.Converters;

public class NameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch ((MessageType)value)
        {
            case MessageType.Info:
                return "Info";
            case MessageType.Error:
                return "Error!";
            case MessageType.Success:
                return "Success";
        }

        return "Unknown";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return true;
    }
}