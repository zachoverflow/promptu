using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class HeightDifferenceConverter : DependencyObject, IMultiValueConverter
    {
        private Thickness padding;

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Thickness? inner = values[0] as Thickness?;
            FrameworkElement outer = values[0] as FrameworkElement;
            if (outer != null)
            {
                return outer.ActualHeight - (this.Padding.Top + this.Padding.Bottom);
            }

            return outer.ActualHeight;
        }

        public Thickness Padding
        {
            get { return this.padding; }
            set { this.padding = value; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
