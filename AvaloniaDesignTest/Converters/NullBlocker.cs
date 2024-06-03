using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaDesignTest.Converters;

public class NullBlocker: IValueConverter
{
    
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return (decimal)value;
            }
            else
            {
                return  BindingOperations.DoNothing;
            }
        }
    }
