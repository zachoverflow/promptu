using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class RgbHexConverter : IValueConverter
    {
        public RgbHexConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = (Color)value;
            StringBuilder builder = new StringBuilder();

            builder.Append("#");
            builder.Append(color.R.ToString("X2", culture));
            builder.Append(color.G.ToString("X2", culture));
            builder.Append(color.B.ToString("X2", culture));

            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
