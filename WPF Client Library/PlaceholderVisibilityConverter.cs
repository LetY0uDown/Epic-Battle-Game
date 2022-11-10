using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_Client_Library;

internal sealed class PlaceholderVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace(value.ToString()) ? Visibility.Visible 
                                                           : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
}