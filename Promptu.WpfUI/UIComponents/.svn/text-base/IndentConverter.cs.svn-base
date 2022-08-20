using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class IndentConverter : IValueConverter
    {
        public IndentConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Thickness baseThickness = new Thickness();

            string stringParameter = parameter as string;
            if (stringParameter != null)
            {
                baseThickness = (Thickness)new ThicknessConverter().ConvertFromString(null, culture, stringParameter);
            }

            baseThickness.Left += ((int)value) * 18;

            return baseThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
