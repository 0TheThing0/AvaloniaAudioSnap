using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Color = System.Drawing.Color;

namespace AvaloniaDesignTest.Converters;

public class ColorConverter : IValueConverter
{
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return ((bool)value)
                ? new SolidColorBrush(new Avalonia.Media.Color(255, 168, 40, 57))
                : new SolidColorBrush(new Avalonia.Media.Color(255, 111, 185, 143));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return true;
        }
}
