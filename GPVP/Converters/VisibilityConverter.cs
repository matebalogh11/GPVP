using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GPVP.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( targetType == typeof(Visibility))
            {
                if ( value is bool boolValue)
                {
                    return boolValue ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
