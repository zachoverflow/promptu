using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ColorInverterConverter : IValueConverter
    {
        public ColorInverterConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color old = (Color)value;
            return Color.FromArgb(0, (byte)(255 - old.R), (byte)(255 - old.G), (byte)(255 - old.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
