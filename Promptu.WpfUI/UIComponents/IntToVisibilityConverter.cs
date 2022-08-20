using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class IntToVisibilityConverter : IValueConverter
    {
        public IntToVisibilityConverter()
        {
        }

        public Visibility IfIs
        {
            get;
            set;
        }

        public Visibility IfNot
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int compareTo = System.Convert.ToInt32(parameter, culture);

            return (int)value != compareTo ? this.IfNot : this.IfIs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
