using System;
using Windows.UI.Xaml.Data;

namespace Reddit.Utilities.Converters
{
    internal class EntryDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime) value;
            var offset = DateTime.Now - date;
            var result =
                (int) offset.TotalDays > 0 ? $"{(int) offset.TotalDays}d ago" :
                    (int) offset.TotalHours > 0 ? $"{(int) offset.TotalHours}h ago" :
                        (int) offset.TotalMinutes > 0 ? $"{(int) offset.TotalMinutes}m ago" :
                            "just now";

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}