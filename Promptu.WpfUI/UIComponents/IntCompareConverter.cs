using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class IntCompareConverter : IValueConverter
    {
        public IntCompareConverter()
        {
        }

        public int Value
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isEqual = ((int)value) == this.Value;

            if (parameter != null && parameter.ToString() == "invert")
            {
                isEqual = !isEqual;
            }

            return isEqual;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
