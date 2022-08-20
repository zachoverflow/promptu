using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class BooleanSwitchConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty IfTrueProperty =
            DependencyProperty.Register("IfTrue", typeof(object), typeof(BooleanSwitchConverter));

        public static readonly DependencyProperty IfFalseProperty =
            DependencyProperty.Register("IfFalse", typeof(object), typeof(BooleanSwitchConverter));

        public BooleanSwitchConverter()
        {
        }

        public object IfTrue
        {
            get { return this.GetValue(IfTrueProperty); }
            set { this.SetValue(IfTrueProperty, value); }
        }

        public object IfFalse
        {
            get { return this.GetValue(IfFalseProperty); }
            set { this.SetValue(IfFalseProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? this.IfTrue : this.IfFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
