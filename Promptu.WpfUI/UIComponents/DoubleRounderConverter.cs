using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class DoubleRounderConverter : IValueConverter
    {
        public DoubleRounderConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double number = (double)value;

            int digits = 2;

            if (parameter is int)
            {
                digits = (int)parameter;
            }

            return Math.Round(number, digits);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
