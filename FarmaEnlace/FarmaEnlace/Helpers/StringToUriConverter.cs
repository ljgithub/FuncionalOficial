using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return string.Empty;

                string data = value as string;

                if (!string.IsNullOrEmpty(data) && data.Equals("noimage", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty;
                }
                return new Uri(data);
            }
            catch (Exception)
            {
                return new Uri("pack://application:,,,/Images/LedGreen.png");
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting a Uri back to a string isn't supported (yet) ");
        }
    }
}
