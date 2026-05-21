using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PatterningOperatorDashboard.Views;

/// <summary>Converts a hex string like "#RRGGBB" to a <see cref="Color"/> for swatch rendering.</summary>
public sealed class HexToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string hex && !string.IsNullOrWhiteSpace(hex))
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(hex)!;
            }
            catch
            {
                return Colors.Transparent;
            }
        }
        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
