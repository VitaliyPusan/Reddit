using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Reddit.Utilities.Converters
{
    internal class ThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var thumbnail = (string) value;
            var source = thumbnail.StartsWith("http") ? thumbnail : "ms-appx:///Assets/Thumbnail-Defailt.png";
            var result = new BitmapImage(new Uri(source, UriKind.Absolute));
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}