using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class VisibilityConverter : IValueConverter
    {
        public VisibilityConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool compareValue = (bool)value;
            if (parameter != null && parameter.ToString() == "invert")
            {
                compareValue = !compareValue;
            }

            return compareValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Collapsed:
                case Visibility.Hidden:
                    return false;
                default:
                    return true;
            }
        }
    }
}
