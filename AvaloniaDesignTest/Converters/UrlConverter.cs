using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AvaloniaDesignTest.Converters;

public class UrlConverter :  IValueConverter
{
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string str = (string)value;
        Uri uri = new Uri(str);
        return $"Listen on: {uri.Host}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return "";
    }
}