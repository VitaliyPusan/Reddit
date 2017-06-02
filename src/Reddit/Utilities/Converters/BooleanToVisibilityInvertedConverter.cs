using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Reddit.Utilities.Converters
{
    public class BooleanToVisibilityInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool))
            {
                return Visibility.Visible;
            }

            var result = !(bool)value ? Visibility.Visible : Visibility.Collapsed;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}