using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_Client.UITools;

internal sealed class HealthConverter : IValueConverter
{
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"ХП: {value}/{parameter}";
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) => value;
}