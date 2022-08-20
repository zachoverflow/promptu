using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class TextEntryTypeConverter : DependencyObject, IValueConverter
    {
        public static DependencyProperty ErrorProperty =
            DependencyProperty.Register(
                "Error",
                typeof(object),
                typeof(TextEntryTypeConverter));

        public static DependencyProperty NormalProperty =
            DependencyProperty.Register(
                "Normal",
                typeof(object),
                typeof(TextEntryTypeConverter));

        public object Error
        {
            get { return this.GetValue(ErrorProperty); }
            set { this.SetValue(ErrorProperty, value); }
        }

        public object Normal
        {
            get { return this.GetValue(NormalProperty); }
            set { this.SetValue(NormalProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TextEntryType type = (TextEntryType)value;

            switch (type)
            {
                case TextEntryType.Error:
                    return this.Error;
                default:
                    return this.Normal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
